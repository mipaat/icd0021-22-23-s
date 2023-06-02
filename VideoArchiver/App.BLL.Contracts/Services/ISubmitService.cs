using System.Security.Claims;
using App.BLL.DTO.Entities;
using App.DAL.DTO.Entities;

namespace App.BLL.Contracts.Services;

public interface ISubmitService : IBaseService
{
    public Task<UrlSubmissionResult> SubmitGenericUrlAsync(string url, ClaimsPrincipal user, bool submitPlaylist);
    public Task SubmitQueueItemAsync(QueueItem queueItem);
}