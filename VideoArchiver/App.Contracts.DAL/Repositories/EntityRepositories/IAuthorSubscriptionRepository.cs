using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IAuthorSubscriptionRepository : IBaseEntityRepository<AuthorSubscription, App.DAL.DTO.Entities.AuthorSubscription>
{
}