using App.DAL.DTO.Entities;
using App.Common.Enums;
using App.DAL.Contracts;
using App.DAL.Contracts.Repositories.EntityRepositories;
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
        var trackedAuthor = Uow.Authors.GetTrackedEntity(entity.AuthorId);
        if (trackedAuthor != null)
        {
            mapped.Author = trackedAuthor;
        }

        var trackedVideo = Uow.Videos.GetTrackedEntity(entity.VideoId);
        if (trackedVideo != null)
        {
            mapped.Video = trackedVideo;
        }

        return mapped;
    }

    public async Task SetVideoAuthor(Guid videoId, Guid authorId, EAuthorRole authorRole = EAuthorRole.Publisher)
    {
        var videoAuthors = await GetAllByVideoAndAuthor(videoId, authorId, authorRole);
        if (videoAuthors.Count > 0) return;

        var videoAuthor = new VideoAuthor
        {
            VideoId = videoId,
            AuthorId = authorId,
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