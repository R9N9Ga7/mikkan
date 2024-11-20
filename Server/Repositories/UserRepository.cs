using Microsoft.EntityFrameworkCore;
using Server.Contexts;
using Server.Exceptions;
using Server.Interfaces.Repositories;
using Server.Models.Entities;

namespace Server.Repositories;

public class UserRepository : IUserRepository
{
    public UserRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<User> Create(User user)
    {
        await _databaseContext.Users.AddAsync(user);
        await _databaseContext.SaveChangesAsync();

        return user;
    }

    public async Task<User?> GetByUsername(string username)
    {
        var user = await _databaseContext.Users
            .FirstOrDefaultAsync(u => u.Username == username);

        return user;
    }

    public async Task<bool> IsExists(string username)
    {
        var isExists = await _databaseContext.Users
            .AnyAsync(u => u.Username == username);
        return isExists;
    }

    public async Task<int> Count()
    {
        var count = await _databaseContext.Users.CountAsync();
        return count;
    }

    readonly DatabaseContext _databaseContext;
}
