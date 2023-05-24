using Domain.ValueObjects;
using FluentValidation;

namespace Application.Users.Command.CreateUser;

internal class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();

        RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(PhoneNumber.MaxLength);

        RuleFor(x => x.Email).EmailAddress().NotEmpty().MaximumLength(Email.MaxLength);
    }
}