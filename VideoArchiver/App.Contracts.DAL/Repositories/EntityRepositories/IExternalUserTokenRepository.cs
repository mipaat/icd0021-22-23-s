using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IExternalUserTokenRepository : IBaseEntityRepository<ExternalUserToken, App.DAL.DTO.Entities.ExternalUserToken>
{
}