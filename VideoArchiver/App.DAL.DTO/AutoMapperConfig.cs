using AutoMapper;

namespace App.DAL.DTO;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<NotMapped.Caption, App.Domain.NotMapped.Caption>().ReverseMap();
        // CreateMap<NotMapped.CaptionsDictionary, App.Domain.NotMapped.CaptionsDictionary>().ReverseMap();
        CreateMap<NotMapped.ImageFile, App.Domain.NotMapped.ImageFile>().ReverseMap();
        // CreateMap<NotMapped.ImageFileList, App.Domain.NotMapped.ImageFileList>().ReverseMap();
        // CreateMap<NotMapped.LangString, App.Domain.NotMapped.LangString>().ReverseMap();
        // CreateMap<NotMapped.VideoFile, App.Domain.NotMapped.VideoFile>().ReverseMap();
        
        CreateMap<Enums.EAuthorRole, App.Domain.Enums.EAuthorRole>().ReverseMap();
        CreateMap<Enums.EPrivacyStatus, App.Domain.Enums.EPrivacyStatus>().ReverseMap();
        CreateMap<Enums.Platform, App.Domain.Enums.Platform>()
            .ConstructUsing(p => p.Name)
            .ReverseMap()
            .ConstructUsing(p => p.Name);

        CreateMap<Entities.Identity.User, Domain.Identity.User>().ReverseMap();
        CreateMap<Domain.Identity.User, Domain.Identity.User>().ReverseMap();
        CreateMap<Entities.Identity.RefreshToken, Domain.Identity.RefreshToken>().ReverseMap();

        CreateMap<Entities.ApiQuotaUsage, Domain.ApiQuotaUsage>().ReverseMap();
        CreateMap<Entities.Author, Domain.Author>().ReverseMap();
        CreateMap<Entities.AuthorCategory, Domain.AuthorCategory>().ReverseMap();
        CreateMap<Entities.AuthorHistory, Domain.AuthorHistory>().ReverseMap();
        CreateMap<Entities.AuthorPubSub, Domain.AuthorPubSub>().ReverseMap();
        CreateMap<Entities.AuthorRating, Domain.AuthorRating>().ReverseMap();
        CreateMap<Entities.AuthorSubscription, Domain.AuthorSubscription>().ReverseMap();
        CreateMap<Entities.Category, Domain.Category>().ReverseMap();
        CreateMap<Entities.Comment, Domain.Comment>().ReverseMap();
        CreateMap<Entities.CommentHistory, Domain.CommentHistory>().ReverseMap();
        CreateMap<Entities.CommentReplyNotification, Domain.CommentReplyNotification>().ReverseMap();
        CreateMap<Entities.ExternalUserToken, Domain.ExternalUserToken>().ReverseMap();
        CreateMap<Entities.Game, Domain.Game>().ReverseMap();
        CreateMap<Entities.Playlist, Domain.Playlist>().ReverseMap();
        CreateMap<Entities.PlaylistAuthor, Domain.PlaylistAuthor>().ReverseMap();
        CreateMap<Entities.PlaylistCategory, Domain.PlaylistCategory>().ReverseMap();
        CreateMap<Entities.PlaylistHistory, Domain.PlaylistHistory>().ReverseMap();
        CreateMap<Entities.PlaylistRating, Domain.PlaylistRating>().ReverseMap();
        CreateMap<Entities.PlaylistSubscription, Domain.PlaylistSubscription>().ReverseMap();
        CreateMap<Entities.PlaylistVideo, Domain.PlaylistVideo>().ReverseMap();
        CreateMap<Entities.PlaylistVideoPositionHistory, Domain.PlaylistVideoPositionHistory>().ReverseMap();
        CreateMap<Entities.QueueItem, Domain.QueueItem>().ReverseMap();
        CreateMap<Entities.StatusChangeEvent, Domain.StatusChangeEvent>().ReverseMap();
        CreateMap<Entities.StatusChangeNotification, Domain.StatusChangeNotification>().ReverseMap();
        CreateMap<Entities.Video, Domain.Video>().ReverseMap();
        CreateMap<Entities.VideoAuthor, Domain.VideoAuthor>().ReverseMap();
        CreateMap<Entities.VideoCategory, Domain.VideoCategory>().ReverseMap();
        CreateMap<Entities.VideoGame, Domain.VideoGame>().ReverseMap();
        CreateMap<Entities.VideoHistory, Domain.VideoHistory>().ReverseMap();
        CreateMap<Entities.VideoRating, Domain.VideoRating>().ReverseMap();
        CreateMap<Entities.VideoUploadNotification, Domain.VideoUploadNotification>().ReverseMap();
    }
}