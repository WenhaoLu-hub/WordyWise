using Application.Abstractions.Messaging;
using Domain.Entities.UserAggregate;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Domain.ValueObjects;

namespace Application.Users.Command.CreateUser;

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
        var phoneNumber =  PhoneNumber.Create(request.PhoneNumber);
        if (!phoneNumber.IsSuccess)
        {
            return Result.Failure<Guid>(phoneNumber.Error);
        }
        var email = Email.Create(request.Email);
        if (!email.IsSuccess)
        {
            return Result.Failure<Guid>(email.Error);
        }

        if (await _userRepository.IsPhoneNumberUniqueAsync(phoneNumber.Value,cancellationToken))
        {
            return Result.Failure<Guid>(DomainErrors.User.DuplicatePhoneNumber);
        }

        if (await _userRepository.IsEmailUniqueAsync(email.Value,cancellationToken))
        {
            return Result.Failure<Guid>(DomainErrors.User.DuplicateEmail);
        }
        
        var user = User.Create(nameResult.Value, phoneNumber.Value, email.Value);
        
        _userRepository.Add(user);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return user.Id;

    }
}