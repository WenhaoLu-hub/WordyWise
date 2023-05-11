namespace Domain.Shared;

public class Error : IEquatable<Error>
{
    public string Code { get; }
    
    public string Message { get; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public static implicit operator string(Error error) => error.Code;

    public static readonly Error None = new(string.Empty, string.Empty);

    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null");

    public static bool operator ==(Error? left, Error? right)
    {
        if (left is null && right is null) // both left and right are null 
        {
            return true;
        }

        if (left is null || right is null) //only one of them is null
        {
            return false;
        }

        return left.Equals(right);
    }

    public static bool operator !=(Error? left, Error? right)
    {
        return !(left == right);
    }
    
    public bool Equals(Error? other)
    {
        if (other is null)
        {
            return false;
        }

        return Code == other.Code &&
               Message == other.Message;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Error error)
        {
            return false;
        }

        return Equals(obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Code, Message);
    }

    public override string ToString()
    {
        return Code;
    }
}