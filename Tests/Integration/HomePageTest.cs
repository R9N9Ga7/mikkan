using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common;
using Tests.Factories;

namespace Tests.Integration;

public class HomePageTest : IntegrationTestBase
{
    public HomePageTest(WebApplicationFactoryBase factory)
        : base(factory) { }

    [Fact]
    public async Task Get_Valid()
    {
        var response = await Get("/");
        response.EnsureSuccessStatusCode();

        var content = await DeserializeResponse<string>(response);
        content.Should().Be("Hello World!");
    }
}
