using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace DAL.Repositories.EntityRepositories;

public class PlaylistCategoryRepository : BaseAppEntityRepository<App.Domain.PlaylistCategory, PlaylistCategory>,
    IPlaylistCategoryRepository
{
    public PlaylistCategoryRepository(AbstractAppDbContext dbContext, ITrackingAutoMapperWrapper mapper) : base(dbContext, mapper)
    {
    }
}