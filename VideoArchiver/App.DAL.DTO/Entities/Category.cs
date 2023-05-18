using App.Common.Enums;
using App.Common;
using Domain.Base;

namespace App.DAL.DTO.Entities;

public class Category : AbstractIdDatabaseEntity
{
    public LangString Name { get; set; } = default!;
    public bool IsPublic { get; set; }
    public bool SupportsAuthors { get; set; } = true;
    public bool SupportsVideos { get; set; } = true;
    public bool SupportsPlaylists { get; set; } = true;
    public bool IsAssignable { get; set; }
    public EPlatform Platform { get; set; }
    public string? IdOnPlatform { get; set; }
}