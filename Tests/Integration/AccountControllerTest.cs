using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Server.Interfaces.Repositories;
using Server.Models.Requests;
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
        user.Password.Should().NotBe(userCreateRequest.Password);
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

    const string Url = "/api/account";

    readonly IUserRepository _userRepository;
}
