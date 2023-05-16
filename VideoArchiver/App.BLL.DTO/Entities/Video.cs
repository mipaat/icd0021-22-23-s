using App.Common;

namespace App.BLL.DTO.Entities;

public class Video
{
    public Guid Id { get; set; }
    public LangString? Title { get; set; }
    public LangString? Description { get; set; }
    public string Url { get; set; } = default!;
    public bool IsAvailable { get; set; }
}