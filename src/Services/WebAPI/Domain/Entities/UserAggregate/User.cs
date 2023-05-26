using Domain.DomainEvents;
using Domain.Errors;
using Domain.Primitives;
using Domain.Shared;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.UserAggregate;

public class User : AggregateRoot
{
    public Name Name { get; private set; }
    public Email? Email { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public string? PasswordHash { get; private set; } = string.Empty;
    public DateTime CreatedOnUtc { get; private init; }
    public DateTime? ModifiedOnUtc { get; private set; }
    public ICollection<Role> Roles { get; set; } = new List<Role>();

    private User() //ORM
    {
    }

    private User(Guid id, Name name, PhoneNumber phoneNumber, Email email) : base(id)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        Email = email;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public static User Create(
        Name name,
        PhoneNumber phoneNumber,
        Email email)
    {
        
        var user = new User(Guid.NewGuid(), name, phoneNumber, email);
        //raise domain event here if needed
        user.RaiseDomainEvent(new UserRegisteredDomainEvent(Guid.NewGuid(),  user.Id));
        return user;
    }

    public void ChangeName(Name name)
    {
        Name = name;
    }

    public Result SetPassword(string password)
    {
        //validation first
        if (!HasPassword())
        {
            return Result.Failure(DomainErrors.User.PasswordAlreadySet);
        }
        var passwordHasher = new PasswordHasher<User>();
        PasswordHash = passwordHasher.HashPassword(this,password);
        ModifiedOnUtc = DateTime.UtcNow;
        return Result.Success();
    }

    private bool HasPassword()
    {
        return PasswordHash is not null;
    }
    public Result<bool> CheckPassword(string password)
    {
        if (!HasPassword())
        {
            return Result.Failure<bool>(DomainErrors.User.PasswordNotSet);
        }
        var verificationResult = new PasswordHasher<User>()
            .VerifyHashedPassword(
                this,
                PasswordHash!,
                password);
        if (verificationResult != PasswordVerificationResult.Success)
        {
            return Result.Failure<bool>(DomainErrors.User.PasswordNotMatch);
        }
        return true;
    }

    public Result ChangePassword(string oldPassword, string newPassword)
    {
        if (!HasPassword())
        {
            return Result.Failure<bool>(DomainErrors.User.PasswordNotSet);
        }

        var checkPassword = CheckPassword(oldPassword);
        if (checkPassword.IsFailure)
        {
            return Result.Failure(checkPassword.Error);
        }

        PasswordHash = new PasswordHasher<User>().HashPassword(this, newPassword);
        ModifiedOnUtc = DateTime.UtcNow;
        return Result.Success();
    }
    
    public void ChangePhoneNumber(PhoneNumber phoneNumber)
    {
        PhoneNumber = phoneNumber;
    }

    public Result AssignRole(Role role)
    {
        if (Roles.Any(x=>x.Id == role.Id))
        {
            return Result.Failure(new Error("User.DuplicateRole","You already have this role"));
        }
        Roles.Add(role);
        return Result.Success();
    }

    public Result RemoveRole(Role role)
    {
        if (Roles.All(x => x.Id != role.Id))
        {
            return Result.Failure(new Error("User.NoRole","You don't have this role"));
        }
        Roles.Remove(role);
        return Result.Success();
    }
    
}

