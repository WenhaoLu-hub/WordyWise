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

    public PhoneNumber()
    {
    }

    public static Result<PhoneNumber> Create(string phoneNumber)
    {
        return Result.Create(phoneNumber)
            .Ensure(e =>
                    phoneNumber.Length <= MaxLength,
                new Error("PhoneNumber.TooLong", "PhoneNumber is too long"))
            .Ensure(e =>
                    phoneNumber.All(char.IsDigit),
                new Error("PhoneNumber.NotDigit", "PhoneNumber is not digits"))
            .Map(e => new PhoneNumber(phoneNumber));

    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}