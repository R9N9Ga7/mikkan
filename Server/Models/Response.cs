namespace Server.Models;

public class Response<T>
{
    public string Message { get; set; } = string.Empty;
    public T? Content { get; set; }
}
