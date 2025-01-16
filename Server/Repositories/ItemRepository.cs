using Microsoft.EntityFrameworkCore;
using Server.Contexts;
using Server.Interfaces.Repositories;
using Server.Models.Entities;

namespace Server.Repositories;

public class ItemRepository(DatabaseContext databaseContext) : IItemRepository
{
    public async Task<Item> Create(Item item)
    {
        await _databaseContext.Items.AddAsync(item);
        await _databaseContext.SaveChangesAsync();
        return item;
    }

    public async Task<IEnumerable<Item>> GetAllByUserId(Guid userId)
    {
        return await _databaseContext.Items
            .Where(i => i.UserId == userId)
            .ToListAsync();
    }

    public async Task<bool> IsExists(Guid id)
    {
        return await _databaseContext.Items
            .AnyAsync(i => i.Id == id);
    }

    public async Task<Item?> GetById(Guid id)
    {
        return await _databaseContext.Items
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task Remove(Item item)
    {
        _databaseContext.Items.Remove(item);
        await _databaseContext.SaveChangesAsync();
    }

    public async Task<Item> Edit(Item item)
    {
        _databaseContext.Items.Update(item);
        await _databaseContext.SaveChangesAsync();

        return item;
    }

    readonly DatabaseContext _databaseContext = databaseContext;
}
