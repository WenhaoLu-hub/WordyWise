using Application.Abstractions.Messaging;
using Domain.Entities.UserAggregate;
using Domain.Errors;
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
        var phoneNumber =  PhoneNumber.Create(request.PhoneNumber);
        
        // if (!nameResult.IsSuccess)
        // {
        //     return Result.Failure<Guid>(nameResult.Error);
        // }
        
        // if (!phoneNumber.IsSuccess)
        // {
        //     return Result.Failure<Guid>(phoneNumber.Error);
        // }
        
        if (!await _userRepository.IsPhoneNumberUniqueAsync(phoneNumber.Value,cancellationToken))
        {
            return Result.Failure<Guid>(DomainErrors.User.DuplicatePhoneNumber);
        }
        var user = User.Create(nameResult.Value, phoneNumber.Value);
        
        _userRepository.Add(user);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return user.Id;

    }
}