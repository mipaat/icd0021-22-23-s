using System.ComponentModel.DataAnnotations.Schema;
using App.Common.Enums;

namespace App.Common;

[NotMapped]
public class Caption
{
    public EPlatform Platform { get; set; }
    public string? IdOnPlatform { get; set; }
    public string Ext { get; set; } = default!;
    public string? FilePath { get; set; }
    public string? Url { get; set; }
    public string? Name { get; set; }
    public string? Etag { get; set; }

    public bool Equals(Caption caption)
    {
        return Platform == caption.Platform &&
               IdOnPlatform == caption.IdOnPlatform &&
               Ext == caption.Ext &&
               FilePath == caption.FilePath &&
               Url == caption.Url &&
               Name == caption.Name &&
               Etag == caption.Etag;
    }

    public Caption Clone()
    {
        return new Caption
        {
            IdOnPlatform = IdOnPlatform,
            Platform = Platform,
            Ext = Ext,
            FilePath = FilePath,
            Url = Url,
            Name = Name,
            Etag = Etag,
        };
    }

    public int GetCustomHashCode()
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 23 + Platform.GetHashCode();
            hash = hash * 23 + (IdOnPlatform?.GetHashCode() ?? 0);
            hash = hash * 23 + Ext.GetHashCode();
            hash = hash * 23 + (FilePath?.GetHashCode() ?? 0);
            hash = hash * 23 + (Url?.GetHashCode() ?? 0);
            hash = hash * 23 + (Name?.GetHashCode() ?? 0);
            hash = hash * 23 + (Etag?.GetHashCode() ?? 0);
            return hash;
        }
    }
}