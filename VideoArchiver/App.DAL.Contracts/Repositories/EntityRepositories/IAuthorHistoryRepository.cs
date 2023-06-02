using App.Domain;
using Contracts.DAL;

namespace App.DAL.Contracts.Repositories.EntityRepositories;

public interface IAuthorHistoryRepository : IBaseEntityRepository<AuthorHistory, App.DAL.DTO.Entities.AuthorHistory>
{
}