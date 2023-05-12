using App.BLL.DTO.Enums;

namespace App.BLL.DTO.Entities;

public class Playlist
{
    public Guid Id { get; set; }
    public Platform Platform { get; set; } = default!;
    public string IdOnPlatform { get; set; } = default!;
}