using Domain.Entities.UserAggregate;

namespace Application.Abstractions;

public interface IEmailService
{
    Task SendUserRegisteredEmail(User user, CancellationToken cancellationToken = default);
}