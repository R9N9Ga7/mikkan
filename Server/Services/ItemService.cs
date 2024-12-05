using Server.Exceptions;
using Server.Interfaces.Repositories;
using Server.Interfaces.Services;
using Server.Models.Entities;

namespace Server.Services;

public class ItemService(IItemRepository itemRepository) : IItemService
{
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

    public async Task<Item> GetById(Guid userId, Guid itemId)
    {
        var item = await _itemRepository.GetById(itemId);
        if (item == null)
        {
            throw new ItemNotFoundException();
        }
        if (item.UserId != userId)
        {
            throw new UserUnauthorizedException();
        }
        return item;
    }

    public async Task RemoveById(Guid userId, Guid itemId)
    {
        var item = await GetById(userId, itemId);
        await _itemRepository.Remove(item);
    }

    readonly IItemRepository _itemRepository = itemRepository;
}
