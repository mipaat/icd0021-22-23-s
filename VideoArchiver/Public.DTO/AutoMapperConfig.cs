using AutoMapper;
using Public.DTO.Mappers;

namespace Public.DTO;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        this.AddGameMap();
    }
}