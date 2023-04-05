using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Domain.Comparers;

public class JsonSerializableValueComparer<T> : ValueComparer<T> where T : new()
{
    public JsonSerializableValueComparer() : base((e1, e2) => EntitiesAreUnchanged(e1, e2),
        e => GetEntityHashCode(e),
        e => GetSnapshot(e))
    {
    }

    private static bool EntitiesAreUnchanged(T? e1, T? e2)
    {
        if (e1 == null && e2 == null) return true;
        if (e1 == null || e2 == null) return false;

        return JsonSerializer.Serialize(e1) == JsonSerializer.Serialize(e2);
    }

    private static int GetEntityHashCode(T entity)
    {
        return JsonSerializer.Serialize(entity).GetHashCode();
    }

    private static T GetSnapshot(T entity)
    {
        return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(entity)) ?? throw new NullReferenceException();
    }
}