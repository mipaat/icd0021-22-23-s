using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class AuthorRepository : BaseAppEntityRepository<App.Domain.Author, Author>, IAuthorRepository
{
    public AuthorRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<Author?> GetByIdOnPlatformAsync(string idOnPlatform, App.DAL.DTO.Enums.Platform platform)
    {
        return Mapper.Map(await Entities
            .Where(a => a.IdOnPlatform == idOnPlatform && a.Platform == platform)
            .SingleOrDefaultAsync());
    }
    
    public async Task<ICollection<Author>> GetAllUserSubAuthors(Guid userId)
    {
        return await GetAllAsync(a => a.UserId == userId);
    }

    public async Task<bool> IsUserSubAuthor(Guid authorId, Guid userId)
    {
        return await Entities.Where(a => a.Id == authorId && a.UserId == userId).AnyAsync();
    }
}