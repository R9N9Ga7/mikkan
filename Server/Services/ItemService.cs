using Server.Interfaces.Repositories;
using Server.Interfaces.Services;
using Server.Models.Entities;

namespace Server.Services;

public class ItemService : IItemService
{
    public ItemService(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task<Item> Create(Item item, Guid userId)
    {
        item.UserId = userId;
        item.CreatedAt = DateTime.UtcNow;

        await _itemRepository.Create(item);

        return item;
    }

    public async Task<IEnumerable<Item>> GetAllByUserId(Guid userId)
    {
        var items = await _itemRepository.GetAllByUserId(userId);
        return items;
    }

    readonly IItemRepository _itemRepository;
}
