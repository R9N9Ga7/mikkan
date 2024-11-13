using Server.Models.Entities;

namespace Server.Interfaces.Repositories;

public interface IUserRepository
{
    public Task<User> Create(User user);
}
