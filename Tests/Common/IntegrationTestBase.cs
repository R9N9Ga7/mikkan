using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using Tests.Factories;

namespace Tests.Common;

public class IntegrationTestBase : IClassFixture<WebApplicationFactoryBase>
{
    public IntegrationTestBase(WebApplicationFactoryBase factory)
    {
        _factory = factory;
    }

    protected async Task<HttpResponseMessage> Get(string url)
    {
        var response = await _factory.CreateClient().GetAsync(url);
        return response;
    }

    protected async Task<HttpResponseMessage> Post<T>(string url, T body)
    {
        var response = await _factory.CreateClient().PostAsJsonAsync(url, body);
        return response;
    }

    protected async Task<HttpResponseMessage> Put<T>(string url, T body)
    {
        var response = await _factory.CreateClient().PutAsJsonAsync(url, body);
        return response;
    }

    protected async Task<HttpResponseMessage> Delete(string url)
    {
        var response = await _factory.CreateClient().DeleteAsync(url);
        return response;
    }

    protected async Task<T> DeserializeResponse<T>(HttpResponseMessage responseMessage)
    {
        var content = await responseMessage.Content.ReadFromJsonAsync<T>();

        Assert.NotNull(content);

        return content;
    }

    protected T GetService<T>() where T : notnull
    {
        var scope = _factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<T>();
        return service;
    }

    readonly WebApplicationFactoryBase _factory;
}
