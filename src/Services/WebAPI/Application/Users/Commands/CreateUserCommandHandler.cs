using Application.Abstractions.Messaging;
using Application.Users.Commands;
using Domain.Entities.UserAggregate;
using Domain.Repositories;
using Domain.Shared;
using Domain.ValueObjects;

namespace Application.Users.Commands;

public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand,Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var nameResult = Name.Create(request.Name);
        var phoneNumber = new PhoneNumber("+86", request.PhoneNumber);
        var isPhoneNumberUniqueAsync = await _userRepository.IsPhoneNumberUniqueAsync(phoneNumber,cancellationToken);
        if (!isPhoneNumberUniqueAsync)
        {
            return Result.Failure<Guid>(new Error("User.DuplicatePhone","Phone number already exist"));
        }
        var user = User.Create(nameResult.Value,request.PhoneNumber);
        _userRepository.Add(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return user.Id;

    }
}