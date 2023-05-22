using Application.Abstractions;
using Infrastructure.BackgroundJobs;
using Infrastructure.Services;
using Quartz;

namespace Infrastructure;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {

        services.AddScoped<IEmailService, EmailServices>();
        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessageJob));
            configure
                .AddJob<ProcessOutboxMessageJob>(jobKey)
                .AddTrigger(
                    trigger =>
                        trigger.ForJob(jobKey)
                            .WithSimpleSchedule(
                                schedule => schedule.WithIntervalInSeconds(10)
                                    .RepeatForever())
                    );
            
            configure.UseMicrosoftDependencyInjectionJobFactory();
        });
        services.AddQuartzHostedService();
        return services;
    }
}