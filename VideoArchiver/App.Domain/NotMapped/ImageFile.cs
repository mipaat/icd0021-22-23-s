using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Enums;

namespace App.Domain.NotMapped;

[NotMapped]
public class ImageFile
{
    public Platform Platform { get; set; } = default!;
    public string? IdOnPlatform { get; set; }
    public string? Key { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string? LocalFilePath { get; set; }
    public string? Etag { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
}