namespace App.BLL.Exceptions;

public class NotFoundException : Exception
{
    public readonly string? Id;

    public NotFoundException(string? message = null) : base(message)
    {
    }

    public NotFoundException(string id, string? message = null) : 
        base(message ?? $"Entity with id {id} not found")
    {
        Id = id;
    }
}