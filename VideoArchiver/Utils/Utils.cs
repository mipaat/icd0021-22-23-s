namespace Utils;

public static class Utils
{
    public static T RaiseIfNull<T>(this T? value) =>
        value ?? throw new ArgumentNullException($"Argument of type {typeof(T)} was null!");
}