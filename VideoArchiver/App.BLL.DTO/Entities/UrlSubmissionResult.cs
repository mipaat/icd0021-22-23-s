using App.BLL.DTO.Enums;
using App.Common.Enums;

namespace App.BLL.DTO.Entities;

public class UrlSubmissionResult
{
    public readonly EUrlSubmissionResultType Type;
    public readonly Guid Id;
    public readonly EPlatform? Platform;
    public readonly string? IdOnPlatform;
    public readonly bool AlreadyAdded;

    public UrlSubmissionResult(Guid id, EUrlSubmissionResultType type, EPlatform? platform, string? idOnPlatform = null,
        bool alreadyAdded = false)
    {
        Id = id;
        Type = type;
        Platform = platform;
        IdOnPlatform = idOnPlatform;
        AlreadyAdded = alreadyAdded;
    }

    public UrlSubmissionResult(App.DAL.DTO.Entities.QueueItem queueItem, bool alreadyAdded = false) :
        this(queueItem.Id, EUrlSubmissionResultType.QueueItem, queueItem.Platform, queueItem.IdOnPlatform, alreadyAdded)
    {
    }

    public UrlSubmissionResult(App.DAL.DTO.Base.BaseArchiveEntityNonMonitored entity, bool alreadyAdded = false) :
        this(entity.Id, entity.UrlSubmissionResultType(), entity.Platform, entity.IdOnPlatform, alreadyAdded)
    {
    }
}

internal static class UrlSubmissionResultTypeExtensions
{
    public static EUrlSubmissionResultType UrlSubmissionResultType(
        this App.DAL.DTO.Base.BaseArchiveEntityNonMonitored entity)
    {
        return entity switch
        {
            DAL.DTO.Entities.Video => EUrlSubmissionResultType.Video,
            DAL.DTO.Entities.Playlists.Playlist => EUrlSubmissionResultType.Playlist,
            DAL.DTO.Entities.Author => EUrlSubmissionResultType.Author,
            _ => throw new ArgumentException($"Entity is not a valid {typeof(EUrlSubmissionResultType)}"),
        };
    }
}