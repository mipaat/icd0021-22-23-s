using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IAuthorRatingRepository : IBaseEntityRepository<AuthorRating, App.DAL.DTO.Entities.AuthorRating>
{
}