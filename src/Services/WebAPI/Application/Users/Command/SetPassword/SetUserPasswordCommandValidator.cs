using FluentValidation;

namespace Application.Users.Command.SetPassword;

public class SetUserPasswordCommandValidator : AbstractValidator<SetUserPasswordCommand>
{
    
    public SetUserPasswordCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}