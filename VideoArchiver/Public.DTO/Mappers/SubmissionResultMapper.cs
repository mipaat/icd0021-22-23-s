using App.BLL.DTO.Entities;

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

    private static v1.EPlatform GetPlatform(this App.BLL.DTO.Enums.Platform domainPlatform)
    {
        return domainPlatform == App.Domain.Enums.Platform.YouTube
            ? v1.EPlatform.YouTube
            : throw new ArgumentException($"Invalid domain platform for {typeof(v1.SubmissionResult)}");
    }
}