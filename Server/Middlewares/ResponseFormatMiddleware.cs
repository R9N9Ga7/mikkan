using Server.Models;
using System.Text.Json;

namespace Server.Middlewares;

public class ResponseFormatMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;
        using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        await _next(context);

        memoryStream.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();
        memoryStream.Seek(0, SeekOrigin.Begin);

        var response = new Response<object>();

        if (IsJson(responseBody))
        {
            response.Content = string.IsNullOrWhiteSpace(responseBody)
                ? null : JsonSerializer.Deserialize<object>(responseBody);
        }
        else
        {
            response.Message = responseBody;
        }

        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
        {
            response.Message = "Unauthorized";
        }

        context.Response.Body = originalBodyStream;
        await context.Response.WriteAsJsonAsync(response);
    }

    private static bool IsJson(string responseBody)
    {
        if (string.IsNullOrWhiteSpace(responseBody))
        {
            return false;
        }

        responseBody = responseBody.Trim();

        if ((responseBody.StartsWith('{') && responseBody.EndsWith('}')) ||
            (responseBody.StartsWith('[') && responseBody.EndsWith(']')))
        {
            try
            {
                using (JsonDocument.Parse(responseBody))
                {
                    return true;
                };
            }
            catch
            {
                return false;
            }
        }

        return false;
    }

    readonly RequestDelegate _next = next;
}
