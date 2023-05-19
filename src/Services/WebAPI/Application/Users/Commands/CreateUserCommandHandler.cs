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
        // Mock Data
        var phoneNumber = new PhoneNumber("+86", request.PhoneNumber);
        if (!await _userRepository.IsPhoneNumberUniqueAsync(phoneNumber,cancellationToken))
        {
            return Result.Failure<Guid>(new Error("User.DuplicatePhone","Phone number already exist"));
        }
        var user = User.Create(nameResult.Value, phoneNumber);
        
        _userRepository.Add(user);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return user.Id;

    }
}