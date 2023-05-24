using App.BLL.DTO.Entities;
using AutoMapper;

namespace App.BLL.DTO.Mappers;

public class CommentMapper
{
    private readonly IMapper _mapper;
    
    public CommentMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Comment Map(DAL.DTO.Entities.CommentRoot comment)
    {
        return _mapper.Map<Comment>(comment);
    }

    public Comment Map(DAL.DTO.Entities.CommentChild comment)
    {
        return _mapper.Map<Comment>(comment);
    }
}

public static partial class AutoMapperConfigExtensions
{
    public static AutoMapperConfig AddCommentMap(this AutoMapperConfig config)
    {
        config.CreateMap<DAL.DTO.Entities.CommentRoot, Comment>()
            .ForMember(c => c.Replies,
                o => o.MapFrom(c => c.ConversationReplies));
        config.CreateMap<DAL.DTO.Entities.CommentChild, Comment>()
            .ForMember(c => c.Replies,
                o => o.MapFrom(c => new List<Comment>()));
        return config;
    }
}