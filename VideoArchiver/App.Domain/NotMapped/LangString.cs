using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.NotMapped;

[NotMapped]
public class LangString : Dictionary<string, string>
{
    private const string DefaultCulture = "en";

    public new string this[string key]
    {
        get => base[key];
        set => base[key] = value;
    }

    public LangString(string value) : this(value, Thread.CurrentThread.CurrentUICulture.Name)
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
        key = Keys.FirstOrDefault(t => culture.StartsWith(DefaultCulture));
        if (key != null)
        {
            return this[key];
        }

        // just return the first in list or null
        return null;
    }

    public override string ToString()
    {
        return Translate() ?? "????";
    }

    public static implicit operator string(LangString? l) => l?.ToString() ?? "null";
    public static implicit operator LangString(string s) => new(s);
}