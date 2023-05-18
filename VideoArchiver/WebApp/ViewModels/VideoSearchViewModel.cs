using App.BLL.DTO.Entities;

namespace WebApp.ViewModels;

public class VideoSearchViewModel
{
    public ICollection<VideoWithAuthor> Videos { get; set; } = new List<VideoWithAuthor>();
    public string? NameQuery { get; set; }
    public string? AuthorQuery { get; set; }
    public ICollection<Guid>? CategoryQuery { get; set; }
}