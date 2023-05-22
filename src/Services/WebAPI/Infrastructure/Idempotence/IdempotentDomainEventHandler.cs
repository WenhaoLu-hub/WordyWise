using Application.Abstractions.Messaging;
using Domain.Primitives;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Outbox;

namespace Infrastructure.Idempotence;

public class IdempotentDomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    private readonly INotificationHandler<TDomainEvent> _decorated;
    private readonly MyContext _myContext;

    public IdempotentDomainEventHandler(INotificationHandler<TDomainEvent> notificationHandler, MyContext myContext)
    {
        _decorated = notificationHandler;
        _myContext = myContext;
    }

    public async Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
    {
        var consumer = _decorated.GetType().Name;
        if (await _myContext
                .Set<OutBoxMessageConsumer>()
                .AnyAsync(
                    outboxMessageConsumer => outboxMessageConsumer.Id ==notification.Id &&
                                             outboxMessageConsumer.Name == consumer,
                    cancellationToken))
        {
            return;
        }

        await _decorated.Handle(notification, cancellationToken);
        _myContext
            .Set<OutBoxMessageConsumer>()
            .Add(new OutBoxMessageConsumer
                {
                    Id = notification.Id,
                    Name = consumer
                }
            );
        await _myContext.SaveChangesAsync(cancellationToken);
    }
}