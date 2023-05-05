using App.Domain;

namespace App.BLL.Extensions;

public static class DomainHistoryExtensions
{
    public static VideoHistory ToHistory(this Video video)
    {
        return new VideoHistory
        {
            Video = video,

            IdOnPlatform = video.IdOnPlatform,

            Title = video.Title,
            Description = video.Description,

            DefaultLanguage = video.DefaultLanguage,
            DefaultAudioLanguage = video.DefaultAudioLanguage,

            Duration = video.Duration,

            ViewCount = video.ViewCount,
            LikeCount = video.LikeCount,
            DislikeCount = video.DislikeCount,
            CommentCount = video.CommentCount,

            Captions = video.Captions,
            AutomaticCaptions = video.AutomaticCaptions,
            Thumbnails = video.Thumbnails,
            Tags = video.Tags,

            IsLivestreamRecording = video.IsLivestreamRecording,
            StreamId = video.StreamId,
            LivestreamStartedAt = video.LivestreamStartedAt,
            LivestreamEndedAt = video.LivestreamEndedAt,

            CreatedAt = video.CreatedAt,
            PublishedAt = video.PublishedAt,
            UpdatedAt = video.UpdatedAt,
            RecordedAt = video.RecordedAt,
        };
    }
}