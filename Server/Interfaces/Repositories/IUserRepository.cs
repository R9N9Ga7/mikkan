using Server.Models.Entities;

namespace Server.Interfaces.Repositories;

public interface IUserRepository
{
    public Task<User> Create(User user);
    public Task<User> GetByUsername(string username);
    public Task<bool> IsExists(string username);
    public Task<int> Count();
}
