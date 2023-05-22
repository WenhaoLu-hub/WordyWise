using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Persistence;
using Persistence.Outbox;
using Quartz;

namespace Infrastructure.BackgroundJobs;

public class ProcessOutboxMessageJob : IJob
{
    private readonly MyContext _myContext;
    private readonly IPublisher _publisher;
    private readonly ILogger<ProcessOutboxMessageJob> _logger;

    public ProcessOutboxMessageJob(MyContext myContext, IPublisher publisher, ILogger<ProcessOutboxMessageJob> logger)
    {
        _myContext = myContext;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var outboxMessages = await _myContext
            .Set<OutboxMessage>()
            .Where(message => message.ProcessedOnUtc == null)
            .Take(20)
            .ToListAsync();

        foreach (var outboxMessage in outboxMessages)
        {
            var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                outboxMessage.Content,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                }
            );
                

            if (domainEvent is null)
            {
                continue;
            }

            try
            {
                await _publisher.Publish(domainEvent, context.CancellationToken);
                _logger.LogInformation($"published the event: {domainEvent.GetType().Name}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Domain event publish error: {e.Message}");
                continue;
            }
            outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
        }
        
        await _myContext.SaveChangesAsync();

    }
}