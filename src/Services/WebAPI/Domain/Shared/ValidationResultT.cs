namespace Domain.Shared;

public sealed class ValidationResult<TValue> :Result, IValidationResult
{
    public Error[] Errors { get; }

    public ValidationResult(Error[] errors) : base(false,IValidationResult.ValidationResult)
    {
        Errors = errors;
    }

    public static ValidationResult<TValue> WithError(Error[] errors) => new(errors);
}