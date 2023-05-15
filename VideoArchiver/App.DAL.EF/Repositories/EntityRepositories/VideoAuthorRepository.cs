using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using App.Common.Enums;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class VideoAuthorRepository : BaseAppEntityRepository<App.Domain.VideoAuthor, VideoAuthor>,
    IVideoAuthorRepository
{
    public VideoAuthorRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext,
        mapper, uow)
    {
    }

    protected override TQueryable IncludeDefaults<TQueryable>(TQueryable queryable)
    {
        queryable
            .Include(e => e.Author)
            .Include(e => e.Video);
        return queryable;
    }

    protected override Domain.VideoAuthor AfterMap(VideoAuthor entity, Domain.VideoAuthor mapped)
    {
        if (entity.Author != null)
        {
            var trackedAuthor = Uow.Authors.GetTrackedEntity(entity.Author);
            if (trackedAuthor != null)
            {
                mapped.Author = Uow.Authors.Map(entity.Author, trackedAuthor);
            }
        }

        if (entity.Video != null)
        {
            var trackedVideo = Uow.Videos.GetTrackedEntity(entity.Video);
            if (trackedVideo != null)
            {
                mapped.Video = Uow.Videos.Map(entity.Video, trackedVideo);
            }
        }

        return mapped;
    }

    public async Task SetVideoAuthor(Video video, Author author, EAuthorRole authorRole = EAuthorRole.Publisher)
    {
        var videoAuthors = await GetAllByVideoAndAuthor(video, author, authorRole);
        if (videoAuthors.Count > 0) return;

        var videoAuthor = new VideoAuthor
        {
            Video = video,
            VideoId = video.Id,
            Author = author,
            AuthorId = author.Id,
            Role = authorRole,
        };

        Add(videoAuthor);
    }

    public async Task<ICollection<VideoAuthor>> GetAllByVideoAndAuthor(Video video, Author author,
        EAuthorRole? authorRole = null)
    {
        return await GetAllByVideoAndAuthor(video.Id, author.Id, authorRole);
    }

    public async Task<ICollection<VideoAuthor>> GetAllByVideoAndAuthor(Guid videoId, Guid authorId,
        EAuthorRole? authorRole = null)
    {
        var query = Entities.Where(va => va.VideoId == videoId && va.AuthorId == authorId);
        if (authorRole != null)
        {
            query = query.Where(va => va.Role == authorRole);
        }

        return await query.ProjectTo<VideoAuthor>(Mapper.ConfigurationProvider).ToListAsync();
    }
}