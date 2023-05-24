using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Users.Command.SetPassword;

public sealed class SetUserPasswordCommandHandler : ICommandHandler<SetUserPasswordCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetUserPasswordCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(SetUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId,cancellationToken);
        if (user is null)
        {
            return Result.Failure(DomainErrors.User.UserNotFoundById(request.UserId));
        }
        var result = user.SetPassword(request.Password);
        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }
        _userRepository.Update(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
};