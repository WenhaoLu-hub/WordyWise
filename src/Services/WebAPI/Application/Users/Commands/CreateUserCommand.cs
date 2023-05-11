using Application.Abstractions.Messaging;
using Domain.Shared;
using Domain.ValueObjects;
using MediatR;

namespace Application.Users.Commands;

public sealed record CreateUserCommand(string Name, string PhoneNumber) : ICommand<Guid>;