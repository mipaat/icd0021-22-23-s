using AutoMapper;
using Contracts.DAL;

namespace Base.DAL;

public class BaseMapperUnidirectional<TSource, TDestination> : IMapperUnidirectional<TSource, TDestination>
{
    protected readonly AutoMapper.IMapper Mapper;

    public BaseMapperUnidirectional(IMapper mapper)
    {
        Mapper = mapper;
    }

    public virtual TDestination? Map(TSource? entity)
    {
        return Mapper.Map<TDestination>(entity);
    }
}

public class BaseMapper<TSource, TDestination> : BaseMapperUnidirectional<TSource, TDestination>, IMapper<TSource, TDestination>
{
    public BaseMapper(AutoMapper.IMapper mapper) : base(mapper)
    {
    }

    public virtual TSource? Map(TDestination? entity)
    {
        return Mapper.Map<TSource>(entity);
    }
}