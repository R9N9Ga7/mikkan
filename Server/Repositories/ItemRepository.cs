using Microsoft.EntityFrameworkCore;
using Server.Contexts;
using Server.Interfaces.Repositories;
using Server.Models.Entities;

namespace Server.Repositories;

public class ItemRepository : IItemRepository
{
    public ItemRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Item> Create(Item item)
    {
        await _databaseContext.Items.AddAsync(item);
        await _databaseContext.SaveChangesAsync();
        return item;
    }

    public async Task<IEnumerable<Item>> GetAllByUserId(Guid userId)
    {
        var items = await _databaseContext.Items
            .Where(i => i.UserId == userId)
            .ToListAsync();

        return items;
    }

    readonly DatabaseContext _databaseContext;
}
