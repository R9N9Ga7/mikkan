using FluentAssertions;
using Server.Interfaces.Repositories;
using Server.Models.Requests;
using System.Net;
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

        var isUserExists = await _userRepository.Contains(userCreateRequest.Username);
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

    const string Url = "/api/account";

    readonly IUserRepository _userRepository;
}
