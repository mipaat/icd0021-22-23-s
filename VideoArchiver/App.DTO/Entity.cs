using App.Domain;

namespace App.DTO;

public class Entity
{
    public readonly Video? Video;
    public readonly Author? Author;
    public readonly Playlist? Playlist;

    private Entity(Video video)
    {
        Video = video;
    }

    private Entity(Author author)
    {
        Author = author;
    }

    private Entity(Playlist playlist)
    {
        Playlist = playlist;
    }

    public static implicit operator Entity(Video video) => new(video);
    public static implicit operator Entity(Author author) => new(author);
    public static implicit operator Entity(Playlist playlist) => new(playlist);
}