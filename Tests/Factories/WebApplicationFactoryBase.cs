using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Server;
using Server.Contexts;
using Server.Settings;

namespace Tests.Factories;

public class WebApplicationFactoryBase : WebApplicationFactory<Program>
{
    public WebApplicationFactoryBase()
    {
        _sqliteConnection = new SqliteConnection("DataSource=:memory:");
        _sqliteConnection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services => {
            services.RemoveAll<DbContextOptions<DatabaseContext>>();
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlite(_sqliteConnection);
            });

            services.Configure<AccountSettings>(options =>
            {
                options.SaltSize = 16;
                options.KeySize = 32;
                options.Iterations = 10000;
                options.UserRegistrationsLimit = int.MaxValue;
            });

            var context = CreateDatabaseContext(services);
            context.Database.EnsureDeleted();
        });
    }

    DatabaseContext CreateDatabaseContext(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        return context;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _sqliteConnection.Dispose();
    }

    readonly SqliteConnection _sqliteConnection;
}
