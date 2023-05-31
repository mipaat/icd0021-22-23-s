using System.ComponentModel.DataAnnotations;
using App.BLL.DTO.Entities.Identity;
using App.Common.Enums;
using Domain.Base;

namespace App.BLL.DTO.Entities;

public class QueueItemForApproval : AbstractIdDatabaseEntity
{
    public EPlatform Platform { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.BLL.DTO.Entities.QueueItemForApproval), Name = nameof(IdOnPlatform))]
    [MaxLength(64)] public string IdOnPlatform { get; set; } = default!;
    public EEntityType EntityType { get; set; }

    public bool Monitor { get; set; }
    public bool Download { get; set; }

    [Display(ResourceType = typeof(App.Resources.App.BLL.DTO.Entities.QueueItemForApproval), Name = nameof(AddedBy))]
    public User AddedBy { get; set; } = default!;
    [Display(ResourceType = typeof(App.Resources.App.BLL.DTO.Entities.QueueItemForApproval), Name = nameof(AddedAt))]
    public DateTime AddedAt { get; set; }

    public User? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public bool GrantAccess { get; set; }
}