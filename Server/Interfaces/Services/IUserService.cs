using Server.Models.Entities;

namespace Server.Interfaces.Services;

public interface IUserService
{
    public Task<User> Create(User user);
}
