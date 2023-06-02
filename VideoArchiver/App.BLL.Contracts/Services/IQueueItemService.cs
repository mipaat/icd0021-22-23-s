using App.BLL.DTO.Entities;
using App.Common.Enums;
using App.DAL.DTO.Entities;
using App.DAL.DTO.Entities.Playlists;
using Author = App.DAL.DTO.Entities.Author;
using Video = App.DAL.DTO.Entities.Video;

namespace App.BLL.Contracts.Services;

public interface IQueueItemService : IBaseService
{
    public Task<QueueItem> Add(Video video, Guid submitterId, bool autoSubmit);
    public Task<QueueItem> Add(Playlist playlist, Guid submitterId, bool autoSubmit);
    public Task<QueueItem> Add(Author author, Guid submitterId, bool autoSubmit);

    public QueueItem Add(string idOnPlatform, EPlatform platform, EEntityType entityType, Guid submitterId,
        bool autoSubmit);

    public Task<List<QueueItemForApproval>> GetAllAwaitingApprovalAsync();
    public Task DeleteAsync(Guid id);
    public Task ApproveAsync(Guid id, Guid approvedById, bool grantAccess = true);
}