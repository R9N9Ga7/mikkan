﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Exceptions;
using Server.Extensions;
using Server.Interfaces.Services;
using Server.Models.Entities;
using Server.Models.Requests;
using Server.Models.Responses;
using System.Security.Claims;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VaultController : ControllerBase
{
    public VaultController(IItemService itemService, IMapper mapper)
    {
        _itemService = itemService;
        _mapper = mapper;
    }

    [HttpPost]
    [Authorize]
    public async Task<IResult> AddItem(AddItemRequest vaultAddItemRequest)
    {
        try
        {
            var item = _mapper.Map<Item>(vaultAddItemRequest);
            var userId = GetUserId();

            var createdItem = await _itemService.Create(item, userId);
            var itemResponse = _mapper.Map<ItemResponse>(createdItem);

            var url = Url.Action(
                nameof(AddItem),
                this.GetControllerName(),
                new { id = itemResponse.Id }
            );

            return Results.Created(url, itemResponse);
        } catch (BadRequestException)
        {
            return Results.BadRequest();
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IResult> GetItems()
    {
        try
        {
            var userId = GetUserId();
            var items = await _itemService.GetAllByUserId(userId);
            var itemsResponse = _mapper.Map<List<Item>>(items);
            return Results.Json(itemsResponse);
        } catch (BadRequestException)
        {
            return Results.BadRequest();
        }
    }

    Guid GetUserId()
    {
        var nameIdentifierClaim = User?.FindFirst(ClaimTypes.NameIdentifier);
        if (nameIdentifierClaim == null)
        {
            throw new BadRequestException();
        }

        var userId = new Guid(nameIdentifierClaim.Value);
        return userId;
    }

    readonly IItemService _itemService;
    readonly IMapper _mapper;
}
