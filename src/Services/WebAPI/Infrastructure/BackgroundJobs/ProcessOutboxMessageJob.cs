using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Persistence;
using Persistence.Outbox;
using Polly;
using Quartz;

namespace Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessageJob : IJob
{
    private readonly MyContext _myContext;
    private readonly IPublisher _publisher;

    public ProcessOutboxMessageJob(MyContext myContext, IPublisher publisher)
    {
        _myContext = myContext;
        _publisher = publisher;
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

            // var policyResult = await Policy
            //     .Handle<Exception>()
            //     .WaitAndRetryAsync(3,
            //         attempt => TimeSpan.FromMicroseconds(50 * attempt))
            //     .ExecuteAndCaptureAsync(() => _publisher.Publish(domainEvent, context.CancellationToken));
            await _publisher.Publish(domainEvent, context.CancellationToken);
            // outboxMessage.Error = policyResult.FinalException.ToString();
            // outboxMessage.Error = policyResult.FinalException.ToString();

            outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
        }
        
        await _myContext.SaveChangesAsync();

    }
}