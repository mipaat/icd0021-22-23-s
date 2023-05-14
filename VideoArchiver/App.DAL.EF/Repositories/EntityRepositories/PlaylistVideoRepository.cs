using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class PlaylistVideoRepository : BaseAppEntityRepository<App.Domain.PlaylistVideo, PlaylistVideo>,
    IPlaylistVideoRepository
{
    public PlaylistVideoRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext,
        mapper, uow)
    {
    }

    protected override Domain.PlaylistVideo AfterMap(PlaylistVideo entity, Domain.PlaylistVideo mapped)
    {
        if (entity.Video != null)
        {
            var video = Uow.Videos.GetTrackedEntity(entity.Video);
            if (video != null)
            {
                mapped.Video = Uow.Videos.Map(entity.Video, video);
            }
        }

        if (entity.Playlist != null)
        {
            var playlist = Uow.Playlists.GetTrackedEntity(entity.Playlist);
            if (playlist != null)
            {
                mapped.Playlist = Uow.Playlists.Map(entity.Playlist, playlist);
            }
        }

        return mapped;
    }
}