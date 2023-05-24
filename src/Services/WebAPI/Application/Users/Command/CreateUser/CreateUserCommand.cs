using Application.Abstractions.Messaging;

namespace Application.Users.Command.CreateUser;

public sealed record CreateUserCommand(
    string Name, 
    string PhoneNumber,
    string Email) : ICommand<Guid>;