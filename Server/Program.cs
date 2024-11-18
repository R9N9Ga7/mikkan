using Microsoft.EntityFrameworkCore;
using Server.Contexts;
using Server.Interfaces.Repositories;
using Server.Interfaces.Services;
using Server.Mappers;
using Server.Repositories;
using Server.Services;
using Server.Settings;

namespace Server;

public class Program
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

        services.AddAutoMapper(typeof(MapperProfile));

        services.AddTransient<IUserRepository, UserRepository>();

        services.AddTransient<IPasswordHasherService, PasswordHasherService>();

        services.AddTransient<IUserService, UserService>();

        services.Configure<AccountSettings>(builder.Configuration.GetSection("AccountSettings"));
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
