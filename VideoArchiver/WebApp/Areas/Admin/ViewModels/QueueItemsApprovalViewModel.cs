using App.BLL.DTO.Entities;
#pragma warning disable CS1591

namespace WebApp.Areas.Admin.ViewModels;

public class QueueItemsApprovalViewModel
{
    public List<QueueItemForApproval> QueueItems { get; set; } = default!;
}