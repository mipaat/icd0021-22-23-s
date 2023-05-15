using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using App.DAL.DTO.Entities.Playlists;
using AutoMapper;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class PlaylistCategoryRepository : BaseAppEntityRepository<App.Domain.PlaylistCategory, PlaylistCategory>,
    IPlaylistCategoryRepository
{
    public PlaylistCategoryRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext, mapper, uow)
    {
    }
}