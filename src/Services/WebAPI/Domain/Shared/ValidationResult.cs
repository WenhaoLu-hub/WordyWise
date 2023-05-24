namespace Domain.Shared;

public sealed class ValidationResult :Result, IValidationResult
{
    public Error[] Errors { get; }

    private ValidationResult(
        Error[] errors)
        : base(false,IValidationResult.ValidationError)
    {
        Errors = errors;
    }

    public static ValidationResult WithError(Error[] errors) => new(errors);
}