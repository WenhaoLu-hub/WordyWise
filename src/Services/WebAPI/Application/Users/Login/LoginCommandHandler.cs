using System.ComponentModel.DataAnnotations;
using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Domain.ValueObjects;

namespace Application.Users.Login;

public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        //Get User
        var email = Email.Create(request.Email);
        if (email.IsFailure)
        {
            return Result.Failure<string>(email.Error);
        }
        var user = await _userRepository.GetByEmailAsync(email.Value, cancellationToken);
        if (user is null)
        {
            return Result.Failure<string>(DomainErrors.User.UserNotFoundByEmail(email.Value));
        }
        var checkPassword = user.CheckPassword(request.Password);
        if (checkPassword.IsFailure)
        {
            return Result.Failure<string>(checkPassword.Error);
        }
        
        //Generate Token
        var token = _jwtProvider.Generate(user);
        
        //Return Token
        return token;
    }
}