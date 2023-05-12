using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace DAL.Repositories.EntityRepositories;

public class AuthorHistoryRepository : BaseAppEntityRepository<App.Domain.AuthorHistory, AuthorHistory>,
    IAuthorHistoryRepository
{
    public AuthorHistoryRepository(AbstractAppDbContext dbContext, ITrackingAutoMapperWrapper mapper) : base(dbContext, mapper)
    {
    }
}