using Microsoft.EntityFrameworkCore;
using Server.Services;

namespace Server;

#pragma warning disable CA1052 // Static holder types should be Static or NotInheritable
public class Program
#pragma warning restore CA1052 // Static holder types should be Static or NotInheritable
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigServices(builder);

        var app = builder.Build();
        ConfigApp(app);

        app.Run();
    }

    static void ConfigServices(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddCors(options =>
        {
            options.AddPolicy(DevCorsPolicyName, builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });
        });

        services.AddDbContext<DatabaseContext>(options =>
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            options.UseSqlite(connectionString);
        });
    }

    static void ConfigApp(WebApplication app)
    {
        app.MapControllers();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseDeveloperExceptionPage();
            app.UseCors(DevCorsPolicyName);
        }
    }

    const string DevCorsPolicyName = "AllowAll";
}
