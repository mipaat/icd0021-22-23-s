using App.DAL.DTO.Entities.Playlists;
using AutoMapper;
using Base.Mapping;

namespace App.BLL.DTO.Mappers;

public class PlaylistMapper : BaseMapperUnidirectional<Playlist, Entities.Playlist>
{
    public PlaylistMapper(IMapper mapper) : base(mapper)
    {
    }
}

public static class PlaylistMapperExtensions
{
    public static AutoMapperConfig AddPlaylistMap(this AutoMapperConfig config)
    {
        config.CreateMap<Playlist, Entities.Playlist>();
        return config;
    }
}