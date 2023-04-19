using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Domain.Base;

namespace App.Domain;

public class ExternalUserToken : AbstractIdDatabaseEntity
{
    [MaxLength(4096)] public string AccessToken { get; set; } = default!;
    [MaxLength(4096)] public string RefreshToken { get; set; } = default!;
    public TimeSpan ExpiresIn { get; set; }
    public DateTime IssuedAt { get; set; }
    [MaxLength(2048)] public string? Scope { get; set; }
    [MaxLength(128)] public string? TokenType { get; set; }

    public User? User { get; set; }
    public Guid UserId { get; set; }
    public Author? Author { get; set; }
    public Guid AuthorId { get; set; }
}