namespace BuildingBlocks.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    { }

    public BadRequestException(string name, object key) : base($"Entity \"{name}\" ({key}) was not found")
    { }
}
