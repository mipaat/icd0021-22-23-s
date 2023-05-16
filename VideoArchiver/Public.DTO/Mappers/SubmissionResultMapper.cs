using App.BLL.DTO.Entities;
using App.BLL.DTO.Enums;
using EPlatform = App.Common.Enums.EPlatform;

#pragma warning disable 1591
namespace Public.DTO.Mappers;

public static class SubmissionResultMapper
{
    public static v1.SubmissionResult Map(UrlSubmissionResult bllSubmissionResult)
    {
        var submissionResult = new v1.SubmissionResult
        {
            Id = bllSubmissionResult.Id,
            Type = bllSubmissionResult.GetEntityType(),
            Platform = bllSubmissionResult.Platform?.GetPlatform() ?? throw new ArgumentException(
                $"Invalid {typeof(UrlSubmissionResult)} - failed to parse platform!"),
            IsQueueItem = bllSubmissionResult.Type == EUrlSubmissionResultType.QueueItem,
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
        return bllSubmissionResult.Type switch
        {
            EUrlSubmissionResultType.Author => v1.EEntityType.Author,
            EUrlSubmissionResultType.Video => v1.EEntityType.Video,
            EUrlSubmissionResultType.Playlist => v1.EEntityType.Playlist,
            _ => null,
        };
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