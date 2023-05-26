using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Users.Command.AssignRole;

public class AssignRoleCommandHandler : ICommandHandler<AssignRoleCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignRoleCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }


    public async Task<Result> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await  _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure(DomainErrors.User.UserNotFoundById(request.UserId));
        }

        var role = await _userRepository.FindRoleById(request.RoleId, cancellationToken);
        if (role is null)
        {
            return Result.Failure(DomainErrors.User.RoleNotFoundById(request.RoleId));
        }

        Result result = user.AssignRole(role);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}