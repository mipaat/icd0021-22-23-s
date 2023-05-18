using Domain.Base;

namespace App.DAL.DTO.Entities.Identity;

public class Role : AbstractIdDatabaseEntity
{
    public string Name { get; set; } = default!;
    public string? ConcurrencyStamp { get; set; }
}