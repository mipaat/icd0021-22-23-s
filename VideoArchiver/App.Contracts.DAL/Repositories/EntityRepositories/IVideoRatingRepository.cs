using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IVideoRatingRepository : IBaseEntityRepository<VideoRating, App.DAL.DTO.Entities.VideoRating>
{
}