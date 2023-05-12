using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface ICategoryRepository : IBaseEntityRepository<Category, App.DAL.DTO.Entities.Category>
{
}