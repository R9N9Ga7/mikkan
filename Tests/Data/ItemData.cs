using Server.Models.Requests;
using Server.Models.Entities;
using System.Collections;

namespace Tests.Data;

public class ItemData
{
    public static AddItemRequest CreateAddItemRequest()
    {
        return new AddItemRequest
        {
            Login = $"Login-{Guid.NewGuid()}",
            Password = $"Password-{Guid.NewGuid()}",
            Name = $"Name-{Guid.NewGuid()}",
        };
    }

    public static Item CreateItem()
    {
        return new Item
        {
            Login = $"Login-{Guid.NewGuid()}",
            Password = $"Password-{Guid.NewGuid()}",
            Name = $"Name-{Guid.NewGuid()}",
        };
    }
}

public class ItemDataInvalid : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] {
            new AddItemRequest { Login = "Login", Password = "Password", Name = "" }
        };

        yield return new object[] {
            new AddItemRequest { Login = new string('l', 512), Password = "Password", Name = "Name" }
        };

        yield return new object[] {
            new AddItemRequest { Login = "Login", Password = new string('l', 512), Name = "Name" }
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
