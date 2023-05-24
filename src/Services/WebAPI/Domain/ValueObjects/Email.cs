using System.Text.RegularExpressions;
using Domain.Primitives;
using Domain.Shared;

namespace Domain.ValueObjects;

public sealed class Email : ValueObject
{
    public string Value { get; private set; }

    public const int MaxLength = 20;
    
    public const string Pattern = @"^[a-zA-Z0-9._%-+]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";


    private Email(string value)
    {
        Value = value;
    }
    
    private Email()
    {
        
    }
    
    public static Result<Email> Create(string email)
    {
        return Result.Create(email)
            .Ensure(e =>
                    string.IsNullOrWhiteSpace(email),
                new Error("Email.Empty", "Email is empty"))
            .Ensure(e =>
                    !Regex.IsMatch(email, Pattern),
                new Error("Email.Invalid", "Email is not match the pattern"))
            .Ensure(e =>
                    email.Length > MaxLength,
                new Error("Email.TooLong", "Email is too long"))
            .Map(x => new Email(email));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}