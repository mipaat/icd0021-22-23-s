using System.ComponentModel.DataAnnotations;
using App.Common;
using App.Common.Enums;
using Domain.Base;

namespace App.BLL.DTO.Entities;

public class BasicVideoWithAuthor : AbstractIdDatabaseEntity
{
    [Display(Name = nameof(Title), ResourceType = typeof(Resources.App.BLL.DTO.Entities.BasicVideoWithAuthor))]
    public LangString? Title { get; set; }
    public bool IsAvailable { get; set; }
    public EPrivacyStatus? PrivacyStatus { get; set; }
    public EPrivacyStatus InternalPrivacyStatus { get; set; }

    public ImageFileList? Thumbnails { get; set; }
    [Display(Name = nameof(Thumbnail), ResourceType = typeof(Resources.App.BLL.DTO.Entities.BasicVideoWithAuthor))]
    public ImageFile? Thumbnail { get; set; }
    
    [Display(Name = nameof(Duration), ResourceType = typeof(Resources.App.BLL.DTO.Entities.BasicVideoWithAuthor))]
    public TimeSpan? Duration { get; set; }

    public EPlatform Platform { get; set; }
    public string IdOnPlatform { get; set; } = default!;

    [Display(Name = nameof(Author), ResourceType = typeof(Resources.App.BLL.DTO.Entities.BasicVideoWithAuthor))]
    public Author Author { get; set; } = default!;
}