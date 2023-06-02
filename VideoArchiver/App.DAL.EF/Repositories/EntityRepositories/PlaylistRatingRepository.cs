using App.DAL.Contracts;
using App.DAL.Contracts.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using App.DAL.DTO.Entities.Playlists;
using AutoMapper;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class PlaylistRatingRepository : BaseAppEntityRepository<App.Domain.PlaylistRating, PlaylistRating>,
    IPlaylistRatingRepository
{
    public PlaylistRatingRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext, mapper, uow)
    {
    }
}