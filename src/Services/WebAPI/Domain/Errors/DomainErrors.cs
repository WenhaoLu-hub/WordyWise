using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Errors;

public static class DomainErrors
{
    public static class User
    {
        public static readonly Error DuplicatePhoneNumber = new (
            "User.DuplicatePhone",
            "Phone number already exists");
        
        public static readonly Error DuplicateEmail = new (
            "User.DuplicateEmail",
            "Email already exists");

        public static readonly Error PasswordNotSet = new(
            "Password.NotSet",
            "Please set up your password");

        public static readonly Error PasswordNotMatch = new(
            "Password.NotMatch",
            "Password verification fails");

        public static readonly Error PasswordAlreadySet = new(
            "Password.AlreadySet",
            "Already set up password");
        
        public static readonly Func<Email, Error> UserNotFoundByEmail = email => new Error(
            "User.NotFound", 
            $"The User with email {email} is not found");

        public static readonly Func<Guid,Error> UserNotFoundById  = id => new Error(
            "User.NotFound", 
            $"The User with Id {id} is not found");
    }
}