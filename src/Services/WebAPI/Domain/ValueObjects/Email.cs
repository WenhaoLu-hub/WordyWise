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
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Failure<Email>(new Error("Name.Empty", "Name is empty"));
        }
        
        if(!Regex.IsMatch(email, Pattern))
        {
            return Result.Failure<Email>(new Error("Name.Invalid", "email is not match the pattern"));
        }

        if (email.Length > MaxLength)
        {
            return Result.Failure<Email>(new Error("Name.TooLong", "Name is too long"));
        }

        return new Email(email);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}