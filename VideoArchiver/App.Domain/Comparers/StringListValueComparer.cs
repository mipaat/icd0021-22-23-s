using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace App.Domain.Comparers;

public class StringListValueComparer : ValueComparer<List<string>>
{
    public StringListValueComparer() : base((l1, l2) =>
        (l1 == null && l2 == null) ||
        (l1 != null && l2 != null && l1.SequenceEqual(l2)),
        l => l.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
        l => l.ToList())
    {
    }
}