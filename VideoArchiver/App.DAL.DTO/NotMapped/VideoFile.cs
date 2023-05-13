using System.ComponentModel.DataAnnotations.Schema;

namespace App.DAL.DTO.NotMapped;

public class VideoFile
{
    public string? Key { get; set; }
    public string FilePath { get; set; } = default!;
    public string? Etag { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? BitrateBps { get; set; }
}