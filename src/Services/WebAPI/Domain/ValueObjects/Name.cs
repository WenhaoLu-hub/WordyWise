using Domain.Primitives;
using Domain.Shared;

namespace Domain.ValueObjects;

public sealed class Name : ValueObject
{
    public string Value { get; private set; }

    public const int MaxLength = 20;

    private Name(string value)
    {
        Value = value;
    }
    
    public static Result<Name> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Name>(new Error("Name.Empty", "Name is empty"));
        }

        if (name.Length > MaxLength)
        {
            return Result.Failure<Name>(new Error("Name.TooLong", "Name is too long"));
        }

        return new Name(name);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}