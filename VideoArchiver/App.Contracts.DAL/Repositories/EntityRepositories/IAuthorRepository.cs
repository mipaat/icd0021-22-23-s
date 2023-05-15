using App.Common.Enums;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IAuthorRepository : IBaseEntityRepository<App.Domain.Author, Author>
{
    public Task<Author?> GetByIdOnPlatformAsync(string idOnPlatform, EPlatform platform);

    public Task<ICollection<Author>> GetAllUserSubAuthors(Guid userId);

    public Task<bool> IsUserSubAuthor(Guid authorId, Guid userId);
}