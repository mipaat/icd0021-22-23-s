using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities.Playlists;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class PlaylistCategoryRepository : BaseAppEntityRepository<App.Domain.PlaylistCategory, PlaylistCategory>,
    IPlaylistCategoryRepository
{
    public PlaylistCategoryRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext, mapper, uow)
    {
    }

    public async Task<ICollection<Guid>> GetAllCategoryIdsAsync(Guid playlistId, Guid? authorId)
    {
        return await Entities.Where(e => e.PlaylistId == playlistId && e.AssignedById == authorId)
            .Select(e => e.CategoryId)
            .ToListAsync();
    }
}