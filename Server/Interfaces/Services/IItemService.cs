﻿using Server.Models.Entities;

namespace Server.Interfaces.Services;

public interface IItemService
{
    public Task<Item> Create(Item item, Guid userId);
    public Task<IEnumerable<Item>> GetAllByUserId(Guid userId);
    public Task<Item> GetById(Guid userId, Guid itemId);
    public Task RemoveById(Guid userId, Guid itemId);
    public Task<Item> Edit(Item item, Guid userId);
}
