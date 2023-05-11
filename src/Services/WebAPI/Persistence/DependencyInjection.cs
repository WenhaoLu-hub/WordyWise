using Microsoft.EntityFrameworkCore;

namespace Persistence;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        var dbConnection = Environment.GetEnvironmentVariable("DBConnectionStringDev");
        services.AddDbContext<MyContext>(
            (provider, builder) =>
            {
                builder.UseSqlServer(dbConnection);
            });
        return services;
    }
}