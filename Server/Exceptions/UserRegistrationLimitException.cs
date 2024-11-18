namespace Server.Exceptions;

public class UserRegistrationLimitException : Exception
{
    public UserRegistrationLimitException()
        : base("User registration limit reached") { }
}
