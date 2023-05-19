using System.Data;

namespace Domain.Shared;

public class Result
{
    public bool IsSuccess { get; }
    
    public bool IsFailure => !IsSuccess;
    
    public Error Error { get; }

    protected Result(bool isSuccess, Error? error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }
    }

    public static Result Success() => new Result(true,Error.None);
    public static Result<TValue> Success<TValue>(TValue? value) => new (value, true, Error.None);

    public static Result Failure(Error error) => new Result(false,error);

    public static Result<TValue> Failure<TValue>(Error error) => new(default,false,error);
    
    public static Result<TValue> Create<TValue>(TValue? value) => value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
}