using App.Domain;
using App.Domain.Enums;
using Contracts.DAL;
using Domain;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IVideoAuthorRepository : IBaseEntityRepository<VideoAuthor>
{
    public Task SetVideoAuthor(Video video, Author author, EAuthorRole authorRole = EAuthorRole.Publisher);

    public Task<ICollection<VideoAuthor>> GetAllByVideoAndAuthor(Video video, Author author,
        EAuthorRole? authorRole = null);
    public Task<ICollection<VideoAuthor>> GetAllByVideoAndAuthor(Guid videoId, Guid authorId,
        EAuthorRole? authorRole = null);
}