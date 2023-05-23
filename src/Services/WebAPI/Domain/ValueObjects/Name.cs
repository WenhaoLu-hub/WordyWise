using Domain.Primitives;
using Domain.Shared;

namespace Domain.ValueObjects;

public sealed class Name : ValueObject
{
    public string Value { get; }

    public const int MaxLength = 20;

    private Name(string value)
    {
        Value = value;
    }

    public Name()
    {
    }

    public static Result<Name> Create(string name)
    { 
        return  Result.Create(name)
            .Ensure(e =>
                    !string.IsNullOrEmpty(name),
                new Error("Name.Empty", "Name is empty"))
            .Ensure(e =>
                    name.Length < MaxLength,
                new Error("Name.TooLong", "Name is too long"))
            .Map(x => new Name(name));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}