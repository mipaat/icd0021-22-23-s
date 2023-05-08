using App.Domain.NotMapped;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace App.Domain.Comparers;

public class ImageFileListValueComparer : ValueComparer<ImageFileList?>
{
    public ImageFileListValueComparer() : base((il1, il2) => AreImageFileListsEqual(il1, il2),
        il => il == null ? 0 : il.GetHashCode(),
        il => il == null ? null : il.GetSnapShot())
    {
    }

    private static bool AreImageFileListsEqual(ImageFileList? il1, ImageFileList? il2)
    {
        if (il1 == null && il2 == null) return true;
        if ((il1 == null && il2 != null) || (il2 == null && il1 != null)) return false;
        return il1!.Equals(il2!);
    }
}