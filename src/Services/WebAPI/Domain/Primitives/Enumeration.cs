using System.Reflection;

namespace Domain.Primitives;

public abstract class Enumeration<TEnum>: IEquatable<Enumeration<TEnum>>
    where TEnum : Enumeration<TEnum>
{
    private static readonly Dictionary<int, TEnum> Enumerations = CreateEnumerations();
    
    public int Value { get; protected init; }
    public string Name { get; protected init; }

    protected Enumeration(int value, string name)
    {
        Value = value;
        Name = name;
    }


    public static TEnum? FromValue(int value)
    {
        return Enumerations.TryGetValue(value,
            out TEnum? enumeration)?
                enumeration
                : default;
    }

    public static TEnum? FromName(string name)
    {
        return Enumerations.Values.FirstOrDefault(x => x.Name == name);
    }
    public bool Equals(Enumeration<TEnum>? other)
    {
        if (other is null)
        {
            return false;
        }

        return other.GetType() == GetType() &&
               Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is Enumeration<TEnum> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode() * 17;
    }
    
    private static Dictionary<int, TEnum> CreateEnumerations()
    {
        var enumerationType = typeof(TEnum);
        var fieldsForType = enumerationType.GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy)
            .Where(info => 
                enumerationType.IsAssignableFrom(info.FieldType))
            .Select(info => 
                (TEnum)info.GetValue(default)!);
        return fieldsForType.ToDictionary(x => x.Value);

    }

}