using System.Text.RegularExpressions;

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

    public static void EnsureDirectoryExists(string directory)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    public static string MakeRelativeFilePath(string filePath, string? directory = null)
    {
        var fileUri = new Uri(filePath);
        directory ??= Directory.GetCurrentDirectory();
        var referenceUri = new Uri(directory);
        var relativePath = Uri.UnescapeDataString(referenceUri.MakeRelativeUri(fileUri).ToString());

        var directoryName = Path.GetFileName(directory);
        if (relativePath.StartsWith(directoryName))
        {
            relativePath = relativePath[directoryName.Length..];
            while (relativePath.StartsWith('/'))
            {
                relativePath = relativePath[1..];
            }
        }

        return relativePath.Replace('/', Path.DirectorySeparatorChar);
    }

    public static string PostgresRegexEscapeString(this string value)
    {
        return Regex.Escape(value).Replace("\\", "\\\\").Replace("\"", "\\\"");
    }
}