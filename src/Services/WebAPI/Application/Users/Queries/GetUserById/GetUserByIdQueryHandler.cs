using Application.Abstractions.Messaging;
using Domain.Errors;
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
            return Result.Failure<UserResponse>(DomainErrors.User.UserNotFoundById(request.UserId));
        }

        return new UserResponse(user.Id, user.Email.Value);
    }
}