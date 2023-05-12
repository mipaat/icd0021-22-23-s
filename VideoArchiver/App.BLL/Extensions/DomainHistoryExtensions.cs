using App.DAL.DTO.Entities;

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

            LocalVideoFiles = video.LocalVideoFiles,

            LastValidAt = DateTime.UtcNow,
        };
    }

    public static CommentHistory ToHistory(this Comment comment)
    {
        return new CommentHistory
        {
            Comment = comment,
            
            IdOnPlatform = comment.IdOnPlatform,
            
            Content = comment.Content,
            LikeCount = comment.LikeCount,
            DislikeCount = comment.DislikeCount,
            ReplyCount = comment.ReplyCount,
            IsFavorited = comment.IsFavorited,

            LastValidAt = DateTime.UtcNow,
        };
    }

    public static PlaylistHistory ToHistory(this Playlist playlist)
    {
        return new PlaylistHistory
        {
            Playlist = playlist,

            IdOnPlatform = playlist.IdOnPlatform,

            Title = playlist.Title,
            Description = playlist.Description,

            Thumbnails = playlist.Thumbnails,
            Tags = playlist.Tags,

            CreatedAt = playlist.CreatedAt,
            PublishedAt = playlist.UpdatedAt,
            UpdatedAt = playlist.UpdatedAt,

            LastValidAt = DateTime.UtcNow,
        };
    }
}