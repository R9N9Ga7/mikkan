using AutoMapper;
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
public class VaultController(IItemService itemService, IMapper mapper) : ControllerBase
{
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
                nameof(GetItemById),
                this.GetControllerName(),
                new { id = itemResponse.Id }
            );

            return Results.Created(url, itemResponse);
        }
        catch (BadRequestException)
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
        }
        catch (BadRequestException)
        {
            return Results.BadRequest();
        }
    }

    [Authorize]
    [HttpGet("{itemId}")]
    public async Task<IResult> GetItemById(Guid itemId)
    {
        try
        {
            var userId = GetUserId();
            var item = await _itemService.GetById(userId, itemId);
            var itemResponse = _mapper.Map<ItemResponse>(item);
            return Results.Json(itemResponse);
        }
        catch (ItemNotFoundException)
        {
            return Results.NotFound();
        }
        catch (UserUnauthorizedException)
        {
            return Results.Unauthorized();
        }
    }

    [Authorize]
    [HttpDelete("{itemId}")]
    public async Task<IResult> RemoveItemById(Guid itemId)
    {
        try
        {
            var userId = GetUserId();
            await _itemService.RemoveById(userId, itemId);
            return Results.Ok();
        }
        catch (ItemNotFoundException)
        {
            return Results.NotFound();
        }
        catch (UserUnauthorizedException)
        {
            return Results.Unauthorized();
        }
    }

    [HttpPut]
    [Authorize]
    public async Task<IResult> EditItem(EditItemRequest itemRequest)
    {
        try
        {
            var userId = GetUserId();
            var item = _mapper.Map<Item>(itemRequest);
            await _itemService.Edit(item, userId);
            return Results.Ok();
        }
        catch (ItemNotFoundException)
        {
            return Results.NotFound();
        }
        catch (UserUnauthorizedException)
        {
            return Results.Unauthorized();
        }
    }

    Guid GetUserId()
    {
        var nameIdentifierClaim = User?.FindFirst(ClaimTypes.NameIdentifier)
            ?? throw new BadRequestException();

        return new Guid(nameIdentifierClaim.Value); ;
    }

    readonly IItemService _itemService = itemService;
    readonly IMapper _mapper = mapper;
}
