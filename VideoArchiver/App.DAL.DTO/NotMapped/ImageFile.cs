using System.ComponentModel.DataAnnotations.Schema;
using App.DAL.DTO.Enums;

namespace App.DAL.DTO.NotMapped;

public class ImageFile
{
    public Platform Platform { get; set; } = default!;
    public string? IdOnPlatform { get; set; }
    public string? Key { get; set; }
    public string? Quality { get; set; }
    public string Url { get; set; } = default!;
    public string? LocalFilePath { get; set; }
    public string? Etag { get; set; }
    public string? Hash { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }

    public ImageFile GetSnapShot() => new()
    {
        Platform = Platform,
        IdOnPlatform = IdOnPlatform,
        Key = Key,
        Quality = Quality,
        Url = Url,
        LocalFilePath = LocalFilePath,
        Etag = Etag,
        Hash = Hash,
        Width = Width,
        Height = Height,
    };

    public override bool Equals(object? obj)
    {
        if (obj is not ImageFile imageFile) return false;
        return Platform == imageFile.Platform &&
               IdOnPlatform == imageFile.IdOnPlatform &&
               Key == imageFile.Key &&
               Quality == imageFile.Quality &&
               Url == imageFile.Url &&
               Utils.Utils.EqualsOrNull(LocalFilePath, imageFile.LocalFilePath) &&
               Utils.Utils.EqualsOrNull(Etag, imageFile.Etag) &&
               Utils.Utils.EqualsOrNull(Hash, imageFile.Hash) &&
               Width == imageFile.Width &&
               Height == imageFile.Height;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Platform, IdOnPlatform, Key, Quality, Url, LocalFilePath, Etag, Hash);
    }
}