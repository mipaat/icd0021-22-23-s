using App.BLL.DTO.Entities;
using AutoMapper;
using Base.Mapping;

namespace App.BLL.DTO.Mappers;

public class QueueItemMapper : BaseMapper<App.DAL.DTO.Entities.QueueItem, QueueItemForApproval>
{
    public QueueItemMapper(IMapper mapper) : base(mapper)
    {
    }
}

public static partial class AutoMapperConfigExtensions
{
    public static AutoMapperConfig AddQueueItemMap(this AutoMapperConfig config)
    {
        config.CreateMap<App.DAL.DTO.Entities.QueueItem, QueueItemForApproval>();
        return config;
    }
}