using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.NotMapped;

[NotMapped]
public class Caption
{
    public Platform Platform { get; set; } = default!;
    public string? IdOnPlatform { get; set; }
    public string? Key { get; set; }
    public string FilePath { get; set; } = default!;
    public string? Etag { get; set; }
}