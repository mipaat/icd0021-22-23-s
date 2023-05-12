#pragma warning disable 1591
using App.BLL.DTO.Entities;
using AutoMapper;
using Base.DAL;

namespace Public.DTO.Mappers;

public class GameMapper : BaseMapper<Game, Public.DTO.v1.Domain.GameWithId>
{
    public GameMapper(IMapper mapper) : base(mapper)
    {
    }

    public Public.DTO.v1.Domain.GameWithoutId MapWithoutId(Game game)
    {
        return Mapper.Map<Public.DTO.v1.Domain.GameWithoutId>(game);
    }

    public Game MapWithoutId(Public.DTO.v1.Domain.GameWithoutId game)
    {
        return Mapper.Map<Game>(game);
    }
}

public static class GameMapperExtensions
{
    public static AutoMapperConfig AddGameMap(this AutoMapperConfig config)
    {
        config.CreateMap<Game, Public.DTO.v1.Domain.GameWithoutId>();
        config.CreateMap<Public.DTO.v1.Domain.GameWithoutId, Game>()
            .AfterMap((_, domainGame) => domainGame.Id = new Guid());
        config.CreateMap<Game, Public.DTO.v1.Domain.GameWithId>()
            .ReverseMap();
        return config;
    }
}