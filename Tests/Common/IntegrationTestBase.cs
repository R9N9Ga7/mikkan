using Microsoft.Extensions.DependencyInjection;
using Server.Interfaces.Repositories;
using Server.Interfaces.Services;
using Server.Models.Dtos;
using Server.Models.Entities;
using System.Net.Http.Json;
using Tests.Factories;

namespace Tests.Common;

public class IntegrationTestBase : IClassFixture<WebApplicationFactoryBase>
{
    public IntegrationTestBase(WebApplicationFactoryBase factory)
    {
        _factory = factory;
    }

    protected async Task<HttpResponseMessage> Get(string url, bool isAuth = true)
    {
        var client = await GetClient(isAuth);
        var response = await client.GetAsync(url);
        return response;
    }

    protected async Task<HttpResponseMessage> Post<T>(string url, T body, bool isAuth = true)
    {
        var client = await GetClient(isAuth);
        var response = await client.PostAsJsonAsync(url, body);
        return response;
    }

    protected async Task<HttpResponseMessage> Put<T>(string url, T body, bool isAuth = true)
    {
        var client = await GetClient(isAuth);
        var response = await client.PutAsJsonAsync(url, body);
        return response;
    }

    protected async Task<HttpResponseMessage> Delete(string url, bool isAuth = true)
    {
        var client = await GetClient(isAuth);
        var response = await client.DeleteAsync(url);
        return response;
    }

    protected async Task<T> DeserializeResponse<T>(HttpResponseMessage responseMessage)
    {
        var content = await responseMessage.Content.ReadFromJsonAsync<T>();

        Assert.NotNull(content);

        return content;
    }

    protected T GetService<T>() where T : notnull
    {
        var scope = _factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<T>();
        return service;
    }

    protected async Task<User> GetTestUser()
    {
        if (_testUser == null)
        {
            const string Password = "Test-Password";

            _testUser = new User {
                Username = "Test-User",
                Password = Password
            };

            var userRepository = GetService<IUserRepository>();
            var isExists = await userRepository.IsExists(_testUser.Username);
            if (!isExists)
            {
                var userService = GetService<IUserService>();
                _testUser = await userService.Create(_testUser);

                // Revert password from hash for authorization
                _testUser.Password = Password;
            }
        }

        return _testUser;
    }

    protected async Task<UserTokensDto> GetTestUserTokens()
    {
        var userService = GetService<IUserService>();
        var user = await GetTestUser();
        var tokens = await userService.Login(user);
        return tokens;
    }

    async Task<HttpClient> GetClient(bool isAuth)
    {
        var client = _factory.CreateClient();
        if (isAuth)
        {
            var tokens = await GetTestUserTokens();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokens.AccessToken}");
        }
        return client;
    }

    readonly protected WebApplicationFactoryBase _factory;

    User? _testUser;
}
