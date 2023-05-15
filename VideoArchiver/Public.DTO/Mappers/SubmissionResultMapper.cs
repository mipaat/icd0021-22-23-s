using App.BLL.DTO.Entities;
using App.Common.Enums;

#pragma warning disable 1591
namespace Public.DTO.Mappers;

public static class SubmissionResultMapper
{
    public static v1.SubmissionResult Map(UrlSubmissionResult bllSubmissionResult)
    {
        var submissionResult = new v1.SubmissionResult
        {
            Type = bllSubmissionResult.GetEntityType(),
            Platform = bllSubmissionResult.Entity?.Platform.GetPlatform() ??
                       bllSubmissionResult.QueueItem?.Platform?.GetPlatform() ??
                       throw new ArgumentException(
                           $"Invalid {typeof(UrlSubmissionResult)} - failed to parse platform!"),
            Id = bllSubmissionResult.Entity?.Author?.Id ??
                 bllSubmissionResult.Entity?.Video?.Id ??
                 bllSubmissionResult.Entity?.Playlist?.Id ??
                 bllSubmissionResult.QueueItem?.Id ??
                 throw new ArgumentException(
                     $"Invalid {typeof(UrlSubmissionResult)} - failed to parse ID!"),
            IsQueueItem = bllSubmissionResult.QueueItem != null,
            AlreadyAdded = bllSubmissionResult.AlreadyAdded,
        };

        return submissionResult;
    }

    public static List<v1.SubmissionResult> Map(UrlSubmissionResults urlSubmissionResults)
    {
        return urlSubmissionResults.Select(Map).ToList();
    }

    private static v1.EEntityType? GetEntityType(this UrlSubmissionResult bllSubmissionResult)
    {
        if (bllSubmissionResult.Entity?.Author != null) return v1.EEntityType.Author;
        if (bllSubmissionResult.Entity?.Video != null) return v1.EEntityType.Video;
        if (bllSubmissionResult.Entity?.Playlist != null) return v1.EEntityType.Playlist;
        return null;
    }

    private static v1.EPlatform GetPlatform(this EPlatform platform)
    {
        return platform switch
        {
            EPlatform.This => v1.EPlatform.This,
            EPlatform.YouTube => v1.EPlatform.YouTube,
            _ => throw new ArgumentException($"Invalid domain platform for {typeof(v1.SubmissionResult)}: {platform}"),
        };
    }
}