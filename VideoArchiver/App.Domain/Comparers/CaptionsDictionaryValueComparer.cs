using App.Domain.NotMapped;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace App.Domain.Comparers;

public class CaptionsDictionaryValueComparer : ValueComparer<CaptionsDictionary?>
{
    public CaptionsDictionaryValueComparer() : base(
        (cd1, cd2) => AreCaptionsDictionariesEqual(cd1, cd2),
        d => d == null ? 0 : d.GetHashCode(),
        d => d == null ? null : d.GetSnapShot())
    {
    }

    private static bool AreCaptionsDictionariesEqual(CaptionsDictionary? cd1,
        CaptionsDictionary? cd2)
    {
        if (cd1 == null && cd2 == null) return true;
        if ((cd1 == null && cd2 != null) || (cd2 == null && cd1 != null)) return false;
        return cd1!.Equals(cd2!);
    }
}