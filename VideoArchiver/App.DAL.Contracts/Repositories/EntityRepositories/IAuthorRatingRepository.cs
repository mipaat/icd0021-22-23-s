using App.Domain;
using Contracts.DAL;

namespace App.DAL.Contracts.Repositories.EntityRepositories;

public interface IAuthorRatingRepository : IBaseEntityRepository<AuthorRating, App.DAL.DTO.Entities.AuthorRating>
{
}