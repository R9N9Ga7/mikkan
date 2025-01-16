using FluentAssertions;
using Server.Interfaces.Repositories;
using Server.Interfaces.Services;
using Server.Models.Entities;
using Server.Models.Requests;
using Server.Models.Responses;
using System.Net;
using Tests.Common;
using Tests.Data;
using Tests.Factories;

namespace Tests.Integration;

public class VaultControllerTest : IntegrationTestBase
{
    public VaultControllerTest(WebApplicationFactoryBase factory)
        : base(factory)
    {
        _itemRepository = GetService<IItemRepository>();
        _itemService = GetService<IItemService>();
    }

    [Fact]
    public async Task AddItemWithValidData()
    {
        var itemRequest = ItemData.CreateAddItemRequest();

        var response = await Post(UrlAddItem, itemRequest);
        response.EnsureSuccessStatusCode();

        var content = await DeserializeResponse<Item>(response);
        content.Should().NotBeNull();

        var isExists = await _itemRepository.IsExists(content.Id);
        isExists.Should().BeTrue();

        content.Should().BeEquivalentTo(itemRequest);

        var findedItem = await _itemRepository.GetById(content.Id);
        findedItem.Should().NotBeNull();

        var user = await GetTestUser();
        findedItem?.UserId.Should().Be(user.Id);
    }

    [Theory]
    [ClassData(typeof(ItemDataInvalid))]
    public async Task AddItemWithInvalidData(AddItemRequest itemRequest)
    {
        var response = await Post(UrlAddItem, itemRequest);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetItemByIdValidData()
    {
        var item = new Item {
            Name = "Test-Name",
            Login = "Test-Login",
            Password = "Test-Password",
        };

        var user = await GetTestUser();
        var createdItem = await _itemService.Create(item, user.Id);

        var response = await Get($"{UrlAddItem}/{createdItem.Id}");
        response.EnsureSuccessStatusCode();

        var content = await DeserializeResponse<ItemResponse>(response);
        content.Should().NotBeNull();

        content.Name.Should().BeEquivalentTo(item.Name);
        content.Login.Should().BeEquivalentTo(item.Login);
        content.Password.Should().BeEquivalentTo(item.Password);
    }

    [Fact]
    public async Task GetItemByIdWithInvalidUserId()
    {
        var item = new Item
        {
            Name = "Test-Name",
            Login = "Test-Login",
            Password = "Test-Password",
        };

        var user = await CreateRandomTestUser();
        var createdItem = await _itemService.Create(item, user.Id);

        var response = await Get($"{UrlAddItem}/{createdItem.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetItemByIdWidthInvalidItemId()
    {
        var response = await Get($"{UrlAddItem}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetItemByIdWithoutAuthorization()
    {
        var response = await Get($"{UrlAddItem}/{Guid.NewGuid()}", false);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task RemoveItemByIdValidItemId()
    {
        var item = ItemData.CreateItem();
        var user = await GetTestUser();

        await _itemService.Create(item, user.Id);

        var response = await Delete($"{UrlRemoveItem}/{item.Id}");
        response.EnsureSuccessStatusCode();

        var findedItem = await _itemRepository.GetById(item.Id);
        findedItem.Should().BeNull();
    }

    [Fact]
    public async Task RemoveItemByIdInvalidId()
    {
        var response = await Delete($"{UrlRemoveItem}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RemoveItemByIdInvalidUserId()
    {
        var item = ItemData.CreateItem();
        var user = await CreateRandomTestUser();

        await _itemService.Create(item, user.Id);

        var response = await Delete($"{UrlRemoveItem}/{item.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task RemoveItemByIdWithoutAuthorization()
    {
        var response = await Get($"{UrlRemoveItem}/{Guid.NewGuid()}", false);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task EditItemWithValidData()
    {
        var itemRequest = ItemData.CreateAddItemRequest();

        var response = await Post(UrlAddItem, itemRequest);
        response.EnsureSuccessStatusCode();

        var content = await DeserializeResponse<Item>(response);
        content.Should().NotBeNull();

        content.Name = "Edited name";
        content.Password = "Edited password";
        content.Login = "Edited login";

        var editResponse = await Put(UrlEditItem, content);
        editResponse.EnsureSuccessStatusCode();

        var findedItem = await _itemRepository.GetById(content.Id);
        findedItem.Should().NotBeNull();

        findedItem?.Name.Should().Be(content.Name);
        findedItem?.Password.Should().Be(content.Password);
        findedItem?.Login.Should().Be(content.Login);
    }

    const string Url = "api/vault";
    const string UrlAddItem = Url;
    const string UrlRemoveItem = Url;
    const string UrlEditItem = Url;

    readonly IItemRepository _itemRepository;
    readonly IItemService _itemService;
}
