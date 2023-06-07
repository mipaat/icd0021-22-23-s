using App.BLL.Contracts;
using App.BLL.Contracts.Services;
using App.BLL.DTO.Entities;
using App.BLL.DTO.Enums;
using App.BLL.Services;
using App.Common;
using App.Common.Enums;
using App.Common.Exceptions;
using App.DAL.Contracts;
using App.DAL.Contracts.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;
using Base.WebHelpers;
using Microsoft.Extensions.Logging;
using Moq;
using Tests.Helpers;
using Comment = App.BLL.DTO.Entities.Comment;

namespace Tests.Unit.App.BLL;

public class VideoPresentationServiceTests
{
    private readonly ILogger<VideoPresentationService> _logger;

    private readonly Mock<IServiceUow> _serviceUowMock;
    private readonly Mock<IPlatformVideoPresentationHandler> _presentationHandler1;
    private readonly Mock<IPlatformVideoPresentationHandler> _presentationHandler2;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICommentService> _commentServiceMock;
    private readonly Mock<IVideoRepository> _videoRepoMock;

    private readonly VideoWithBasicAuthors _existingVideo = new()
    {
        Id = Guid.NewGuid(),
    };

    private readonly ImageFile _customThumbnail = new()
    {
        Url = "ImageFileUrl",
    };

    private readonly Comment _defaultComment = new()
    {
        Id = Guid.NewGuid(),
    };

    private VideoPresentationService ConstructService(params IPlatformVideoPresentationHandler[] handlers)
    {
        return new VideoPresentationService(handlers,
            _serviceUowMock.Object, _logger, _mapperMock.Object);
    }

    private readonly Guid _noVideoFileVideoId = Guid.NewGuid();
    private readonly Guid _oneVideoFileVideoId = Guid.NewGuid();
    private readonly Guid _multipleVideoFilesVideoId = Guid.NewGuid();

    private static readonly VideoFile DefaultVideoFile = new()
    {
        FilePath = "filepath1"
    };

    private static readonly List<VideoFile> VideoFiles = new()
    {
        new() { FilePath = "filepath2" },
        DefaultVideoFile,
        new() { FilePath = "filepath3" },
    };

    public VideoPresentationServiceTests()
    {
        _videoRepoMock = new Mock<IVideoRepository>();
        _videoRepoMock.Setup(e =>
                e.GetByIdWithBasicAuthorsAsync(It.Is<Guid>(i => i == _existingVideo.Id)))
            .ReturnsAsync(_existingVideo);
        _videoRepoMock.Setup(e => e.GetVideoFilesAsync(It.Is<Guid>(i => i == _noVideoFileVideoId)))
            .ReturnsAsync(new List<VideoFile>());
        _videoRepoMock.Setup(e => e.GetVideoFilesAsync(It.Is<Guid>(i => i == _oneVideoFileVideoId)))
            .ReturnsAsync(new List<VideoFile>
            {
                DefaultVideoFile
            });
        _videoRepoMock.Setup(e => e.GetVideoFilesAsync(It.Is<Guid>(i => i == _multipleVideoFilesVideoId)))
            .ReturnsAsync(VideoFiles);
        _videoRepoMock.Setup(e => e.SearchVideosAsync(
                It.IsAny<EPlatform?>(), It.IsAny<string?>(), It.IsAny<string?>(),
                It.IsAny<ICollection<Guid>?>(), It.IsAny<Guid?>(), It.IsAny<Guid?>(),
                It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<EVideoSortingOptions>(), It.IsAny<bool>()
            ))
            .ReturnsAsync(new List<BasicVideoWithBasicAuthors> { new()
            {
                Id = _existingVideo.Id,
            } });

        var appUowMock = new Mock<IAppUnitOfWork>();
        appUowMock.SetupGet(e => e.Videos).Returns(_videoRepoMock.Object);

        _commentServiceMock = new Mock<ICommentService>();
        _commentServiceMock.Setup(e => e.LoadVideoComments(It.IsAny<VideoWithAuthor>(),
                It.IsAny<int>(), It.IsAny<int>()))
            .Returns<VideoWithAuthor, int, int>((e, _, _) =>
                Task.FromResult(new VideoWithAuthorAndComments
                {
                    Id = e.Id,
                    Comments = new List<Comment>
                    {
                        _defaultComment
                    }
                }));

        _serviceUowMock = new Mock<IServiceUow>();
        _serviceUowMock.SetupGet(e => e.Uow).Returns(appUowMock.Object);
        _serviceUowMock.SetupGet(e => e.CommentService).Returns(_commentServiceMock.Object);

        _mapperMock = new Mock<IMapper>();
        _mapperMock.Setup(e => e.Map<VideoWithAuthor>(
                It.IsAny<VideoWithBasicAuthors>()))
            .Returns<VideoWithBasicAuthors>(e => new VideoWithAuthor
            {
                Id = e.Id,
            });
        _mapperMock.Setup(e => e.Map<BasicVideoWithAuthor>(It.IsAny<BasicVideoWithBasicAuthors>()))
            .Returns<BasicVideoWithBasicAuthors>(e => new BasicVideoWithAuthor
            {
                Id = e.Id,
            });

        _presentationHandler1 = new Mock<IPlatformVideoPresentationHandler>();
        _presentationHandler1.Setup(e => e.CanHandle(It.IsAny<VideoWithAuthor>()))
            .Returns(false);
        _presentationHandler1.Setup(e =>
                e.CanHandle(It.Is<VideoWithAuthor>(x => x.Id == _existingVideo.Id)))
            .Returns(true);
        _presentationHandler1.Setup(e =>
                e.Handle(It.Is<VideoWithAuthor>(x => x.Id == _existingVideo.Id)))
            .Callback<VideoWithAuthor>(e => { e.Url = e.Id.ToString(); });
        _presentationHandler2 = new Mock<IPlatformVideoPresentationHandler>();
        _presentationHandler2.Setup(e => e.CanHandle(It.IsAny<VideoWithAuthor>()))
            .Returns(false);

        _presentationHandler1.Setup(e => e.CanHandle(It.IsAny<VideoWithAuthorAndComments>()))
            .Returns(false);
        _presentationHandler1.Setup(e =>
                e.CanHandle(It.Is<VideoWithAuthorAndComments>(x => x.Id == _existingVideo.Id)))
            .Returns(true);
        _presentationHandler1.Setup(e =>
                e.Handle(It.Is<VideoWithAuthorAndComments>(x => x.Id == _existingVideo.Id)))
            .Callback<VideoWithAuthorAndComments>(e => { e.Url = e.Id.ToString(); });
        _presentationHandler2.Setup(e => e.CanHandle(It.IsAny<VideoWithAuthorAndComments>()))
            .Returns(false);

        _presentationHandler1.Setup(e => e.CanHandle(It.IsAny<BasicVideoWithAuthor>()))
            .Returns(false);
        _presentationHandler1.Setup(e =>
                e.CanHandle(It.Is<BasicVideoWithAuthor>(x => x.Id == _existingVideo.Id)))
            .Returns(true);
        _presentationHandler1.Setup(e =>
                e.Handle(It.Is<BasicVideoWithAuthor>(x => x.Id == _existingVideo.Id)))
            .Callback<BasicVideoWithAuthor>(e => { e.Thumbnail = _customThumbnail; });
        _presentationHandler2.Setup(e => e.CanHandle(It.IsAny<BasicVideoWithAuthor>()))
            .Returns(false);

        using var logFactory = LoggerFactory.Create(builder => builder.AddConsole());
        _logger = logFactory.CreateLogger<VideoPresentationService>();
    }

    [Fact]
    public async Task TestGetVideoWithAuthorVideoNotExists()
    {
        var videoPresentationService =
            ConstructService(_presentationHandler1.Object, _presentationHandler2.Object);
        var video = await videoPresentationService.GetVideoWithAuthor(Guid.NewGuid());
        Assert.Null(video);
        _presentationHandler1.Verify(e => e.Handle(
            It.IsAny<VideoWithAuthor>()), Times.Never);
        _presentationHandler1.Verify(e => e.CanHandle(
            It.IsAny<VideoWithAuthor>()), Times.Never);
    }

    [Fact]
    public async Task TestGetVideoWithAuthorNoHandlers()
    {
        var videoPresentationService = ConstructService();
        var result = await videoPresentationService.GetVideoWithAuthor(_existingVideo.Id);
        Assert.NotNull(result);
        Assert.Equal(_existingVideo.Id, result.Id);
    }

    [Fact]
    public async Task TestGetVideoWithAuthorNoSuitableHandlers()
    {
        _presentationHandler1.Setup(e =>
                e.CanHandle(It.IsAny<VideoWithAuthor>()))
            .Returns(false);
        _presentationHandler2.Setup(e => e.CanHandle(It.IsAny<VideoWithAuthor>()))
            .Returns(false);
        var videoPresentationService =
            ConstructService(_presentationHandler1.Object, _presentationHandler2.Object);

        var result = await videoPresentationService.GetVideoWithAuthor(_existingVideo.Id);
        Assert.NotNull(result);
        Assert.Equal(_existingVideo.Id, result.Id);
        _presentationHandler1.Verify(e =>
                e.CanHandle(It.Is<VideoWithAuthor>(x => x.Id == _existingVideo.Id)),
            Times.Once);
        _presentationHandler2.Verify(e =>
                e.CanHandle(It.Is<VideoWithAuthor>(x => x.Id == _existingVideo.Id)),
            Times.Once);
        _presentationHandler1.Verify(e =>
                e.Handle(It.IsAny<VideoWithAuthor>()),
            Times.Never);
        _presentationHandler2.Verify(e =>
                e.Handle(It.IsAny<VideoWithAuthor>()),
            Times.Never);
    }

    [Fact]
    public async Task TestGetVideoWithAuthorFirstHandler()
    {
        var videoPresentationService =
            ConstructService(_presentationHandler1.Object, _presentationHandler2.Object);

        var result = await videoPresentationService.GetVideoWithAuthor(_existingVideo.Id);
        Assert.NotNull(result);
        Assert.Equal(_existingVideo.Id.ToString(), result.Url);
        _presentationHandler1.Verify(e =>
            e.CanHandle(It.Is<VideoWithAuthor>(x => x.Id == _existingVideo.Id)), Times.Once);
        _presentationHandler1.Verify(e =>
            e.Handle(It.Is<VideoWithAuthor>(x => x.Id == _existingVideo.Id)), Times.Once);
        _presentationHandler2.Verify(e =>
            e.CanHandle(It.IsAny<VideoWithAuthor>()), Times.Never);
        _presentationHandler2.Verify(e =>
            e.Handle(It.IsAny<VideoWithAuthor>()), Times.Never);
    }

    [Fact]
    public async Task TestGetVideoWithAuthorNotFirstHandler()
    {
        var videoPresentationService =
            ConstructService(_presentationHandler2.Object, _presentationHandler1.Object);

        var result = await videoPresentationService.GetVideoWithAuthor(_existingVideo.Id);
        Assert.NotNull(result);
        Assert.Equal(_existingVideo.Id.ToString(), result.Url);
        _presentationHandler1.Verify(e =>
            e.CanHandle(It.Is<VideoWithAuthor>(x => x.Id == _existingVideo.Id)), Times.Once);
        _presentationHandler1.Verify(e =>
            e.Handle(It.Is<VideoWithAuthor>(x => x.Id == _existingVideo.Id)), Times.Once);
        _presentationHandler2.Verify(e =>
            e.CanHandle(It.Is<VideoWithAuthor>(x => x.Id == _existingVideo.Id)), Times.Once);
        _presentationHandler2.Verify(e =>
            e.Handle(It.Is<VideoWithAuthor>(x => x.Id == _existingVideo.Id)), Times.Never);
    }

    [Fact]
    public async Task TestGetVideoWithAuthorAndCommentsNotFound()
    {
        var videoPresentationService = ConstructService();
        await Assert.ThrowsAnyAsync<NotFoundException>(async () =>
            await videoPresentationService.GetVideoWithAuthorAndCommentsAsync(Guid.NewGuid(), 50, 0));
    }

    [Fact]
    public async Task TestGetVideoWithAuthorAndCommentsCallsLoadVideoComments()
    {
        var videoPresentationService = ConstructService();
        var result = await videoPresentationService.GetVideoWithAuthorAndCommentsAsync(
            _existingVideo.Id, 34, 5);
        _commentServiceMock.Verify(e => e.LoadVideoComments(
            It.Is<VideoWithAuthor>(x => x.Id == _existingVideo.Id),
            It.Is<int>(x => x == 34),
            It.Is<int>(x => x == 5)), Times.Once);
        Assert.Equal(1, result.Comments.Count);
        Assert.Contains(result.Comments, c => c.Id == _defaultComment.Id);
    }

    [Fact]
    public async Task TestGetVideoWithAuthorAndCommentsNotFirstHandler()
    {
        var videoPresentationService = ConstructService(_presentationHandler2.Object, _presentationHandler1.Object);
        var result = await videoPresentationService.GetVideoWithAuthorAndCommentsAsync(_existingVideo.Id, 50, 0);

        Assert.Equal(_existingVideo.Id.ToString(), result.Url);
        _presentationHandler1.Verify(e =>
            e.CanHandle(It.Is<VideoWithAuthorAndComments>(x => x.Id == _existingVideo.Id)), Times.Once);
        _presentationHandler1.Verify(e =>
            e.Handle(It.Is<VideoWithAuthorAndComments>(x => x.Id == _existingVideo.Id)), Times.Once);
        _presentationHandler2.Verify(e =>
            e.CanHandle(It.Is<VideoWithAuthorAndComments>(x => x.Id == _existingVideo.Id)), Times.Once);
        _presentationHandler2.Verify(e =>
            e.Handle(It.Is<VideoWithAuthorAndComments>(x => x.Id == _existingVideo.Id)), Times.Never);
    }

    [Fact]
    public async Task TestGetVideoFileNoFile()
    {
        var videoPresentationService = ConstructService();
        var result = await videoPresentationService.GetVideoFileAsync(_noVideoFileVideoId);
        Assert.Null(result);
    }

    [Fact]
    public async Task TestGetVideoFileOneFile()
    {
        var videoPresentationService = ConstructService();
        var result = await videoPresentationService.GetVideoFileAsync(_oneVideoFileVideoId);
        Assert.NotNull(result);
        Assert.Equal(DefaultVideoFile.FilePath, result.FilePath);
    }

    [Fact]
    public async Task TestGetVideoFileMultipleFiles()
    {
        var videoPresentationService = ConstructService();
        var result = await videoPresentationService.GetVideoFileAsync(_multipleVideoFilesVideoId);
        Assert.NotNull(result);
        Assert.Contains(VideoFiles, v => v.FilePath == result.FilePath);
    }

    private const EPlatform PlatformQuery = EPlatform.YouTube;
    private const string NameQuery = "videoTitle";
    private const string AuthorQuery = "videoAuthor";

    private readonly ICollection<Guid> _categoryIds = new List<Guid>
    {
        Guid.NewGuid(),
        Guid.NewGuid(),
    };

    private readonly Guid _userAuthorId = Guid.NewGuid();
    private const EVideoSortingOptions VideoSortingOptions = EVideoSortingOptions.CreatedAt;
    private const bool Descending = true;

    [Fact]
    public async Task TestSearchVideosCallsSearchVideosWithCorrectValues()
    {
        var videoPresentationService = ConstructService();
        await videoPresentationService.SearchVideosAsync(PlatformQuery, NameQuery, AuthorQuery, _categoryIds,
            TestUsers.AdminUser, _userAuthorId,
            2, 6, VideoSortingOptions, Descending);
        _videoRepoMock.Verify(e => e.SearchVideosAsync(
            It.Is<EPlatform?>(x => x == PlatformQuery),
            It.Is<string?>(x => x == NameQuery),
            It.Is<string?>(x => x == AuthorQuery),
            It.Is<ICollection<Guid>?>(x => x == _categoryIds),
            It.Is<Guid?>(x => x == TestUsers.AdminUser.GetUserId()),
            It.Is<Guid?>(x => x == _userAuthorId),
            It.Is<bool>(x => x == true),
            It.Is<int>(x => x == 2 * 6),
            It.Is<int>(x => x == 6),
            It.Is<EVideoSortingOptions>(x => x == VideoSortingOptions),
            It.Is<bool>(x => x == Descending)
        ), Times.Once);
    }

    [Fact]
    public async Task TestSearchVideosCallsSearchVideosWithConformedPaginationValues()
    {
        var videoPresentationService = ConstructService();
        await videoPresentationService.SearchVideosAsync(PlatformQuery, NameQuery, AuthorQuery, _categoryIds,
            TestUsers.AdminUser, _userAuthorId,
            -5, 1234, VideoSortingOptions, Descending);
        _videoRepoMock.Verify(e => e.SearchVideosAsync(
            It.Is<EPlatform?>(x => x == PlatformQuery),
            It.Is<string?>(x => x == NameQuery),
            It.Is<string?>(x => x == AuthorQuery),
            It.Is<ICollection<Guid>?>(x => x == _categoryIds),
            It.Is<Guid?>(x => x == TestUsers.AdminUser.GetUserId()),
            It.Is<Guid?>(x => x == _userAuthorId),
            It.Is<bool>(x => x == true),
            It.Is<int>(x => x == 0),
            It.Is<int>(x => x < 500),
            It.Is<EVideoSortingOptions>(x => x == VideoSortingOptions),
            It.Is<bool>(x => x == Descending)
        ), Times.Once);
    }

    [Fact]
    public async Task TestSearchVideosNotFirstHandler()
    {
        var videoPresentationService = ConstructService(_presentationHandler2.Object, _presentationHandler1.Object);
        var result = await videoPresentationService.SearchVideosAsync(PlatformQuery, NameQuery, AuthorQuery, _categoryIds,
            TestUsers.AdminUser, _userAuthorId,
            0, 50, VideoSortingOptions, Descending);
        _presentationHandler1.Verify(e =>
            e.CanHandle(It.Is<BasicVideoWithAuthor>(x => x.Id == _existingVideo.Id)), Times.Once);
        _presentationHandler1.Verify(e =>
            e.Handle(It.Is<BasicVideoWithAuthor>(x => x.Id == _existingVideo.Id)), Times.Once);
        _presentationHandler2.Verify(e =>
            e.CanHandle(It.Is<BasicVideoWithAuthor>(x => x.Id == _existingVideo.Id)), Times.Once);
        _presentationHandler2.Verify(e =>
            e.Handle(It.Is<BasicVideoWithAuthor>(x => x.Id == _existingVideo.Id)), Times.Never);
        Assert.Single(result);
        var video = result[0];
        Assert.Equal(_customThumbnail.Url, video.Thumbnail?.Url);
    }

    [Fact]
    public async Task TestSearchVideosNoUserId()
    {
        var videoPresentationService = ConstructService();
        await videoPresentationService.SearchVideosAsync(PlatformQuery, NameQuery, AuthorQuery, _categoryIds,
            TestUsers.EmptyUser, _userAuthorId,
            0, 50, VideoSortingOptions, Descending);
        _videoRepoMock.Verify(e => e.SearchVideosAsync(
            It.Is<EPlatform?>(x => x == PlatformQuery),
            It.Is<string?>(x => x == NameQuery),
            It.Is<string?>(x => x == AuthorQuery),
            It.Is<ICollection<Guid>?>(x => x == _categoryIds),
            It.Is<Guid?>(x => x == null),
            It.Is<Guid?>(x => x == _userAuthorId),
            It.Is<bool>(x => x == false),
            It.Is<int>(x => x == 0),
            It.Is<int>(x => x == 50),
            It.Is<EVideoSortingOptions>(x => x == VideoSortingOptions),
            It.Is<bool>(x => x == Descending)
        ), Times.Once);
    }

    [Fact]
    public async Task TestSearchVideosNoAccessByRole()
    {
        var videoPresentationService = ConstructService();
        await videoPresentationService.SearchVideosAsync(PlatformQuery, NameQuery, AuthorQuery, _categoryIds,
            TestUsers.NoRoleUser, _userAuthorId,
            0, 50, VideoSortingOptions, Descending);
        _videoRepoMock.Verify(e => e.SearchVideosAsync(
            It.Is<EPlatform?>(x => x == PlatformQuery),
            It.Is<string?>(x => x == NameQuery),
            It.Is<string?>(x => x == AuthorQuery),
            It.Is<ICollection<Guid>?>(x => x == _categoryIds),
            It.Is<Guid?>(x => x == TestUsers.NoRoleUser.GetUserId()),
            It.Is<Guid?>(x => x == _userAuthorId),
            It.Is<bool>(x => x == false),
            It.Is<int>(x => x == 0),
            It.Is<int>(x => x == 50),
            It.Is<EVideoSortingOptions>(x => x == VideoSortingOptions),
            It.Is<bool>(x => x == Descending)
        ), Times.Once);
    }
}