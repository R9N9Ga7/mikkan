namespace Server;

#pragma warning disable CA1052 // Static holder types should be Static or NotInheritable
public class Program
#pragma warning restore CA1052 // Static holder types should be Static or NotInheritable
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/", () => Results.Json("Hello World!"));

        app.Run();
    }
}
