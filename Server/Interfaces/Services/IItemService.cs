using Server.Models.Entities;

namespace Server.Interfaces.Services;

public interface IItemService
{
    public Task<Item> Create(Item item, Guid userId);
    public Task<IEnumerable<Item>> GetAllByUserId(Guid userId);
}
