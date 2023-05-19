namespace Domain.Shared;

public sealed class ValidationResult :Result, IValidationResult
{
    public Error[] Errors { get; }

    public ValidationResult(Error[] errors) : base(false,IValidationResult.ValidationResult)
    {
        Errors = errors;
    }

    public static ValidationResult WithError(Error[] errors) => new(errors);
}