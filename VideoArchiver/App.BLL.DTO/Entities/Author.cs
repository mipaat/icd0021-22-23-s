using App.Common;
using App.Common.Enums;

namespace App.BLL.DTO.Entities;

public class Author
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? DisplayName { get; set; }
    public EPlatform Platform { get; set; }
    public string IdOnPlatform { get; set; } = default!;
    public ImageFileList? ProfileImages { get; set; }
    public string? UrlOnPlatform { get; set; }
}