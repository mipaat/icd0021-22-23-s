using AutoMapper;
using Base.DAL;

namespace Public.DTO.Mappers;

public class GameMapper : BaseMapper<App.Domain.Game, Public.DTO.v1.Domain.GameWithId>
{
    public GameMapper(IMapper mapper) : base(mapper)
    {
    }

    public Public.DTO.v1.Domain.GameWithoutId MapWithoutId(App.Domain.Game game)
    {
        return Mapper.Map<Public.DTO.v1.Domain.GameWithoutId>(game);
    }

    public App.Domain.Game MapWithoutId(Public.DTO.v1.Domain.GameWithoutId game)
    {
        return Mapper.Map<App.Domain.Game>(game);
    }
}

public static class GameMapperExtensions
{
    public static AutoMapperConfig AddGameMap(this AutoMapperConfig config)
    {
        config.CreateMap<App.Domain.Game, Public.DTO.v1.Domain.GameWithoutId>();
        config.CreateMap<Public.DTO.v1.Domain.GameWithoutId, App.Domain.Game>()
            .AfterMap((_, domainGame) => domainGame.Id = new Guid());
        config.CreateMap<App.Domain.Game, Public.DTO.v1.Domain.GameWithId>()
            .ReverseMap();
        return config;
    }
}