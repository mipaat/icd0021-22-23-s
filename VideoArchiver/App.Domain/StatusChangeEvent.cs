using App.Domain.Base;
using App.Domain.Enums;
using Domain.Base;

namespace App.Domain;

public class StatusChangeEvent : AbstractIdDatabaseEntity
{
    public bool? PreviousAvailability { get; set; }
    public bool? NewAvailability { get; set; }
    public EPrivacyStatus? PreviousPrivacyStatus { get; set; }
    public EPrivacyStatus? NewPrivacyStatus { get; set; }
    public DateTime OccurredAt { get; set; }

    public Author? Author { get; set; }
    public Guid? AuthorId { get; set; }
    public Video? Video { get; set; }
    public Guid? VideoId { get; set; }
    public Playlist? Playlist { get; set; }
    public Guid? PlaylistId { get; set; }

    public StatusChangeEvent()
    {
    }

    private StatusChangeEvent(BaseArchiveEntityNonMonitored entity, EPrivacyStatus? newPrivacyStatus,
        bool? newAvailability, DateTime? occurredAt)
    {
        OccurredAt = occurredAt ?? DateTime.UtcNow;
        PreviousAvailability = entity.IsAvailable;
        NewAvailability = newAvailability;
        PreviousPrivacyStatus = entity.PrivacyStatus;
        NewPrivacyStatus = newPrivacyStatus;

        entity.PrivacyStatus = newPrivacyStatus;
        entity.IsAvailable = newAvailability ?? entity.IsAvailable;
    }

    public StatusChangeEvent(Video video, EPrivacyStatus? newPrivacyStatus, bool? newAvailability,
        DateTime? occurredAt = null) :
        this(video as BaseArchiveEntityNonMonitored, newPrivacyStatus, newAvailability, occurredAt)
    {
        Video = video;
    }

    public StatusChangeEvent(Playlist playlist, EPrivacyStatus? newPrivacyStatus, bool? newAvailability,
        DateTime? occurredAt = null) :
        this(playlist as BaseArchiveEntityNonMonitored, newPrivacyStatus, newAvailability, occurredAt)
    {
        Playlist = playlist;
    }

    public StatusChangeEvent(Author author, EPrivacyStatus? newPrivacyStatus, bool? newAvailability,
        DateTime? occurredAt = null) :
        this(author as BaseArchiveEntityNonMonitored, newPrivacyStatus, newAvailability, occurredAt)
    {
        Author = author;
    }
}