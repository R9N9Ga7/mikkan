using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Server.Interfaces.Repositories;
using Server.Interfaces.Services;
using Server.Models;
using Server.Models.Requests;
using Server.Models.Responses;
using Server.Settings;
using System.Net;
using System.Net.Http.Json;
using Tests.Common;
using Tests.Data;
using Tests.Factories;

namespace Tests.Integration;

public class AccountControllerTest : IntegrationTestBase
{
    public AccountControllerTest(WebApplicationFactoryBase factory)
        : base(factory)
    {
        _userRepository = GetService<IUserRepository>();
        _tokenService = GetService<ITokenService>();
    }

    [Fact]
    public async Task CreatingUserAccount()
    {
        var userCreateRequest = UserData.GetUserCreateRequest();

        var response = await Post(UrlCreate, userCreateRequest, false);
        response.EnsureSuccessStatusCode();

        var isUserExists = await _userRepository.IsExists(userCreateRequest.Username);
        isUserExists.Should().BeTrue();
    }

    [Fact]
    public async Task UserPasswordIsHashed()
    {
        var userCreateRequest = UserData.GetUserCreateRequest();

        var response = await Post(UrlCreate, userCreateRequest, false);
        response.EnsureSuccessStatusCode();

        var user = await _userRepository.GetByUsername(userCreateRequest.Username);
        user.Should().NotBeNull();
        user?.Password.Should().NotBe(userCreateRequest.Password);
    }

    [Fact]
    public async Task ShouldNotByAbleToCreateTwoUsersWithIdenticalUsernames()
    {
        var userCreateRequest = UserData.GetUserCreateRequest();

        var firstUser = await Post(UrlCreate, userCreateRequest, false);
        firstUser.EnsureSuccessStatusCode();

        var secondUser = await Post(UrlCreate, userCreateRequest, false);
        secondUser.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [ClassData(typeof(UserDataInvalid))]
    public async Task CreatingUserWithInvalidData(UserCreateRequest userCreateRequest)
    {
        var secondUser = await Post(UrlCreate, userCreateRequest, false);
        secondUser.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldRejectRegistrationIfUsersMoreThanLimit()
    {
        var factory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.Configure<AccountSettings>(options =>
                {
                    options.SaltSize = 16;
                    options.KeySize = 32;
                    options.Iterations = 10000;
                    options.UserRegistrationsLimit = 1;
                });
            });
        });

        var client = factory.CreateClient();

        {
            var userCreateRequest = UserData.GetUserCreateRequest();

            var response = await client.PostAsJsonAsync(UrlCreate, userCreateRequest);
            response.EnsureSuccessStatusCode();
        }

        {
            var userCreateRequest = UserData.GetUserCreateRequest();

            var response = await client.PostAsJsonAsync(UrlCreate, userCreateRequest);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }

    [Fact]
    public async Task FreeRegistrationWithZeroLimit()
    {
        var factory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.Configure<AccountSettings>(options =>
                {
                    options.SaltSize = 16;
                    options.KeySize = 32;
                    options.Iterations = 10000;
                    options.UserRegistrationsLimit = 0;
                });
            });
        });

        var client = factory.CreateClient();

        {
            var userCreateRequest = UserData.GetUserCreateRequest();

            var response = await client.PostAsJsonAsync(UrlCreate, userCreateRequest);
            response.EnsureSuccessStatusCode();
        }

        {
            var userCreateRequest = UserData.GetUserCreateRequest();

            var response = await client.PostAsJsonAsync(UrlCreate, userCreateRequest);
            response.EnsureSuccessStatusCode();
        }
    }

    [Fact]
    public async Task LoginWithValidCredentials()
    {
        var userCreateRequest = UserData.GetUserCreateRequest();

        var user = await Post(UrlCreate, userCreateRequest, false);
        user.EnsureSuccessStatusCode();

        var loginResponse = await Post(UrlLogin, userCreateRequest, false);
        loginResponse.EnsureSuccessStatusCode();

        var loginResponseContent = await DeserializeResponse<Response<UserTokensResponse>>(loginResponse);
        loginResponseContent.Should().NotBeNull();

        var loginContent = loginResponseContent.Content;
        loginContent.Should().NotBeNull();
        loginContent.AccessToken.Should().NotBeEmpty();
        loginContent.RefreshToken.Should().NotBeEmpty();

        var refreshTokenValidationResult = await _tokenService.ValidateToken(loginContent.RefreshToken);
        refreshTokenValidationResult.IsValid.Should().BeTrue();

        var accessTokenValidationResult = await _tokenService.ValidateToken(loginContent.AccessToken);
        accessTokenValidationResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task LoginWithInvalidCredentials()
    {
        var userCreateRequest = new UserCreateRequest {
            Username = "WrongUsername",
            Password = "WrongPassword",
        };
        var loginResponse = await Post(UrlLogin, userCreateRequest, false);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task RefreshValidTokens()
    {
        var userCreateRequest = UserData.GetUserCreateRequest();

        var userCreateResponse = await Post(UrlCreate, userCreateRequest, false);
        userCreateResponse.EnsureSuccessStatusCode();

        var loginResponse = await Post(UrlLogin, userCreateRequest, false);
        loginResponse.EnsureSuccessStatusCode();

        var loginResponseContent = await DeserializeResponse<Response<UserTokensResponse>>(loginResponse);
        loginResponseContent.Should().NotBeNull();

        var loginContent = loginResponseContent.Content;
        loginContent.Should().NotBeNull();
        loginContent.AccessToken.Should().NotBeEmpty();
        loginContent.RefreshToken.Should().NotBeEmpty();

        var refreshTokensResponse = await Post(UrlRefresh, loginContent, false);
        refreshTokensResponse.EnsureSuccessStatusCode();

        var refreshTokensResponseContent = await DeserializeResponse<Response<UserTokensResponse>>(refreshTokensResponse);
        refreshTokensResponseContent.Should().NotBeNull();

        var refreshTokensContent = refreshTokensResponseContent.Content;
        refreshTokensContent.Should().NotBeNull();

        var refreshTokenValidationResult = await _tokenService.ValidateToken(refreshTokensContent.RefreshToken);
        refreshTokenValidationResult.IsValid.Should().BeTrue();

        var accessTokenValidationResult = await _tokenService.ValidateToken(refreshTokensContent.AccessToken);
        accessTokenValidationResult.IsValid.Should().BeTrue();

        loginContent.AccessToken.Should().NotBe(refreshTokensContent.AccessToken);
        loginContent.RefreshToken.Should().NotBe(refreshTokensContent.RefreshToken);
    }

    [Fact]
    public async Task RefreshInvalidTokens()
    {
        var userTokensRequest = new UserTokensRequest {
            AccessToken = "AccessToken",
            RefreshToken = "RefreshToken"
        };

        var refreshTokensResponse = await Post(UrlRefresh, userTokensRequest, false);
        refreshTokensResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    const string Url = "/api/account";
    const string UrlCreate = $"{Url}/create";
    const string UrlLogin = $"{Url}/login";
    const string UrlRefresh = $"{Url}/refresh";

    readonly IUserRepository _userRepository;
    readonly ITokenService _tokenService;
}
