using App.Common.Enums;

#pragma warning disable CS1591
namespace WebApp.ViewModels;

public class UrlSubmissionResultViewModel
{
    public Guid QueueItemId { get; set; }
    public EEntityType Type { get; set; }
    public Guid? EntityId { get; set; }
    public EPlatform Platform { get; set; }
    public string IdOnPlatform { get; set; } = default!;
    public bool AlreadyAdded { get; set; }
}