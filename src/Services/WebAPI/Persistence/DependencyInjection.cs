using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Interceptors;
using Persistence.Repositories;

namespace Persistence;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        var dbConnection = Environment.GetEnvironmentVariable("DBConnectionStringDev");
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        services.AddDbContext<MyContext>(
            (provider, builder) =>
            {
                var interceptor = provider.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>();
                builder.UseSqlServer(dbConnection)
                    .AddInterceptors(interceptor);
            });
        
        services.AddScoped<IUserRepository,UserRepository>();
        services.Decorate<IUserRepository,CachedUserRepository>();
        

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}