using Domain.DomainEvents;
using Domain.Entities.RoleAggregate;
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
    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();
    public string? PasswordHash { get; private set; }
    
    public DateTime CreatedOnUtc { get; private init; }
    
    public DateTime? ModifiedOnUtc { get; private set; }

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

    public static User? Create(
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
        if (HasPassword())
        {
            return Result.Failure(DomainErrors.User.PasswordAlreadySet);
        }
        var passwordHasher = new PasswordHasher<User>();
        PasswordHash = passwordHasher.HashPassword(this,password);
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

    public void ChangePassword()
    {
        throw new NotImplementedException();
    }
    
    public void ChangePhoneNumber(PhoneNumber phoneNumber)
    {
        PhoneNumber = phoneNumber;
    }

    public void AssignRole(Role role)
    {
        if (UserRoles.Any(x => x.RoleId == role.Id))
        {
            return;
        }
        var userRole = new UserRole(Id, role.Id);
        UserRoles.Add(userRole);
    }
    
}

