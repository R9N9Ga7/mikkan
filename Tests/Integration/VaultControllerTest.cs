using FluentAssertions;
using Server.Interfaces.Repositories;
using Server.Models.Entities;
using Server.Models.Requests;
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

    const string Url = "api/vault";
    const string UrlAddItem = Url;

    readonly IItemRepository _itemRepository;
}
