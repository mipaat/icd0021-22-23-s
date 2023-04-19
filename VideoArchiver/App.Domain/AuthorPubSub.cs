using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace App.Domain;

public class AuthorPubSub : AbstractIdDatabaseEntity
{
    public DateTime LeasedAt { get; set; }
    public TimeSpan LeaseDuration { get; set; }
    [MaxLength(512)] public string Secret { get; set; } = default!;
    public Author? Author { get; set; }
    public Guid AuthorId { get; set; }
}