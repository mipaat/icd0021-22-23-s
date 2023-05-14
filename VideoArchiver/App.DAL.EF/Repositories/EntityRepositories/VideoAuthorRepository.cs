using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using App.DAL.DTO.Enums;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class VideoAuthorRepository : BaseAppEntityRepository<App.Domain.VideoAuthor, VideoAuthor>,
    IVideoAuthorRepository
{
    public VideoAuthorRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext, mapper, uow)
    {
    }

    protected override Func<TQueryable, TQueryable> IncludeDefaultsFunc<TQueryable>()
    {
        return q =>
        {
            q
                .Include(e => e.Author)
                .Include(e => e.Video);
            return q;
        };
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
        var domainAuthorRole = authorRole?.ToDomainAuthorRole();
        if (authorRole != null)
        {
            query = query.Where(va => va.Role == domainAuthorRole);
        }

        return (await query.ToListAsync()).Select(e => Mapper.Map(e)!).ToList();
    }
}