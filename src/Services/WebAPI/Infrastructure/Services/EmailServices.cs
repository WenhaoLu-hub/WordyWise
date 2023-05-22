using Application.Abstractions;
using Domain.Entities.UserAggregate;

namespace Infrastructure.Services;

public class EmailServices : IEmailService
{
    public Task SendUserRegisteredEmail(User user, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("SendUserRegisteredEmail trigger ");
        return Task.CompletedTask;
    }
}