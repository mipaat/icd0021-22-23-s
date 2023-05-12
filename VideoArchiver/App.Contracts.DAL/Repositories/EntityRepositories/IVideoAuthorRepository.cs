using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IVideoAuthorRepository : IBaseEntityRepository<App.Domain.VideoAuthor, VideoAuthor>
{
    public Task SetVideoAuthor(Video video, Author author,
        App.DAL.DTO.Enums.EAuthorRole authorRole = App.DAL.DTO.Enums.EAuthorRole.Publisher);

    public Task<ICollection<VideoAuthor>> GetAllByVideoAndAuthor(Video video, Author author,
        App.DAL.DTO.Enums.EAuthorRole? authorRole = null);

    public Task<ICollection<VideoAuthor>> GetAllByVideoAndAuthor(Guid videoId, Guid authorId,
        App.DAL.DTO.Enums.EAuthorRole? authorRole = null);
}