using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Server.Interfaces.Repositories;
using Server.Interfaces.Services;
using Server.Models.Requests;
using Server.Models.Responses;
using Server.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
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
        _jwtKeyService = GetService<IJwtKeyService>();
    }

    [Fact]
    public async Task CreatingUserAccount()
    {
        var userCreateRequest = UserData.GetUserCreateRequest();

        var response = await Post($"{Url}/create", userCreateRequest);
        response.EnsureSuccessStatusCode();

        var isUserExists = await _userRepository.IsExists(userCreateRequest.Username);
        isUserExists.Should().BeTrue();
    }

    [Fact]
    public async Task UserPasswordIsHashed()
    {
        var userCreateRequest = UserData.GetUserCreateRequest();

        var response = await Post($"{Url}/create", userCreateRequest);
        response.EnsureSuccessStatusCode();

        var user = await _userRepository.GetByUsername(userCreateRequest.Username);
        user.Should().NotBeNull();
        user?.Password.Should().NotBe(userCreateRequest.Password);
    }

    [Fact]
    public async Task ShouldNotByAbleToCreateTwoUsersWithIdenticalUsernames()
    {
        var userCreateRequest = UserData.GetUserCreateRequest();

        var firstUser = await Post($"{Url}/create", userCreateRequest);
        firstUser.EnsureSuccessStatusCode();

        var secondUser = await Post($"{Url}/create", userCreateRequest);
        secondUser.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [ClassData(typeof(UserDataInvalid))]
    public async Task CreatingUserWithInvalidData(UserCreateRequest userCreateRequest)
    {
        var secondUser = await Post($"{Url}/create", userCreateRequest);
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

            var response = await client.PostAsJsonAsync($"{Url}/create", userCreateRequest);
            response.EnsureSuccessStatusCode();
        }

        {
            var userCreateRequest = UserData.GetUserCreateRequest();

            var response = await client.PostAsJsonAsync($"{Url}/create", userCreateRequest);
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

            var response = await client.PostAsJsonAsync($"{Url}/create", userCreateRequest);
            response.EnsureSuccessStatusCode();
        }

        {
            var userCreateRequest = UserData.GetUserCreateRequest();

            var response = await client.PostAsJsonAsync($"{Url}/create", userCreateRequest);
            response.EnsureSuccessStatusCode();
        }
    }

    [Fact]
    public async Task LoginWithValidCredentials()
    {
        var userCreateRequest = UserData.GetUserCreateRequest();

        var user = await Post($"{Url}/create", userCreateRequest);
        user.EnsureSuccessStatusCode();

        var loginResponse = await Post($"{Url}/login", userCreateRequest);
        loginResponse.EnsureSuccessStatusCode();

        var loginContent = await DeserializeResponse<UserLoginResponse>(loginResponse);
        loginContent.Should().NotBeNull();
        loginContent.AccessToken.Should().NotBeEmpty();
        loginContent.RefreshToken.Should().NotBeEmpty();

        var tokenHandler = new JwtSecurityTokenHandler();

        {
            var claimPrincipal = tokenHandler.ValidateToken(
                loginContent.RefreshToken,
                _jwtKeyService.GetTokenValidationParameters(),
                out _);

            claimPrincipal.HasClaim(ClaimTypes.Name, userCreateRequest.Username).Should().BeTrue();
        }

        {
            var claimPrincipal = tokenHandler.ValidateToken(
                loginContent.AccessToken,
                _jwtKeyService.GetTokenValidationParameters(),
                out _);

            claimPrincipal.HasClaim(ClaimTypes.Name, userCreateRequest.Username).Should().BeTrue();
        }
    }

    [Fact]
    public async Task LoginWithInvalidCredentials()
    {
        var userCreateRequest = new UserCreateRequest {
            Username = "WrongUsername",
            Password = "WrongPassword",
        };
        var loginResponse = await Post($"{Url}/login", userCreateRequest);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    const string Url = "/api/account";

    readonly IUserRepository _userRepository;
    readonly IJwtKeyService _jwtKeyService;
}
