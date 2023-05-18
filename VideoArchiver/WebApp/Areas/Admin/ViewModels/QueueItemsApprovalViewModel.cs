using App.BLL.DTO.Entities;

namespace WebApp.Areas.Admin.ViewModels;

public class QueueItemsApprovalViewModel
{
    public ICollection<QueueItemForApproval> QueueItems { get; set; } = default!;
}