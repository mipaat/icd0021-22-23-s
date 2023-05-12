using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace DAL.Repositories.EntityRepositories;

public class PlaylistVideoRepository : BaseAppEntityRepository<App.Domain.PlaylistVideo, PlaylistVideo>,
    IPlaylistVideoRepository
{
    public PlaylistVideoRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}