using Microsoft.EntityFrameworkCore;
using Server.Contexts;
using Server.Exceptions;
using Server.Interfaces.Repositories;
using Server.Interfaces.Services;
using Server.Models.Entities;

namespace Server.Repositories;

public class UserRepository : IUserRepository
{
    public UserRepository(DatabaseContext databaseContext, IPasswordHasherService passwordHasherService)
    {
        _databaseContext = databaseContext;
        _passwordHasherService = passwordHasherService; 
    }

    public async Task<User> Create(User user)
    {
        var findedUser = await _databaseContext.Users
            .FirstOrDefaultAsync(u => u.Username == user.Username);

        if (findedUser != null)
        {
            throw new UserAlreadyExistsException();
        }

        user.CreatedAt = DateTime.UtcNow;
        user.Password = _passwordHasherService.HashPassword(user.Password);

        await _databaseContext.Users.AddAsync(user);
        await _databaseContext.SaveChangesAsync();

        return user;
    }

    public async Task<User> GetByUsername(string username)
    {
        var user = await _databaseContext.Users
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            throw new UserNotFoundException();
        }

        return user;
    }

    readonly DatabaseContext _databaseContext;
    readonly IPasswordHasherService _passwordHasherService;
}
