namespace Domain.Shared;

public interface IValidationResult
{
    public static readonly Error ValidationResult = new(
        "ValidationError",
        "A validation problem occured.");

    Error[] Errors { get; }
}