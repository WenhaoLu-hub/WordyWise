namespace Presentation.RequestParams.User;

public sealed record ChangePasswordRequest(string OldPassword, string NewPassword);