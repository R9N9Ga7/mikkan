namespace Server.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException()
        : base("Bad Request") { }
}
