namespace Server.Exceptions;

public class ItemNotFoundException : Exception
{
    public ItemNotFoundException()
        : base("Not Found") { }
}
