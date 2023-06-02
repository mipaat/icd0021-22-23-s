using App.DAL.DTO.Entities;
using App.DAL.DTO.Entities.Playlists;

namespace App.BLL.Contracts.Services;

public interface IImageService : IBaseService
{
    public Task UpdateProfileImages(Author author);
    public Task UpdateThumbnails(Video video);
    public Task UpdateThumbnails(Playlist playlist);
}