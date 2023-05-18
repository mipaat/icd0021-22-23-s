using Domain.Base;

namespace App.BLL.DTO.Entities.Identity;

public class Role : AbstractIdDatabaseEntity
{
    public string Name { get; set; } = default!;
}