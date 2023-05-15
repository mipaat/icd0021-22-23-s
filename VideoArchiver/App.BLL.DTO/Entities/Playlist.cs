using App.Common.Enums;

namespace App.BLL.DTO.Entities;

public class Playlist
{
    public Guid Id { get; set; }
    public EPlatform Platform { get; set; } = default!;
    public string IdOnPlatform { get; set; } = default!;
}