using Domain.Base;
using Domain.Enums;
using Domain.NotMapped;

namespace Domain;

public class Category : AbstractIdDatabaseEntity
{
    public LangString Name { get; set; } = default!;
    public bool IsPublic { get; set; }
    public bool SupportsAuthors { get; set; }
    public bool SupportsVideos { get; set; }
    public bool SupportsPlaylists { get; set; }
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