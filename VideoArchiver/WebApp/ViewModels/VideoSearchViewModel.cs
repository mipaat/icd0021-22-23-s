using App.BLL.DTO.Entities;
using App.Common.Enums;

namespace WebApp.ViewModels;

public class VideoSearchViewModel
{
    public ICollection<VideoWithAuthor> Videos { get; set; } = new List<VideoWithAuthor>();
    public EPlatform? PlatformQuery { get; set; }
    public string? NameQuery { get; set; }
    public string? AuthorQuery { get; set; }
}