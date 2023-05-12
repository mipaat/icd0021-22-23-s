using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IAuthorHistoryRepository : IBaseEntityRepository<AuthorHistory, App.DAL.DTO.Entities.AuthorHistory>
{
}