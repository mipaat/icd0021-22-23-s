using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities.Playlists;
using AutoMapper;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class PlaylistVideoRepository :
    BaseAppEntityRepository<App.Domain.PlaylistVideo, PlaylistVideo>, IPlaylistVideoRepository
{
    public PlaylistVideoRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) :
        base(dbContext, mapper, uow)
    {
    }

    private App.Domain.PlaylistVideo AfterMap(BasicPlaylistVideo playlistVideo, App.Domain.PlaylistVideo mapped)
    {
        var trackedVideo = Uow.Videos.GetTrackedEntity(playlistVideo.Video.Id) ??
                           throw new ApplicationException(
                               $"Expected video '{playlistVideo.Video.Id}' to be tracked when updating {typeof(BasicPlaylistVideo)}");
        mapped.Video = Uow.Videos.Map(playlistVideo.Video, trackedVideo);
        return mapped;
    }

    private App.Domain.PlaylistVideo Map(BasicPlaylistVideo playlistVideo)
    {
        return AfterMap(playlistVideo, Mapper.Map<BasicPlaylistVideo, App.Domain.PlaylistVideo>(playlistVideo));
    }

    private App.Domain.PlaylistVideo Map(BasicPlaylistVideo playlistVideo, App.Domain.PlaylistVideo mapped)
    {
        return AfterMap(playlistVideo, Mapper.Map(playlistVideo, mapped));
    }

    public void Add(BasicPlaylistVideo playlistVideo)
    {
        AddBase(playlistVideo, Map, Map);
    }

    public void Update(BasicPlaylistVideo playlistVideo)
    {
        UpdateBase(playlistVideo, Map, Map);
    }
}