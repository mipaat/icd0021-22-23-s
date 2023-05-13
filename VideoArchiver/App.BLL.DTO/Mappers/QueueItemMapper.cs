using AutoMapper;
using Base.Mapping;

namespace App.BLL.DTO.Mappers;

public class QueueItemMapper : BaseMapperUnidirectional<App.DAL.DTO.Entities.QueueItem, Entities.QueueItem>
{
    public QueueItemMapper(IMapper mapper) : base(mapper)
    {
    }
}

public static class QueueItemMapperExtensions
{
    public static AutoMapperConfig AddQueueItemMap(this AutoMapperConfig config)
    {
        config.CreateMap<App.DAL.DTO.Entities.QueueItem, Entities.QueueItem>();
        return config;
    }
}