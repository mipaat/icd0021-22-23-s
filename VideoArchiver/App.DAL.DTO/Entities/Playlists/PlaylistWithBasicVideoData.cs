using App.DAL.DTO.Base;
using App.Common;

namespace App.DAL.DTO.Entities.Playlists;

public class PlaylistWithBasicVideoData : BaseArchiveEntity
{
    public LangString? Title { get; set; }
    public ICollection<BasicPlaylistVideo> PlaylistVideos { get; set; } = default!;
}