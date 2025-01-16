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
        var item = await _itemRepository.GetById(itemId) ?? throw new ItemNotFoundException();
        return item.UserId == userId
            ? item : throw new UserUnauthorizedException();
    }

    public async Task RemoveById(Guid userId, Guid itemId)
    {
        var item = await GetById(userId, itemId);
        await _itemRepository.Remove(item);
    }

    public async Task<Item> Edit(Item item, Guid userId)
    {
        var findedItem = await GetById(userId, item.Id);

        findedItem.Name = item.Name;
        findedItem.Login = item.Login;
        findedItem.Password = item.Password;

        var editedItem = await _itemRepository.Edit(findedItem);
        return editedItem;
    }

    readonly IItemRepository _itemRepository = itemRepository;
}
