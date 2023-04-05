using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;

namespace Domain.NotMapped;

[NotMapped]
public class ImageFile
{
    public string? Key { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string? LocalFilePath { get; set; }
    public string? Etag { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public bool IsNotDifferent(ImageFile? entity)
    {
        if (entity == null) return false;
        return Key == entity.Key && Url == entity.Url && LocalFilePath == entity.LocalFilePath && Etag == entity.Etag &&
               Width == entity.Width && Height == entity.Height;
    }

    public int GetStateHashCode()
    {
        throw new NotImplementedException();
    }

    public ImageFile GetSnapshot()
    {
        throw new NotImplementedException();
    }
}