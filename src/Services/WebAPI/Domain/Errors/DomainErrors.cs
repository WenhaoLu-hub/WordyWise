using Domain.Shared;

namespace Domain.Errors;

public static class DomainErrors
{
    public static class User
    {
        public static readonly Error DuplicatePhoneNumber = new (
            "User.DuplicatePhone",
            "Phone number already exist");
        
    }
}