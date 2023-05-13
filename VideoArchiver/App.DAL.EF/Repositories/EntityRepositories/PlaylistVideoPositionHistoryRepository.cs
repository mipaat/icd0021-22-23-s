using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace DAL.Repositories.EntityRepositories;

public class PlaylistVideoPositionHistoryRepository :
    BaseAppEntityRepository<App.Domain.PlaylistVideoPositionHistory, PlaylistVideoPositionHistory>, IPlaylistVideoPositionHistoryRepository
{
    public PlaylistVideoPositionHistoryRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}