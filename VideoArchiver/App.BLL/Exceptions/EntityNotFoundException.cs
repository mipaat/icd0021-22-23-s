namespace App.BLL.Exceptions;

public class EntityNotFoundException : Exception
{
    public readonly string Id;

    public EntityNotFoundException(string id, string? message = null) : 
        base(message ?? $"Entity with id {id} not found")
    {
        Id = id;
    }
}