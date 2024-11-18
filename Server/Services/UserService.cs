using Microsoft.Extensions.Options;
using Server.Exceptions;
using Server.Interfaces.Repositories;
using Server.Interfaces.Services;
using Server.Models.Entities;
using Server.Settings;

namespace Server.Services;

public class UserService : IUserService
{
    public UserService(
        IUserRepository userRepository,
        IPasswordHasherService passwordHasherService,
        IOptions<AccountSettings> options)
    {
        _userRepository = userRepository;
        _passwordHasherService = passwordHasherService;

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

    readonly IUserRepository _userRepository;
    readonly IPasswordHasherService _passwordHasherService;
    readonly AccountSettings _accountSettings;
}
