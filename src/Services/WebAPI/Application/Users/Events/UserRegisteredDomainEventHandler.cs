using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.DomainEvents;
using Domain.Primitives;
using Domain.Repositories;

namespace Application.Users.Events;

internal sealed class UserRegisteredDomainEventHandler : IDomainEventHandler<UserRegisteredDomainEvent>
{
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepository;

    public UserRegisteredDomainEventHandler(IEmailService emailService, IUserRepository userRepository)
    {
        _emailService = emailService;
        _userRepository = userRepository;
    }

    public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(notification.UserId, cancellationToken);
        if (user is null)
        {
            return;
        }
        
        await _emailService.SendUserRegisteredEmail(user, cancellationToken);
    }
}