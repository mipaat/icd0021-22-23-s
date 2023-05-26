using AutoMapper;
using Base.Mapping;
using Public.DTO.v1;
#pragma warning disable CS1591

namespace Public.DTO.Mappers;

public class QueueItemMapper : BaseMapperUnidirectional<App.BLL.DTO.Entities.QueueItemForApproval, QueueItemForApproval>
{
    public QueueItemMapper(IMapper mapper) : base(mapper)
    {
    }
}

public static partial class AutoMapperConfigExtensions
{
    public static AutoMapperConfig AddQueueItemMap(this AutoMapperConfig config)
    {
        config.CreateMap<App.BLL.DTO.Entities.QueueItemForApproval, QueueItemForApproval>();
        return config;
    }
}