using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IAuthorPubSubRepository : IBaseEntityRepository<AuthorPubSub, App.DAL.DTO.Entities.AuthorPubSub>
{
}