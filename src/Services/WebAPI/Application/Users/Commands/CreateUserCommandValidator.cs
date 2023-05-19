using Application.Abstractions.Messaging;
using Domain.Shared;
using Domain.ValueObjects;
using FluentValidation;
using MediatR;

namespace Application.Users.Commands;

internal class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();

        RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(PhoneNumber.MaxLength);
    }
}