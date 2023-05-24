namespace Presentation.RequestParams.User;

public sealed record RegisterUserRequest(string Name, string PhoneNumber, string Email);