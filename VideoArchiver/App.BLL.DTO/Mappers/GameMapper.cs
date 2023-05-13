using AutoMapper;
using Base.Mapping;

namespace App.BLL.DTO.Mappers;

public class GameMapper : BaseMapper<App.DAL.DTO.Entities.Game, Entities.Game>
{
    public GameMapper(IMapper mapper) : base(mapper)
    {
    }
}