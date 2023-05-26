namespace Public.DTO.v1;

/// <summary>
/// Filtering conditions for listing user accounts.
/// </summary>
public class UserFilter
{
    /// <summary>
    /// If true, the results should include only user accounts that haven't been approved yet.
    /// </summary>
    public bool IncludeOnlyNotApproved { get; set; }
    /// <summary>
    /// If not null, results should include only user accounts with usernames containing this string.
    /// </summary>
    public string? NameQuery { get; set; }
}