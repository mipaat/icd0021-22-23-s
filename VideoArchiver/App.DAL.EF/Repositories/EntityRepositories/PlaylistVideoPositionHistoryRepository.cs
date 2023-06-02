using App.DAL.Contracts;
using App.DAL.Contracts.Repositories.EntityRepositories;
using App.DAL.DTO.Entities.Playlists;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class PlaylistVideoPositionHistoryRepository :
    BaseAppEntityRepository<App.Domain.PlaylistVideoPositionHistory, PlaylistVideoPositionHistory>, IPlaylistVideoPositionHistoryRepository
{
    public PlaylistVideoPositionHistoryRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext, mapper, uow)
    {
    }

    protected override TQueryable IncludeDefaults<TQueryable>(TQueryable queryable)
    {
        queryable.Include(e => e.PlaylistVideo);
        return queryable;
    }

    protected override Domain.PlaylistVideoPositionHistory AfterMap(PlaylistVideoPositionHistory entity, Domain.PlaylistVideoPositionHistory mapped)
    {
        var trackedEntity = Uow.PlaylistVideos.GetTrackedEntity(entity.PlaylistVideo.Id);
        if (trackedEntity != null)
        {
            mapped.PlaylistVideo = Mapper.Map(entity.PlaylistVideo, trackedEntity);
        }

        return mapped;
    }
}