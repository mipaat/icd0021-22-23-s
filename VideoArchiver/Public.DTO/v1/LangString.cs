namespace Public.DTO.v1;

/// <summary>
/// Dictionary of localized/translated string values, with culture identifiers for keys.
/// __UNKNOWN__ key may be used for a generic/unknown/unset culture.
/// </summary>
public class LangString : Dictionary<string, string>
{
    /// <summary>
    /// Construct a new LangString.
    /// </summary>
    public LangString()
    {
    }

    /// <summary>
    /// Construct a new LangString from an existing dictionary.
    /// </summary>
    /// <param name="dictionary">The dictionary to construct a new LangString from.</param>
    public LangString(IDictionary<string, string> dictionary) : base(dictionary)
    {
    }
}