using Microsoft.EntityFrameworkCore;
using Server.Contexts;
using Server.Interfaces.Repositories;
using Server.Models.Entities;

namespace Server.Repositories;

public class UserRepository(DatabaseContext databaseContext) : IUserRepository
{
    public async Task<User> Create(User user)
    {
        await _databaseContext.Users.AddAsync(user);
        await _databaseContext.SaveChangesAsync();

        return user;
    }

    public async Task<User?> GetByUsername(string username)
    {
        return await _databaseContext.Users
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<bool> IsExists(string username)
    {
        return await _databaseContext.Users
            .AnyAsync(u => u.Username == username);
    }

    public async Task<int> Count()
    {
        return await _databaseContext.Users.CountAsync();
    }

    readonly DatabaseContext _databaseContext = databaseContext;
}
