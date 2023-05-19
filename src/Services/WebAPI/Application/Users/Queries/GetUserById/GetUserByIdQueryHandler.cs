using Application.Abstractions.Messaging;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Users.Queries.GetUserById;

internal sealed class GetUserByIdQueryHandler :IQueryHandler<GetUserByIdQuery,UserResponse>
{
    private readonly IUserRepository _repository;

    public GetUserByIdQueryHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async  Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<UserResponse>(new Error(
                "User.NotFound", 
                $"The User with {request.UserId} is not found"));
        }

        return new UserResponse(user.Id, user.Name.Value);
    }
}