namespace App.BLL.Exceptions;

public class VideoNotFoundException : NotFoundException
{
    public VideoNotFoundException(string? message = null) : base(message)
    {
    }

    public VideoNotFoundException(string id, string? message = null) :
        base(id, message ?? $"Video with ID {id} not found")
    {
    }
}