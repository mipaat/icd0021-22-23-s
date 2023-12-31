using System.ComponentModel.DataAnnotations.Schema;

namespace App.Common;

[NotMapped]
public class LangString : Dictionary<string, string>
{
    private const string DefaultCulture = "en";
    public const string UnknownCulture = "__UNKNOWN__";

    public new string this[string key]
    {
        get => base[key];
        set => base[key] = value;
    }

    public LangString(string value) : this(value, Thread.CurrentThread.CurrentUICulture.Name)
    {
    }

    public LangString(Dictionary<string, string> source) : base(source)
    {
    }

    public LangString()
    {
    }

    public LangString(string value, string culture)
    {
        this[culture] = value;
    }

    public void SetTranslation(string value)
    {
        this[Thread.CurrentThread.CurrentUICulture.Name] = value;
    }

    public string? Translate(string? culture = null)
    {
        if (Count == 0) return null;

        culture = culture?.Trim() ?? Thread.CurrentThread.CurrentUICulture.Name;

        /*
         in database - en, en-GB
         in query - en, en-GB, en-US
         */

        // do we have exact match - en-GB == en-GB
        if (ContainsKey(culture))
        {
            return this[culture];
        }

        // do we have match without the region - en-US.StartsWith(en)
        var key = Keys.FirstOrDefault(t => culture.StartsWith(t));
        if (key != null)
        {
            return this[key];
        }

        // try to find the default culture
        key = Keys.FirstOrDefault(_ => culture.StartsWith(DefaultCulture));
        if (key != null)
        {
            return this[key];
        }

        // try to use unknown culture
        if (ContainsKey(UnknownCulture))
        {
            return this[UnknownCulture];
        }

        // just return the first in list or null
        return Values.FirstOrDefault();
    }

    public override string ToString()
    {
        return Translate() ?? "????";
    }

    public static implicit operator string(LangString? l) => l?.ToString() ?? "null";
    public static implicit operator LangString(string s) => new(s);

    public static bool operator ==(LangString? l, LangString? r)
    {
        return l?.Equals(r) ?? r is null;
    }

    public static bool operator !=(LangString? l, LangString? r)
    {
        return !(l == r);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not LangString other) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (Count != other.Count) return false;
        return Keys.All(other.ContainsKey) &&
               this.All(kvp => other.GetValueOrDefault(kvp.Key) == kvp.Value);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            foreach (var kvp in this.OrderBy(kvp => kvp.Key))
            {
                hash = hash * 23 + kvp.Key.GetHashCode();
                hash = hash * 23 + kvp.Value.GetHashCode();
            }
            return hash;
        }
    }

    public bool IsUnspecified => Count == 1 && ContainsKey(UnknownCulture);

    public bool IsSpecifiedVersionOf(LangString other)
    {
        if (!other.IsUnspecified) return false;
        var otherValue = other[UnknownCulture];
        return Values.Any(v => v == otherValue);
    }

    public bool IsUnspecifiedVersionOf(LangString? other)
    {
        return other != null && IsUnspecified && other.Values.Any(v => v == this[UnknownCulture]);
    }
}