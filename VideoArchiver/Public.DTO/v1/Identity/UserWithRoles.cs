namespace Public.DTO.v1.Identity;

/// <summary>
/// Information about a user in the archive, including their roles.
/// </summary>
public class UserWithRoles
{
    /// <summary>
    /// The unique ID of the user.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// A unique human-friendly string identifying the user.
    /// </summary>
    public string UserName { get; set; } = default!;
    /// <summary>
    /// A boolean indicating whether this user account is approved for usage or not.
    /// If false, this account can't be logged into.
    /// </summary>
    public bool IsApproved { get; set; }
    /// <summary>
    /// The roles that this user is in.
    /// </summary>
    public ICollection<Role> Roles { get; set; } = default!;
}