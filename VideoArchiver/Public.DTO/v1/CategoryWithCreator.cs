namespace Public.DTO.v1;

/// <summary>
/// Information about a category.
/// </summary>
public class CategoryWithCreator
{
    /// <summary>
    /// The unique ID of the category.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// The name of the category.
    /// </summary>
    public LangString Name { get; set; } = default!;
    /// <summary>
    /// Whether the category is public,
    /// allowing any users of the archive to
    /// see it and interact with it.
    /// </summary>
    public bool IsPublic { get; set; }
    /// <summary>
    /// Whether this category can be manually assigned to entities on its platform of origin.
    /// If this is true, but the category is not native to the archive,
    /// archive users still won't be able to assign it.
    /// </summary>
    public bool IsAssignable { get; set; }
    /// <summary>
    /// The category's platform of origin.
    /// </summary>
    public EPlatform Platform { get; set; }
    /// <summary>
    /// The category's ID of its platform of origin.
    /// </summary>
    public string? IdOnPlatform { get; set; }
    
    /// <summary>
    /// The creator of the category.
    /// </summary>
    public Author? Creator { get; set; }
}