using System.ComponentModel.DataAnnotations;
using App.Common;
using App.Common.Enums;
using Domain.Base;

namespace App.Domain;

public class Category : AbstractIdDatabaseEntity
{
    public LangString Name { get; set; } = default!;
    public bool IsPublic { get; set; }
    public bool IsAssignable { get; set; }
    public EPlatform Platform { get; set; }
    [MaxLength(128)] public string? IdOnPlatform { get; set; }

    public Author? Creator { get; set; }
    public Guid? CreatorId { get; set; }

    public ICollection<AuthorCategory>? AuthorCategories { get; set; }
    public ICollection<VideoCategory>? VideoCategories { get; set; }
    public ICollection<PlaylistCategory>? PlaylistCategories { get; set; }

    public ICollection<AuthorRating>? AuthorRatings { get; set; }
    public ICollection<VideoRating>? VideoRatings { get; set; }
    public ICollection<PlaylistRating>? PlaylistRatings { get; set; }
}