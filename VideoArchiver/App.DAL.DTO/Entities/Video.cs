using System.ComponentModel.DataAnnotations;
using App.DAL.DTO.Base;
using App.DAL.DTO.NotMapped;

namespace App.DAL.DTO.Entities;

public class Video : BaseArchiveEntity
{
    public LangString? Title { get; set; }
    public LangString? Description { get; set; }

    [MaxLength(32)] public string? DefaultLanguage { get; set; }
    [MaxLength(32)] public string? DefaultAudioLanguage { get; set; }

    public TimeSpan? Duration { get; set; }

    public long? ViewCount { get; set; }
    public long? LikeCount { get; set; }
    public long? DislikeCount { get; set; }
    public long? CommentCount { get; set; }

    public CaptionsDictionary? Captions { get; set; }
    public CaptionsDictionary? AutomaticCaptions { get; set; }
    public ImageFileList? Thumbnails { get; set; }
    public List<string>? Tags { get; set; }

    public bool? IsLivestreamRecording { get; set; }
    [MaxLength(64)] public string? StreamId { get; set; }
    public DateTime? LivestreamStartedAt { get; set; }
    public DateTime? LivestreamEndedAt { get; set; }

    public DateTime? PublishedAt { get; set; }
    public DateTime? RecordedAt { get; set; }

    public List<VideoFile>? LocalVideoFiles { get; set; }
    
    public DateTime? LastCommentsFetch { get; set; }

    public ICollection<VideoGame>? VideoGames { set; get; }
    public ICollection<VideoAuthor>? VideoAuthors { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<VideoRating>? VideoRatings { get; set; }
    public ICollection<VideoCategory>? VideoCategories { get; set; }
    public ICollection<StatusChangeEvent>? StatusChangeEvents { get; set; }
    public ICollection<VideoHistory>? VideoHistories { get; set; }
}