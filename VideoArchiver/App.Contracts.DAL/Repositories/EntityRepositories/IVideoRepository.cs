using App.Common;
using App.Common.Enums;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IVideoRepository : IBaseEntityRepository<App.Domain.Video, Video>
{
    public Task<Video?> GetByIdOnPlatformAsync(string idOnPlatform, EPlatform platform);
    public Task<ICollection<string>> GetAllIdsOnPlatformWithUnarchivedComments(EPlatform platform);

    public Task<VideoWithComments?> GetByIdOnPlatformWithCommentsAsync(string idOnPlatform,
        EPlatform platform);

    public Task<ICollection<Video>> GetAllNotOfficiallyFetched(EPlatform platform, int? limit = null);

    public Task<ICollection<Video>> GetAllBeforeOfficialApiFetch(EPlatform platform, DateTime cutoff,
        int? limit = null);

    public App.Domain.Video Map(BasicVideoData video, App.Domain.Video mapped);
    public Task<ICollection<string>> GetAllIdsOnPlatformNotDownloaded(EPlatform platform);

    public Task<VideoWithBasicAuthors> GetByIdWithBasicAuthorsAsync(Guid id);
    public Task<VideoWithBasicAuthorsAndComments> GetByIdWithBasicAuthorsAndCommentsAsync(Guid id);
    public Task<ICollection<VideoFile>?> GetVideoFilesAsync(Guid videoId);
}