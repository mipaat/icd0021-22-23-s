using App.Common.Enums;
using App.Domain;
using Contracts.DAL;
using Author = App.DAL.DTO.Entities.Author;
using Playlist = App.DAL.DTO.Entities.Playlists.Playlist;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IPlaylistAuthorRepository : IBaseEntityRepository<PlaylistAuthor, App.DAL.DTO.Entities.Playlists.PlaylistAuthor>
{
    public Task SetPlaylistAuthor(Playlist playlist, Author author, EAuthorRole authorRole = EAuthorRole.Publisher);

    public Task<ICollection<App.DAL.DTO.Entities.Playlists.PlaylistAuthor>> GetAllByPlaylistAndAuthor(Playlist playlist, Author author,
        EAuthorRole? authorRole = null);
    public Task<ICollection<App.DAL.DTO.Entities.Playlists.PlaylistAuthor>> GetAllByPlaylistAndAuthor(Guid playlistId, Guid authorId,
        EAuthorRole? authorRole = null);
}