namespace Utils;

public static class Utils
{
    public static T RaiseIfNull<T>(this T? value) =>
        value ?? throw new ArgumentNullException($"Argument of type {typeof(T)} was null!");

    public static bool LaterThan(DateTime? left, DateTime? right)
    {
        if (left == null && right == null) return false;
        if (right == null) return true;
        if (left == null) return false;
        return left > right;
    }

    public static bool EqualsOrNull<T>(T? value1, T? value2)
    {
        if (value1 == null || value2 == null) return true;
        return value1.Equals(value2);
    }
}