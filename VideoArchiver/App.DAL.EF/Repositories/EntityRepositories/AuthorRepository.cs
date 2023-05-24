using App.Common;
using App.Common.Enums;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class AuthorRepository : BaseAppEntityRepository<App.Domain.Author, Author>, IAuthorRepository
{
    public AuthorRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext, mapper, uow)
    {
    }

    public async Task<Author?> GetByIdOnPlatformAsync(string idOnPlatform, EPlatform platform)
    {
        return AttachIfNotAttached(await Entities
            .Where(a => a.IdOnPlatform == idOnPlatform && a.Platform == platform)
            .ProjectTo<Author?>(Mapper.ConfigurationProvider)
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

    public async Task<ICollection<AuthorBasic>> GetAllBasicByIdsOnPlatformAsync(IEnumerable<string> idsOnPlatform, EPlatform platform)
    {
        return AttachIfNotAttached<ICollection<AuthorBasic>, AuthorBasic>(
            await Entities
                .Where(e => e.Platform == platform && idsOnPlatform.Contains(e.IdOnPlatform))
                .ProjectTo<AuthorBasic>(Mapper.ConfigurationProvider)
                .ToListAsync());
    }

    public async Task<ImageFileList?> GetProfileImagesForAuthor(Guid authorId)
    {
        return await Entities.Where(e => e.Id == authorId).Select(e => e.ProfileImages).FirstOrDefaultAsync();
    }
}