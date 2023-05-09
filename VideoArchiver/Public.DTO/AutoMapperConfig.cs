using AutoMapper;
using Public.DTO.Mappers;

namespace Public.DTO;

/// <summary>
/// Configuration class for configuring an automapper between public DTOs and internal DTOs.
/// </summary>
public class AutoMapperConfig : Profile
{
    /// <summary>
    /// Constructor for creating a new instance of this automapper configuration.
    /// </summary>
    public AutoMapperConfig()
    {
        this.AddGameMap();
    }
}