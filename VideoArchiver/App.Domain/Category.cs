using App.Domain.Enums;
using App.Domain.NotMapped;
using Domain.Base;

namespace App.Domain;

public class Category : AbstractIdDatabaseEntity
{
    public LangString Name { get; set; } = default!;
    public bool IsPublic { get; set; }
    public bool SupportsAuthors { get; set; } = true;
    public bool SupportsVideos { get; set; } = true;
    public bool SupportsPlaylists { get; set; } = true;
    public bool IsAssignable { get; set; }
    public Category? ParentCategory { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public Platform? Platform { get; set; }

    public Author? Creator { get; set; }
    public Guid? CreatorId { get; set; }

    public ICollection<AuthorCategory>? AuthorCategories { get; set; }
    public ICollection<VideoCategory>? VideoCategories { get; set; }
    public ICollection<PlaylistCategory>? PlaylistCategories { get; set; }

    public ICollection<AuthorRating>? AuthorRatings { get; set; }
    public ICollection<VideoRating>? VideoRatings { get; set; }
    public ICollection<PlaylistRating>? PlaylistRatings { get; set; }

    public ICollection<Category>? DirectChildCategories { get; set; } 
}