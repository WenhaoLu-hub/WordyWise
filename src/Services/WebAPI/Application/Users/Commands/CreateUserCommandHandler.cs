using Application.Abstractions.Messaging;
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
        
        if (!nameResult.IsSuccess)
        {
            return Result.Failure<Guid>(nameResult.Error);
        }
        
        // Mock Data
        var phoneNumber =  PhoneNumber.Create(request.PhoneNumber);
        
        if (!phoneNumber.IsSuccess)
        {
            return Result.Failure<Guid>(phoneNumber.Error);
        }
        
        if (await _userRepository.IsPhoneNumberUniqueAsync(phoneNumber.Value,cancellationToken))
        {
            return Result.Failure<Guid>(new Error("User.DuplicatePhone","Phone number already exist"));
        }
        var user = User.Create(nameResult.Value, phoneNumber.Value);
        
        _userRepository.Add(user);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return user.Id;

    }
}