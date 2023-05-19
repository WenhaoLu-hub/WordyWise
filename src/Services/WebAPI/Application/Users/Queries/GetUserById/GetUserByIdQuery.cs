using Application.Abstractions.Messaging;

namespace Application.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;

public sealed record UserResponse(Guid Id, string Email);