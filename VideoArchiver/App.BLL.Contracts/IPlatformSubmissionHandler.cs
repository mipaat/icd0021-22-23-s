using App.BLL.DTO.Entities;
using App.Common.Enums;
using App.DAL.DTO.Entities.Playlists;
using Author = App.DAL.DTO.Entities.Author;
using Video = App.DAL.DTO.Entities.Video;

namespace App.BLL.Contracts;

public interface IPlatformSubmissionHandler
{
    public bool IsPlatformUrl(string url);
    public Task<UrlSubmissionResult> SubmitUrl(string url, Guid submitterId, bool autoSubmit, bool submitPlaylist = false);
    public bool CanHandle(EPlatform platform, EEntityType entityType);
    public Task<Video> SubmitVideo(string idOnPlatform);
    public Task<Playlist> SubmitPlaylist(string idOnPlatform);
    public Task<Author> SubmitAuthor(string idOnPlatform);
}