using App.Common;
using App.Common.Enums;

namespace App.DAL.DTO.Entities;

public class AuthorBasic
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? DisplayName { get; set; }
    
    public EPlatform Platform { get; set; }
    public string IdOnPlatform { get; set; } = default!;

    public long? SubscriberCount { get; set; }
    public ImageFileList? ProfileImages { get; set; }
}