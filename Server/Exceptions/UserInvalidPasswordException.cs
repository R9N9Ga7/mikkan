namespace Server.Exceptions;

public class UserInvalidPasswordException : Exception
{
    public UserInvalidPasswordException() : base("Invalid password") { }
}
