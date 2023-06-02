using App.DAL.DTO.Entities;
using App.DAL.DTO.Entities.Playlists;

namespace App.BLL.Contracts.Services;

public interface IEntityUpdateService
{
    public Task UpdateComment(Comment comment, Comment newCommentData);

    public Task UpdateVideo(Video video, Video newVideoData);

    public Task UpdatePlaylist(Playlist playlist, Playlist newPlaylistData);
}