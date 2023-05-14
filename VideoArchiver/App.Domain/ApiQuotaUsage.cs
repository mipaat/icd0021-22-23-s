using Domain.Base;

namespace App.Domain;

public class ApiQuotaUsage : AbstractIdDatabaseEntity
{
    public string Identifier { get; set; } = default!;
    public DateTime UpdatedAt { get; set; }
    public int UsageAmount { get; set; }
}