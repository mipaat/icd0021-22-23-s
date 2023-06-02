using App.Domain;
using Contracts.DAL;

namespace App.DAL.Contracts.Repositories.EntityRepositories;

public interface IVideoRatingRepository : IBaseEntityRepository<VideoRating, App.DAL.DTO.Entities.VideoRating>
{
}