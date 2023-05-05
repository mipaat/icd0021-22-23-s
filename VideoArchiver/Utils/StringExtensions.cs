namespace Utils;

public static class StringExtensions
{
    public static string ToCapitalized(this string s)
    {
        if (s.Length == 0) return s;
        return char.ToUpper(s[0]) + s[1..];
    }
}