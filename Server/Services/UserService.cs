using Microsoft.Extensions.Options;
using Server.Exceptions;
using Server.Interfaces.Repositories;
using Server.Interfaces.Services;
using Server.Models.Dtos;
using Server.Models.Entities;
using Server.Settings;
using System.Security.Claims;

namespace Server.Services;

public class UserService(
        IUserRepository userRepository,
        IPasswordHasherService passwordHasherService,
        ITokenService tokenService,
        IOptions<AccountSettings> options
) : IUserService
{
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

    public async Task<UserTokensDto> Login(User user)
    {
        var findedUser = await _userRepository.GetByUsername(user.Username)
            ?? throw new UserNotFoundException();

        var isValidPassword = _passwordHasherService.VerifyPassword(findedUser.Password, user.Password);
        if (!isValidPassword)
        {
            throw new UserInvalidPasswordException();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, findedUser.Username),
            new(ClaimTypes.NameIdentifier, findedUser.Id.ToString()),
        };

        var refreshToken = _tokenService.GetRefreshToken(claims);
        var accessToken = _tokenService.GetAccessToken(claims);

        var userTokensDto = new UserTokensDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        return userTokensDto;
    }

    public async Task<UserTokensDto> RefreshTokens(UserTokensDto userTokensDto)
    {
        var tokenValidationResult = await _tokenService.ValidateToken(userTokensDto.RefreshToken);

        if (!tokenValidationResult.IsValid)
        {
            throw new UserUnauthorizedException();
        }

        var claims = tokenValidationResult.ClaimsIdentity.Claims;

        var refreshToken = _tokenService.GetRefreshToken(claims);
        var accessToken = _tokenService.GetAccessToken(claims);

        var refreshedUserTokensDto = new UserTokensDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        return refreshedUserTokensDto;
    }

    readonly IUserRepository _userRepository = userRepository;
    readonly IPasswordHasherService _passwordHasherService = passwordHasherService;
    readonly ITokenService _tokenService = tokenService;
    readonly AccountSettings _accountSettings = options.Value;
}
