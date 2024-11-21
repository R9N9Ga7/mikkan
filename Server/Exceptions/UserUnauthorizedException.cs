namespace Server.Exceptions;

public class UserUnauthorizedException : Exception
{
    public UserUnauthorizedException()
        : base("Unauthorized") { }
}
