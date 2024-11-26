﻿using Server.Models.Entities;

namespace Server.Interfaces.Repositories;

public interface IItemRepository
{
    public Task<Item> Create(Item item);
    public Task<IEnumerable<Item>> GetAllByUserId(Guid userId);
}