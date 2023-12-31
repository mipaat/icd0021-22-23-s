using System.ComponentModel.DataAnnotations;

namespace App.Common.Enums;

public enum EEntityType
{
    [Display(
        Name = nameof(Video),
        ResourceType = typeof(Resources.App.Common.Enums.EEntityType)
    )]
    Video,
    [Display(
        Name = nameof(Author),
        ResourceType = typeof(Resources.App.Common.Enums.EEntityType)
    )]
    Author,
    [Display(
        Name = nameof(Playlist),
        ResourceType = typeof(Resources.App.Common.Enums.EEntityType)
    )]
    Playlist,
}