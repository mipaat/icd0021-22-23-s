using App.Domain;
using App.Domain.Enums;
using Contracts.DAL;
using Domain;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IPlaylistAuthorRepository : IBaseEntityRepository<PlaylistAuthor>
{
    public Task SetPlaylistAuthor(Playlist playlist, Author author, EAuthorRole authorRole = EAuthorRole.Publisher);

    public Task<ICollection<PlaylistAuthor>> GetAllByPlaylistAndAuthor(Playlist playlist, Author author,
        EAuthorRole? authorRole = null);
    public Task<ICollection<PlaylistAuthor>> GetAllByPlaylistAndAuthor(Guid playlistId, Guid authorId,
        EAuthorRole? authorRole = null);
}