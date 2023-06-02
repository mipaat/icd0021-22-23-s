using System.Security.Claims;
using App.BLL.Contracts;
using App.BLL.Contracts.Services;
using App.BLL.Exceptions;
using App.BLL.Services;
using App.Common.Enums;
using App.DAL.Contracts;
using App.DAL.Contracts.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using App.DAL.DTO.Entities.Playlists;
using Base.Tests;
using Microsoft.Extensions.Logging;
using Moq;
using Tests.Helpers;

namespace Tests.Unit.App.BLL;

public class SubmitServiceTests
{
    private readonly ILogger<SubmitService> _logger;
    private SubmitService? _submitService;

    private SubmitService SubmitService => _submitService ??= new SubmitService(_serviceUowMock.Object, _logger,
        new[] { _platformSubmissionHandlerMock1.Object, _platformSubmissionHandlerMock2.Object });

    private readonly Mock<IQueueItemRepository> _queueItemRepoMock;
    private readonly Mock<IAuthorizationService> _authorizationServiceMock;
    private readonly Mock<IServiceUow> _serviceUowMock;
    private readonly Mock<IPlatformSubmissionHandler> _platformSubmissionHandlerMock1;
    private readonly Mock<IPlatformSubmissionHandler> _platformSubmissionHandlerMock2;

    private const string Platform1 = "Platform1";
    private const string Platform2 = "Platform2";

    public SubmitServiceTests()
    {
        _queueItemRepoMock = new Mock<IQueueItemRepository>();

        var appUowMock = new Mock<IAppUnitOfWork>();
        appUowMock.SetupGet(x => x.QueueItems).Returns(_queueItemRepoMock.Object);

        _authorizationServiceMock = new Mock<IAuthorizationService>();

        _serviceUowMock = new Mock<IServiceUow>();
        _serviceUowMock.SetupGet(x => x.Uow).Returns(appUowMock.Object);
        _serviceUowMock.SetupGet(x => x.AuthorizationService).Returns(_authorizationServiceMock.Object);

        _platformSubmissionHandlerMock1 = new Mock<IPlatformSubmissionHandler>();
        _platformSubmissionHandlerMock1.Setup(
                x => x.IsPlatformUrl(It.IsAny<string>()))
            .Returns<string>(url => url.StartsWith(Platform1));
        _platformSubmissionHandlerMock1.Setup(x => x.CanHandle(
                It.IsAny<EPlatform>(), It.IsAny<EEntityType>()))
            .Returns(false);

        _platformSubmissionHandlerMock2 = new Mock<IPlatformSubmissionHandler>();
        _platformSubmissionHandlerMock2.Setup(
                x => x.IsPlatformUrl(It.IsAny<string>()))
            .Returns<string>(url => url.StartsWith(Platform2));
        _platformSubmissionHandlerMock2.Setup(x => x.CanHandle(
                It.IsAny<EPlatform>(), It.IsAny<EEntityType>()))
            .Returns<EPlatform, EEntityType>((platform, _) => platform == EPlatform.YouTube);

        using var logFactory = LoggerFactory.Create(builder => builder.AddConsole());
        _logger = logFactory.CreateLogger<SubmitService>();
    }

    [Fact]
    public async void TestSubmitGenericUrlAsyncFirstMatchAsync()
    {
        const string url = Platform1;
        await SubmitService.SubmitGenericUrlAsync(url, TestUsers.AdminUser, false);

        _platformSubmissionHandlerMock1.Verify(x => x.IsPlatformUrl(It.Is<string>(u => u == url)), Times.Once);
        _platformSubmissionHandlerMock1.Verify(x => x.IsPlatformUrl(It.IsAny<string>()), Times.Once);
        _platformSubmissionHandlerMock2.Verify(x => x.IsPlatformUrl(It.IsAny<string>()), Times.Never);
        _platformSubmissionHandlerMock1.Verify(x => x.SubmitUrl(
                It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<bool>()),
            Times.Once);
    }

    [Fact]
    public async void TestSubmitGenericUrlAsyncNotFirstMatchAsync()
    {
        const string url = Platform2;
        await SubmitService.SubmitGenericUrlAsync(url, TestUsers.AdminUser, false);

        _platformSubmissionHandlerMock1.Verify(x => x.IsPlatformUrl(It.IsAny<string>()), Times.Once);
        _platformSubmissionHandlerMock2.Verify(x => x.IsPlatformUrl(It.IsAny<string>()), Times.Once);
        _platformSubmissionHandlerMock2.Verify(x => x.IsPlatformUrl(It.Is<string>(u => u == url)), Times.Once);
        _platformSubmissionHandlerMock1.Verify(x => x.SubmitUrl(
                It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<bool>()),
            Times.Never);
        _platformSubmissionHandlerMock2.Verify(x => x.SubmitUrl(
                It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<bool>()),
            Times.Once);
    }

    [Fact]
    public async void TestSubmitGenericUrlAsyncNoMatchAsync()
    {
        const string url = "FakeUrl";
        await Assert.ThrowsAnyAsync<UnrecognizedUrlException>(async () =>
            await SubmitService.SubmitGenericUrlAsync(url, TestUsers.AdminUser, false));

        _platformSubmissionHandlerMock1.Verify(x => x.IsPlatformUrl(It.IsAny<string>()), Times.Once);
        _platformSubmissionHandlerMock2.Verify(x => x.IsPlatformUrl(It.IsAny<string>()), Times.Once);
        _platformSubmissionHandlerMock1.Verify(x => x.SubmitUrl(
                It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<bool>()),
            Times.Never);
        _platformSubmissionHandlerMock2.Verify(x => x.SubmitUrl(
                It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<bool>()),
            Times.Never);
    }

    [Theory]
    [ClassData(typeof(AllowedToAutoSubmitUsersGenerator))]
    public async void TestSubmitGenericUrlAsyncIsAllowedToAutoSubmitAsync(ClaimsPrincipal user, bool isAllowed)
    {
        const string url = Platform1;
        await SubmitService.SubmitGenericUrlAsync(url, user, false);
        _platformSubmissionHandlerMock1.Verify(x => x.SubmitUrl(
                It.IsAny<string>(), It.IsAny<Guid>(), It.Is<bool>(v => v == isAllowed), It.IsAny<bool>()),
            Times.Once);
    }

    private Guid BaseArrangeTestSubmitQueueItem(QueueItem queueItem)
    {
        var submittedEntityId = Guid.NewGuid();
        _platformSubmissionHandlerMock2.Setup(x =>
                x.SubmitVideo(It.Is<string>(id =>
                    id == queueItem.IdOnPlatform)))
            .Returns(Task.FromResult(new Video { Id = submittedEntityId }));
        _platformSubmissionHandlerMock2.Setup(x =>
                x.SubmitPlaylist(It.Is<string>(id =>
                    id == queueItem.IdOnPlatform)))
            .Returns(Task.FromResult(new Playlist { Id = submittedEntityId }));
        _platformSubmissionHandlerMock2.Setup(x =>
                x.SubmitAuthor(It.Is<string>(id =>
                    id == queueItem.IdOnPlatform)))
            .Returns(Task.FromResult(new Author { Id = submittedEntityId }));
        return submittedEntityId;
    }

    [Theory]
    [ClassData(typeof(HandleableQueueItemsGenerator))]
    public async void TestSubmitQueueItemAsyncSubmitsEntityAndUpdatesQueueItemAsync(QueueItem queueItem)
    {
        // Arrange
        var testStart = DateTime.UtcNow;
        var submittedEntityId = BaseArrangeTestSubmitQueueItem(queueItem);

        // Act
        await SubmitService.SubmitQueueItemAsync(queueItem);

        // Assert
        var isVideo = queueItem.EntityType == EEntityType.Video;
        var isAuthor = queueItem.EntityType == EEntityType.Author;
        var isPlaylist = queueItem.EntityType == EEntityType.Playlist;

        if (isVideo)
        {
            Assert.Equal(submittedEntityId, queueItem.VideoId);
        }

        if (isPlaylist)
        {
            Assert.Equal(submittedEntityId, queueItem.PlaylistId);
        }

        if (isAuthor)
        {
            Assert.Equal(submittedEntityId, queueItem.AuthorId);
        }

        var submitVideoTimes = isVideo ? Times.Once() : Times.Never();
        var submitAuthorTimes = isAuthor ? Times.Once() : Times.Never();
        var submitPlaylistTimes = isPlaylist ? Times.Once() : Times.Never();

        _platformSubmissionHandlerMock2.Verify(x =>
            x.SubmitVideo(It.Is<string>(id =>
                id == queueItem.IdOnPlatform || !isVideo)), submitVideoTimes);
        _platformSubmissionHandlerMock2.Verify(x =>
            x.SubmitAuthor(It.Is<string>(id =>
                id == queueItem.IdOnPlatform || !isAuthor)), submitAuthorTimes);
        _platformSubmissionHandlerMock2.Verify(x =>
            x.SubmitPlaylist(It.Is<string>(id =>
                id == queueItem.IdOnPlatform || !isPlaylist)), submitPlaylistTimes);

        _platformSubmissionHandlerMock1.Verify(x => x.SubmitVideo(It.IsAny<string>()), Times.Never);
        _platformSubmissionHandlerMock1.Verify(x => x.SubmitAuthor(It.IsAny<string>()), Times.Never);
        _platformSubmissionHandlerMock1.Verify(x => x.SubmitPlaylist(It.IsAny<string>()), Times.Never);

        Assert.True(queueItem.CompletedAt >= testStart);

        _queueItemRepoMock.Verify(x => x.Update(It.Is<QueueItem>(e => e == queueItem)), Times.Once);
    }

    [Theory]
    [ClassData(typeof(HandleableApprovableQueueItemsGenerator))]
    public async void TestSubmitQueueItemsAsyncAddsAuthorizationIfNeededAsync(QueueItem queueItem)
    {
        // Arrange
        BaseArrangeTestSubmitQueueItem(queueItem);

        // Act
        await SubmitService.SubmitQueueItemAsync(queueItem);

        // Assert
        var grantAccessVideoTimes = queueItem is { EntityType: EEntityType.Video, GrantAccess: true }
            ? Times.Once()
            : Times.Never();
        var grantAccessAuthorTimes = queueItem is { EntityType: EEntityType.Author, GrantAccess: true }
            ? Times.Once()
            : Times.Never();
        var grantAccessPlaylistTimes = queueItem is { EntityType: EEntityType.Playlist, GrantAccess: true }
            ? Times.Once()
            : Times.Never();

        _authorizationServiceMock.Verify(x => x.AuthorizeVideoIfNotAuthorized(queueItem.AddedById, It.IsAny<Guid>()),
            grantAccessVideoTimes);
        _authorizationServiceMock.Verify(x => x.AuthorizePlaylistIfNotAuthorized(queueItem.AddedById, It.IsAny<Guid>()),
            grantAccessPlaylistTimes);
        _authorizationServiceMock.Verify(x => x.AuthorizeAuthorIfNotAuthorized(queueItem.AddedById, It.IsAny<Guid>()),
            grantAccessAuthorTimes);
    }
}

internal class AllowedToAutoSubmitUsersGenerator : TestDataGenerator
{
    protected override List<object[]> Data => new()
    {
        new object[] { TestUsers.AdminUser, true },
        new object[] { TestUsers.SuperAdminUser, true },
        new object[] { TestUsers.HelperUser, false },
        new object[] { TestUsers.NoRoleUser, false },
    };
}

internal class HandleableQueueItemsGenerator : TestDataGenerator
{
    protected override List<object[]> Data => new()
    {
        new object[]
        {
            new QueueItem
            {
                Id = Guid.NewGuid(),
                AddedById = Guid.NewGuid(),
                CompletedAt = null,
                Platform = EPlatform.YouTube,
                EntityType = EEntityType.Video,
            }
        },
        new object[]
        {
            new QueueItem
            {
                Id = Guid.NewGuid(),
                AddedById = Guid.NewGuid(),
                CompletedAt = null,
                Platform = EPlatform.YouTube,
                EntityType = EEntityType.Author,
            }
        },
        new object[]
        {
            new QueueItem
            {
                Id = Guid.NewGuid(),
                AddedById = Guid.NewGuid(),
                CompletedAt = null,
                Platform = EPlatform.YouTube,
                EntityType = EEntityType.Playlist,
            }
        },
    };
}

internal class HandleableApprovableQueueItemsGenerator : TestDataGenerator
{
    protected override List<object[]> Data => new()
    {
        new object[]
        {
            new QueueItem
            {
                Id = Guid.NewGuid(),
                AddedById = Guid.NewGuid(),
                CompletedAt = null,
                Platform = EPlatform.YouTube,
                EntityType = EEntityType.Video,
            }
        },
        new object[]
        {
            new QueueItem
            {
                Id = Guid.NewGuid(),
                AddedById = Guid.NewGuid(),
                CompletedAt = null,
                Platform = EPlatform.YouTube,
                EntityType = EEntityType.Author,
            }
        },
        new object[]
        {
            new QueueItem
            {
                Id = Guid.NewGuid(),
                AddedById = Guid.NewGuid(),
                CompletedAt = null,
                Platform = EPlatform.YouTube,
                EntityType = EEntityType.Playlist,
            }
        },
        new object[]
        {
            new QueueItem
            {
                Id = Guid.NewGuid(),
                AddedById = Guid.NewGuid(),
                CompletedAt = null,
                Platform = EPlatform.YouTube,
                EntityType = EEntityType.Video,
                GrantAccess = true,
            }
        },
        new object[]
        {
            new QueueItem
            {
                Id = Guid.NewGuid(),
                AddedById = Guid.NewGuid(),
                CompletedAt = null,
                Platform = EPlatform.YouTube,
                EntityType = EEntityType.Author,
                GrantAccess = true,
            }
        },
        new object[]
        {
            new QueueItem
            {
                Id = Guid.NewGuid(),
                AddedById = Guid.NewGuid(),
                CompletedAt = null,
                Platform = EPlatform.YouTube,
                EntityType = EEntityType.Playlist,
                GrantAccess = true,
            }
        },
    };
}