using App.BLL.DTO.Entities;

namespace WebApp.Areas.Admin.ViewModels;

public class DashboardViewModel : IQueueItemsApprovalViewModel
{
    public ICollection<QueueItemForApproval> QueueItems { get; set; } = default!;
    public Guid Id { get; set; }
    public bool Approve { get; set; }
    public bool Delete { get; set; }
    
    
}