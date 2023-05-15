using App.DAL.DTO.Base;
using App.DAL.DTO.Entities.Identity;
using App.Common;

namespace App.DAL.DTO.Entities;

public class Author : BaseArchiveEntity
{
    public string? UserName { get; set; }
    public string? DisplayName { get; set; }
    public LangString? Bio { get; set; }

    public long? SubscriberCount { get; set; }

    public ImageFileList? ProfileImages { get; set; }
    public ImageFileList? Banners { get; set; }
    public ImageFileList? Thumbnails { get; set; }

    public Guid? UserId { get; set; }
    public User? User { get; set; }
}