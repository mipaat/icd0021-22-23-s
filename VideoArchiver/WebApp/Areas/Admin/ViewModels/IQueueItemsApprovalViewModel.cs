using App.BLL.DTO.Entities;

namespace WebApp.Areas.Admin.ViewModels;

public interface IQueueItemsApprovalViewModel
{
    public ICollection<QueueItemForApproval> QueueItems { get; set; }
    
    public Guid Id { get; set; }
}