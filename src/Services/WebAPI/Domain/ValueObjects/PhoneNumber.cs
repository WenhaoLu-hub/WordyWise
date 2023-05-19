using Domain.Primitives;

namespace Domain.ValueObjects;

public class PhoneNumber : ValueObject
{
    public string CountryCode;
    public string Number;

    public static int MaxLength;

    public PhoneNumber(string countryCode, string number)
    {
        CountryCode = countryCode;
        Number = number;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        throw new NotImplementedException();
    }
}