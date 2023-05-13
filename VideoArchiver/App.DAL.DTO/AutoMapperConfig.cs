using AutoMapper;

namespace App.DAL.DTO;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<App.DAL.DTO.NotMapped.Caption, App.Domain.NotMapped.Caption>().ReverseMap();
        // CreateMap<NotMapped.CaptionsDictionary, App.Domain.NotMapped.CaptionsDictionary>().ReverseMap();
        CreateMap<App.DAL.DTO.NotMapped.ImageFile, App.Domain.NotMapped.ImageFile>().ReverseMap();
        // CreateMap<NotMapped.ImageFileList, App.Domain.NotMapped.ImageFileList>().ReverseMap();
        // CreateMap<NotMapped.LangString, App.Domain.NotMapped.LangString>().ReverseMap();
        CreateMap<NotMapped.VideoFile, App.Domain.NotMapped.VideoFile>().ReverseMap();

        CreateMap<App.DAL.DTO.Enums.EAuthorRole, App.Domain.Enums.EAuthorRole>().ReverseMap();
        CreateMap<App.DAL.DTO.Enums.EPrivacyStatus, App.Domain.Enums.EPrivacyStatus>().ReverseMap();
        CreateMap<App.DAL.DTO.Enums.Platform, App.Domain.Enums.Platform>()
            .ConstructUsing(p => p.Name)
            .ReverseMap()
            .ConstructUsing(p => p.Name);

        CreateMap<App.DAL.DTO.Entities.Identity.User, App.Domain.Identity.User>().ReverseMap();
        CreateMap<App.Domain.Identity.User, App.Domain.Identity.User>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.Identity.RefreshToken, App.Domain.Identity.RefreshToken>().ReverseMap();

        CreateMap<App.DAL.DTO.Entities.ApiQuotaUsage, App.Domain.ApiQuotaUsage>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.Author, App.Domain.Author>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.AuthorCategory, App.Domain.AuthorCategory>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.AuthorHistory, App.Domain.AuthorHistory>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.AuthorPubSub, App.Domain.AuthorPubSub>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.AuthorRating, App.Domain.AuthorRating>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.AuthorSubscription, App.Domain.AuthorSubscription>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.Category, App.Domain.Category>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.Comment, App.Domain.Comment>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.CommentHistory, App.Domain.CommentHistory>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.CommentReplyNotification, App.Domain.CommentReplyNotification>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.ExternalUserToken, App.Domain.ExternalUserToken>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.Game, App.Domain.Game>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.Playlist, App.Domain.Playlist>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.PlaylistAuthor, App.Domain.PlaylistAuthor>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.PlaylistCategory, App.Domain.PlaylistCategory>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.PlaylistHistory, App.Domain.PlaylistHistory>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.PlaylistRating, App.Domain.PlaylistRating>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.PlaylistSubscription, App.Domain.PlaylistSubscription>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.PlaylistVideo, App.Domain.PlaylistVideo>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.PlaylistVideoPositionHistory, App.Domain.PlaylistVideoPositionHistory>()
            .ReverseMap();
        CreateMap<App.DAL.DTO.Entities.QueueItem, App.Domain.QueueItem>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.StatusChangeEvent, App.Domain.StatusChangeEvent>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.StatusChangeNotification, App.Domain.StatusChangeNotification>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.Video, App.Domain.Video>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.VideoAuthor, App.Domain.VideoAuthor>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.VideoCategory, App.Domain.VideoCategory>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.VideoGame, App.Domain.VideoGame>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.VideoHistory, App.Domain.VideoHistory>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.VideoRating, App.Domain.VideoRating>().ReverseMap();
        CreateMap<App.DAL.DTO.Entities.VideoUploadNotification, App.Domain.VideoUploadNotification>().ReverseMap();
    }
}