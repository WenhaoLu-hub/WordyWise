namespace Domain.Shared;

public sealed class ValidationResult<TValue> :Result<TValue>, IValidationResult
{
    public Error[] Errors { get; }

    private ValidationResult(Error[] errors) : base(default, false,IValidationResult.ValidationError)
    {
        Errors = errors;
    }

    public static ValidationResult<TValue> WithError(Error[] errors) => new(errors);
}