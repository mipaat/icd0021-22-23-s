using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class PlaylistRatingRepository : BaseAppEntityRepository<App.Domain.PlaylistRating, PlaylistRating>,
    IPlaylistRatingRepository
{
    public PlaylistRatingRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}