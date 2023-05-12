namespace App.DAL.DTO.NotMapped;

public class ImageFileList : List<ImageFile>
{
    public ImageFileList GetSnapShot()
    {
        var result = new ImageFileList();
        foreach (var imageFile in this)
        {
            result.Add(imageFile.GetSnapShot());
        }

        return result;
    }

    public override bool Equals(object? obj)
    {
        return obj is ImageFileList imageFileList && this.All(imageFileList.Contains);
    }

    public override int GetHashCode()
    {
        return this.OrderBy(imageFile => imageFile.GetHashCode())
            .Aggregate(17, (i, imageFile) => HashCode.Combine(i, imageFile.GetHashCode()));
    }

    public ImageFileList()
    {
    }

    public ImageFileList(IEnumerable<ImageFile> imageFiles) : base(imageFiles)
    {
    }
}