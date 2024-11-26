using Microsoft.AspNetCore.Authentication.JwtBearer;
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

        services.AddAuthorization();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var authSettings = builder.Configuration.GetSection("AuthSettings").Get<AuthSettings>();
                if (authSettings == null)
                {
                    return;
                }

                var tokenService = new TokenService(authSettings);
                options.TokenValidationParameters = tokenService.GetTokenValidationParameters();
            });

        services.AddDbContext<DatabaseContext>(options =>
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            options.UseSqlite(connectionString);
        });

        services.AddAutoMapper(typeof(MapperProfile));

        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IItemRepository, ItemRepository>();

        services.AddTransient<IPasswordHasherService, PasswordHasherService>();
        services.AddTransient<ITokenService, TokenService>();

        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IItemService, ItemService>();

        services.Configure<AccountSettings>(builder.Configuration.GetSection("AccountSettings"));
        services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));
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
