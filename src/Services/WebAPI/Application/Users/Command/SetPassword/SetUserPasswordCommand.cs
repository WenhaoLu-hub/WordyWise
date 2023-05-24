using Application.Abstractions.Messaging;

namespace Application.Users.Command.SetPassword;

public sealed record SetUserPasswordCommand(Guid UserId, string Password) : ICommand;