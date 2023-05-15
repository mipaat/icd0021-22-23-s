using App.Common.Enums;
using Domain.Base;

namespace App.DAL.DTO.Entities;

public class BasicVideoData : IIdDatabaseEntity
{
    public Guid Id { get; set; }
    public string IdOnPlatform { get; set; } = default!;
    public EPlatform Platform { get; set; } = default!;
}