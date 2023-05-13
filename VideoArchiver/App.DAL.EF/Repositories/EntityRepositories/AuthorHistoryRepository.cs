using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class AuthorHistoryRepository : BaseAppEntityRepository<App.Domain.AuthorHistory, AuthorHistory>,
    IAuthorHistoryRepository
{
    public AuthorHistoryRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}