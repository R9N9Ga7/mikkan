using Server.Exceptions;
using Server.Interfaces.Repositories;
using Server.Interfaces.Services;
using Server.Models.Entities;

namespace Server.Services;

public class UserService : IUserService
{
    public UserService(IUserRepository userRepository, IPasswordHasherService passwordHasherService)
    {
        _userRepository = userRepository;
        _passwordHasherService = passwordHasherService;
    }

    public async Task<User> Create(User user)
    {
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
}
