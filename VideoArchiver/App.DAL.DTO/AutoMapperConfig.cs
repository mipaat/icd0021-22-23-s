using App.DAL.DTO.Entities;
using App.DAL.DTO.Entities.Identity;
using App.DAL.DTO.Entities.Playlists;
using AutoMapper;

namespace App.DAL.DTO;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<User, Domain.Identity.User>().ReverseMap();
        CreateMap<Domain.Identity.User, Domain.Identity.User>().ReverseMap();
        CreateMap<Domain.Identity.Role, Role>();
        CreateMap<Domain.Identity.User, UserWithRoles>()
            .ForMember(u => u.Roles, o =>
                o.MapFrom(u => u!.UserRoles!.Select(ur => ur.Role)))
            .ReverseMap();
        CreateMap<RefreshToken, Domain.Identity.RefreshToken>().ReverseMap();

        CreateMap<ApiQuotaUsage, Domain.ApiQuotaUsage>().ReverseMap();

        CreateMap<Author, Domain.Author>().ReverseMap();
        CreateMap<Domain.Author, AuthorBasic>().ReverseMap();
        CreateMap<AuthorCategory, Domain.AuthorCategory>().ReverseMap();
        CreateMap<Domain.AuthorCategory, AuthorCategoryOnlyIds>().ReverseMap();
        CreateMap<AuthorHistory, Domain.AuthorHistory>().ReverseMap();
        CreateMap<AuthorRating, Domain.AuthorRating>().ReverseMap();
        CreateMap<Author, AuthorBasic>();

        CreateMap<CategoryWithCreator, Domain.Category>()
            .ReverseMap().ForMember(c => c.Creator, o => o.MapFrom(c => c.Creator));
        CreateMap<Domain.Category, CategoryWithCreatorAndVideoAssignments>()
            .ReverseMap().ForMember(c => c.VideoCategories, o => o.Ignore());

        CreateMap<Comment, Domain.Comment>().ReverseMap();
        CreateMap<CommentHistory, Domain.CommentHistory>().ReverseMap();
        CreateMap<Domain.Comment, CommentRoot>();
        CreateMap<Domain.Comment, CommentChild>();

        CreateMap<PlaylistAuthor, Domain.PlaylistAuthor>().ReverseMap();
        CreateMap<PlaylistCategory, Domain.PlaylistCategory>().ReverseMap();
        CreateMap<Domain.PlaylistCategory, PlaylistCategoryOnlyIds>().ReverseMap();
        CreateMap<PlaylistHistory, Domain.PlaylistHistory>().ReverseMap();
        CreateMap<PlaylistRating, Domain.PlaylistRating>().ReverseMap();
        CreateMap<PlaylistSubscription, Domain.PlaylistSubscription>().ReverseMap();
        CreateMap<PlaylistVideoPositionHistory, Domain.PlaylistVideoPositionHistory>()
            .ForMember(e => e.PlaylistVideoId,
                expression => expression
                    .MapFrom(s => s.PlaylistVideo.Id))
            .ReverseMap();
        CreateMap<QueueItem, Domain.QueueItem>().ReverseMap();
        CreateMap<StatusChangeEvent, Domain.StatusChangeEvent>().ReverseMap();
        CreateMap<StatusChangeNotification, Domain.StatusChangeNotification>().ReverseMap();
        CreateMap<VideoAuthor, Domain.VideoAuthor>().ReverseMap();
        CreateMap<VideoCategory, Domain.VideoCategory>().ReverseMap();
        CreateMap<Domain.VideoCategory, VideoCategoryOnlyIds>().ReverseMap();
        CreateMap<VideoHistory, Domain.VideoHistory>().ReverseMap();
        CreateMap<VideoRating, Domain.VideoRating>().ReverseMap();

        CreateMap<Playlist, App.Domain.Playlist>().ReverseMap();
        CreateMap<App.Domain.Playlist, PlaylistWithBasicVideoData>().ReverseMap();

        CreateMap<PlaylistVideo, Domain.PlaylistVideo>().ReverseMap();
        CreateMap<App.Domain.PlaylistVideo, BasicPlaylistVideo>().ReverseMap();

        CreateMap<Video, App.Domain.Video>().ReverseMap();
        CreateMap<VideoWithComments, App.Domain.Video>().ForMember(v => v.Comments, opts => opts.Ignore());
        CreateMap<App.Domain.Video, VideoWithComments>()
            .ForMember(v => v.Comments,
                o => o.MapFrom(v => v.Comments));
        CreateMap<App.Domain.Video, VideoWithBasicAuthors>()
            .ForMember(v => v.Authors,
                o => o.MapFrom(v => v.VideoAuthors!.Select(va => va.Author)))
            .ForMember(v => v.ArchivedCommentCount,
                o => o.MapFrom(v => v.Comments!.Count))
            .ForMember(v => v.ArchivedRootCommentCount,
                o => o.MapFrom(v => v.Comments!.Count(c => c.ConversationRootId == null)));
        CreateMap<VideoWithBasicAuthors, App.Domain.Video>();
        CreateMap<App.Domain.Video, VideoWithBasicAuthorsAndComments>()
            .ForMember(v => v.Authors,
                o => o.MapFrom(v => v.VideoAuthors!.Select(va => va.Author)))
            .ForMember(v => v.ArchivedCommentCount,
                o => o.MapFrom(v => v.Comments!.Count))
            .ForMember(v => v.ArchivedRootCommentCount,
                o => o.MapFrom(v => v.Comments!.Count(c => c.ConversationRootId == null)));
        CreateMap<App.Domain.Video, BasicVideoData>().ReverseMap();
        CreateMap<App.Domain.Video, BasicVideoWithBasicAuthors>()
            .ForMember(v => v.Authors,
                o => o.MapFrom(v => v.VideoAuthors!.Select(va => va.Author)));

        CreateMap<App.Domain.EntityAccessPermission, EntityAccessPermission>().ReverseMap();
    }
}