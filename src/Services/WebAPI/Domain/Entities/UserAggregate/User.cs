using Domain.Entities.RoleAggregate;
using Domain.Primitives;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.UserAggregate;

public class User : AggregateRoot
{
    public Name Name { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();
    public string? PasswordHash { get; private set; }
    
    public DateTime CreatedOnUtc { get; private init; }
    
    public DateTime? ModifiedOnUtc { get; private set; }


    private User(Guid id, Name name, string phoneNumber, string? passwordHash, string email) : base(id)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        PasswordHash = passwordHash;
        Email = email;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public static User Create(Guid id,
        Name name,
        string phoneNumber,
        string? passwordHash,
        string email)
    {
        var user = new User(id, name, phoneNumber, passwordHash, email);
        
        //raise domain event here if needed

        return user;
    }

    public void ChangeName(Name name)
    {
        Name = name;
    }

    public void SetPassword(string password)
    {
        //validation first
        var passwordHasher = new PasswordHasher<User>();
        PasswordHash = passwordHasher.HashPassword(this,password);
    }

    public bool CheckPassword()
    {
        throw new NotImplementedException();
    }

    public void ChangePassword()
    {
        throw new NotImplementedException();
    }
    
    public void ChangePhoneNumber(string phoneNumber)
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