using App.Common.Enums;
using App.Common;
using Domain.Base;

namespace App.DAL.DTO.Entities;

public class CategoryWithCreator : AbstractIdDatabaseEntity
{
    public LangString Name { get; set; } = default!;
    public bool IsPublic { get; set; }
    public bool IsAssignable { get; set; }
    public EPlatform Platform { get; set; }
    public string? IdOnPlatform { get; set; }

    public AuthorBasic? Creator { get; set; }
    public Guid? CreatorId { get; set; }
}