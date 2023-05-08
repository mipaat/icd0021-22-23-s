using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace App.Domain.Comparers;

public class StringListUnorderedValueComparer : ValueComparer<List<string>>
{
    public StringListUnorderedValueComparer() : base((l1, l2) =>
        (l1 == null && l2 == null) ||
        (l1 != null && l2 != null && l1.All(l2.Contains)),
        l => l.OrderBy(s => s.GetHashCode()).Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
        l => l.ToList())
    {
    }
}