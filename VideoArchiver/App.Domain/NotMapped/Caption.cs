using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Enums;

namespace App.Domain.NotMapped;

[NotMapped]
public class Caption
{
    public Platform Platform { get; set; } = default!;
    public string? IdOnPlatform { get; set; }
    public string? Key { get; set; }
    public string FilePath { get; set; } = default!;
    public string? Etag { get; set; }
}