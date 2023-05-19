using Domain.Primitives;
using Domain.Shared;

namespace Domain.ValueObjects;

public sealed class PhoneNumber : ValueObject
{
    public string Value { get; private set; }
    public static int MaxLength = 15;

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static Result<PhoneNumber> Create(string value)
    {
        if (value.Length > MaxLength)
        {
            return Result.Failure<PhoneNumber>(new Error("PhoneNumber.TooLong","PhoneNumber is too long"));
        }

        return new PhoneNumber(value);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}