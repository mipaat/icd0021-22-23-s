using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace DAL.Repositories.EntityRepositories;

public class PlaylistCategoryRepository : BaseAppEntityRepository<App.Domain.PlaylistCategory, PlaylistCategory>,
    IPlaylistCategoryRepository
{
    public PlaylistCategoryRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}