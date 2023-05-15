using App.Common.Enums;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IVideoAuthorRepository : IBaseEntityRepository<App.Domain.VideoAuthor, VideoAuthor>
{
    public Task SetVideoAuthor(Video video, Author author,
        EAuthorRole authorRole = EAuthorRole.Publisher);

    public Task<ICollection<VideoAuthor>> GetAllByVideoAndAuthor(Video video, Author author,
        EAuthorRole? authorRole = null);

    public Task<ICollection<VideoAuthor>> GetAllByVideoAndAuthor(Guid videoId, Guid authorId,
        EAuthorRole? authorRole = null);
}