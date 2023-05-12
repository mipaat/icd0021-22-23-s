using Platform = App.BLL.DTO.Enums.Platform;

namespace App.BLL.DTO.Entities;

public class Entity
{
    public readonly Video? Video;
    public readonly Author? Author;
    public readonly Playlist? Playlist;

    public Platform Platform =>
        Video?.Platform ?? Author?.Platform ??
        Playlist?.Platform ?? throw new ApplicationException("At least one entity must be not null!");

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