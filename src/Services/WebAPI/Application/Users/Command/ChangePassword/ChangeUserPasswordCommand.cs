using Application.Abstractions.Messaging;

namespace Application.Users.Command.ChangePassword;

public sealed record ChangeUserPasswordCommand(
    Guid UserId,
    string OldPassword,
    string NewPassword) : ICommand;