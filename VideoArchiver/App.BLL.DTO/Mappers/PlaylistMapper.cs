using AutoMapper;
using Base.DAL;

namespace App.BLL.DTO.Mappers;

public class PlaylistMapper : BaseMapperUnidirectional<App.DAL.DTO.Entities.Playlist, Entities.Playlist>
{
    public PlaylistMapper(IMapper mapper) : base(mapper)
    {
    }
}

public static class PlaylistMapperExtensions
{
    public static AutoMapperConfig AddPlaylistMap(this AutoMapperConfig config)
    {
        config.CreateMap<App.DAL.DTO.Entities.Playlist, Entities.Playlist>();
        return config;
    }
}