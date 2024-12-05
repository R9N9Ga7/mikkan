using Server.Models.Entities;

namespace Server.Interfaces.Repositories;

public interface IItemRepository
{
    public Task<Item> Create(Item item);
    public Task<IEnumerable<Item>> GetAllByUserId(Guid userId);
    public Task<bool> IsExists(Guid id);
    public Task<Item?> GetById(Guid id);
    public Task Remove(Item item);
}
