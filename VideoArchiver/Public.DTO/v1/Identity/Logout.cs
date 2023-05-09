namespace Public.DTO.v1.Identity;

/// <summary>
/// Required data for logging out a user by deleting their refresh token.
/// </summary>
public class Logout
{
    /// <summary>
    /// The refresh token to delete.
    /// </summary>
    public string RefreshToken { get; set; } = default!;
}