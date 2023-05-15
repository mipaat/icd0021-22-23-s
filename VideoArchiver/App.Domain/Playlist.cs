using App.Common;
using App.Domain.Base;

namespace App.Domain;

public class Playlist : BaseArchiveEntity
{
    public LangString? Title { get; set; }
    public LangString? Description { get; set; }

    public string? DefaultLanguage { get; set; }

    public ImageFileList? Thumbnails { get; set; }
    public List<string>? Tags { get; set; }

    public DateTime? PublishedAt { get; set; }

    public DateTime? LastVideosFetch { get; set; }

    public ICollection<PlaylistVideo>? PlaylistVideos { get; set; }
    public ICollection<PlaylistAuthor>? PlaylistAuthors { get; set; }
    public ICollection<PlaylistSubscription>? PlaylistSubscriptions { get; set; }
    public ICollection<PlaylistRating>? PlaylistRatings { get; set; }
    public ICollection<PlaylistCategory>? PlaylistCategories { get; set; }
    public ICollection<PlaylistHistory>? PlaylistHistories { get; set; }
    public ICollection<StatusChangeEvent>? StatusChangeEvents { get; set; }
    public ICollection<QueueItem>? QueueItems { get; set; }
}