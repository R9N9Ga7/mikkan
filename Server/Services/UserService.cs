using Microsoft.Extensions.Options;
using Server.Exceptions;
using Server.Interfaces.Repositories;
using Server.Interfaces.Services;
using Server.Models.Dtos;
using Server.Models.Entities;
using Server.Settings;
using System.Security.Claims;

namespace Server.Services;

public class UserService : IUserService
{
    public UserService(
        IUserRepository userRepository,
        IPasswordHasherService passwordHasherService,
        IJwtKeyService jwtKeyService,
        IOptions<AccountSettings> options)
    {
        _userRepository = userRepository;
        _passwordHasherService = passwordHasherService;
        _jwtKeyService = jwtKeyService;

        _accountSettings = options.Value;
    }

    public async Task<User> Create(User user)
    {
        var userRegistrationsLimit = _accountSettings.UserRegistrationsLimit;
        if (userRegistrationsLimit > 0)
        {
            var count = await _userRepository.Count();
            if (count >= userRegistrationsLimit)
            {
                throw new UserRegistrationLimitException();
            }
        }

        var isUserAlreadyExists = await _userRepository.IsExists(user.Username);
        if (isUserAlreadyExists)
        {
            throw new UserAlreadyExistsException();
        }

        user.CreatedAt = DateTime.UtcNow;
        user.Password = _passwordHasherService.HashPassword(user.Password);

        var createdUser = await _userRepository.Create(user);
        return createdUser;
    }

    public async Task<UserLoginDto> Login(User user)
    {
        var findedUser = await _userRepository.GetByUsername(user.Username);

        if (findedUser == null)
        {
            throw new UserNotFoundException();
        }

        var isValidPassword = _passwordHasherService.VerifyPassword(findedUser.Password, user.Password);

        if (!isValidPassword)
        {
            throw new UserInvalidPasswordException();
        }

        var claims = new List<Claim> {
            new(ClaimTypes.Name, findedUser.Username),
        };

        var refreshToken = _jwtKeyService.GetRefreshToken(claims);
        var accessToken = _jwtKeyService.GetAccessToken(claims);

        var userLoginDto = new UserLoginDto {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        return userLoginDto;
    }

    readonly IUserRepository _userRepository;
    readonly IPasswordHasherService _passwordHasherService;
    readonly IJwtKeyService _jwtKeyService;
    readonly AccountSettings _accountSettings;
}
