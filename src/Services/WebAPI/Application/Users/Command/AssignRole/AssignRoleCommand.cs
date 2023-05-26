using Application.Abstractions.Messaging;

namespace Application.Users.Command.AssignRole;

public sealed record AssignRoleCommand(Guid UserId, int RoleId) : ICommand;