using App.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace App.Domain.Comparers;

// public class LangStringValueComparer : ValueComparer<LangString>
// {
//     public LangStringValueComparer() : base(
//         (l, r) => ,
//         e => e.GetHashCode(),
//         e => new LangString(e.ToDictionary(kvp => kvp.Key, kvp => kvp.Value))
//         )
//     {
//     }
// }