
using Server.Models.Requests;
using System.Collections;

namespace Tests.Data;

public class UserData
{
    public static UserCreateRequest GetUserCreateRequest()
    {
        return new UserCreateRequest
        {
            Username = "username",
            Password = "password",
        };
    }
}

public class UserDataInvalid : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new UserCreateRequest
            {
                Username = "",
                Password = "password",
            }
        };

        yield return new object[]
        {
            new UserCreateRequest
            {
                Username = "username",
                Password = "",
            }
        };

        yield return new object[]
        {
            new UserCreateRequest
            {
                Username = new string('p', 128),
                Password = "password",
            }
        };

        yield return new object[]
        {
            new UserCreateRequest
            {
                Username = "username",
                Password = new string('p', 128),
            }
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
