namespace App.BLL.Exceptions;

public class PlaylistNotFoundException : EntityNotFoundException
{
    public PlaylistNotFoundException(string? message = null) : base(message)
    {
    }

    public PlaylistNotFoundException(string id, string? message = null) :
        base(id, message ?? $"Playlist with ID {id} not found")
    {
    }
}