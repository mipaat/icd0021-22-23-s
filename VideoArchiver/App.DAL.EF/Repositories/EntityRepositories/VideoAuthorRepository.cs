using App.Contracts.DAL.Repositories.EntityRepositories;
using App.Domain;
using App.Domain.Enums;
using DAL.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class VideoAuthorRepository : BaseEntityRepository<VideoAuthor, AbstractAppDbContext>, IVideoAuthorRepository
{
    public VideoAuthorRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<VideoAuthor> DefaultIncludes(DbSet<VideoAuthor> entities)
    {
        entities
            .Include(v => v.Author)
            .Include(v => v.Video);
        return entities;
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

    public async Task<ICollection<VideoAuthor>> GetAllByVideoAndAuthor(Video video, Author author, EAuthorRole? authorRole = null)
    {
        return await GetAllByVideoAndAuthor(video.Id, author.Id, authorRole);
    }

    public async Task<ICollection<VideoAuthor>> GetAllByVideoAndAuthor(Guid videoId, Guid authorId, EAuthorRole? authorRole = null)
    {
        var query = Entities.Where(va => va.VideoId == videoId && va.AuthorId == authorId);
        if (authorRole != null)
        {
            query = query.Where(va => va.Role == authorRole);
        }

        return await query.ToListAsync();
    }
}