using System.Reflection;

namespace Domain.Primitives;

public abstract class Enumeration<TEnum>: IEquatable<Enumeration<TEnum>>
    where TEnum : Enumeration<TEnum>
{
    private static readonly Lazy<Dictionary<int, TEnum>> EnumerationsDictionary =
        new(() => CreateEnumerationDictionary(typeof(TEnum)));
    
    public int Id { get; protected init; }
    public string Name { get; protected init; }

    protected Enumeration(int value, string name)
    {
        Id = value;
        Name = name;
    }


    public static TEnum? FromValue(int value)
    {
        return EnumerationsDictionary.Value.TryGetValue(value,
            out TEnum? enumeration)?
                enumeration
                : default;
    }

    public static IReadOnlyCollection<TEnum> GetValues()
    {
        return EnumerationsDictionary.Value.Values.ToList();
    }

    public static TEnum? FromName(string name)
    {
        return EnumerationsDictionary.Value.Values.FirstOrDefault(x => x.Name == name);
    }
    public bool Equals(Enumeration<TEnum>? other)
    {
        if (other is null)
        {
            return false;
        }

        return other.GetType() == GetType() &&
               Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        return obj is Enumeration<TEnum> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode() * 17;
    }
    
    
    private static Dictionary<int, TEnum> CreateEnumerationDictionary(Type enumType)
    {
        return GetFieldsForType(enumType).ToDictionary(t => t.Id);
    }
    
    private static IEnumerable<TEnum> GetFieldsForType(Type enumType)
    {
        return enumType.GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy)
            .Where(fieldInfo => 
                enumType.IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo => 
                (TEnum)fieldInfo.GetValue(default)!);
    }
}