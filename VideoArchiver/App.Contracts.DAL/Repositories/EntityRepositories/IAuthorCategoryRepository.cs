using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IAuthorCategoryRepository : IBaseEntityRepository<AuthorCategory, App.DAL.DTO.Entities.AuthorCategory>
{
}