using App.Common;
using App.Common.Enums;
using Domain.Base;

namespace App.BLL.DTO.Entities;

public class CategoryWithCreator : AbstractIdDatabaseEntity
{
    public LangString Name { get; set; } = default!;
    public bool IsPublic { get; set; }
    public bool IsAssignable { get; set; }
    public EPlatform Platform { get; set; }
    public string? IdOnPlatform { get; set; }

    public Author? Creator { get; set; }
}