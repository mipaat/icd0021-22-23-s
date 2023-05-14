using App.Contracts.DAL;
using App.DAL.DTO.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace App.BLL.YouTube;

public class YouTubeContext
{
    private readonly IServiceProvider _services;
    private const string ApiUsageIdentifier = "YouTubeOfficialApiTokens";

    public YouTubeContext(IServiceProvider services)
    {
        _services = services;
        using var scope = services.CreateScope();
        var uow = scope.ServiceProvider.GetRequiredService<IAppUnitOfWork>();
        var apiQuotaUsage = uow.ApiQuotaUsages.GetLatestByIdentifier(ApiUsageIdentifier).Result;
        if (apiQuotaUsage != null && IsTodayPst(apiQuotaUsage.UpdatedAt))
        {
            _apiUsageAmount = apiQuotaUsage.UsageAmount;
            _apiUsageUpdatedAt = DateTime.UtcNow;
        }
        else
        {
            _apiUsageAmount = 0;
            _apiUsageUpdatedAt = DateTime.UtcNow;
        }
    }

    public event EventHandler<string>? NewCommentsQueued;
    public void QueueNewComments(string videoId) => Task.Run(() => NewCommentsQueued?.Invoke(null, videoId));

    private readonly SemaphoreSlim _apiUsageSemaphore = new(1);
    private DateTime _apiUsageUpdatedAt;
    private int _apiUsageAmount;

    public int ApiUsage
    {
        get
        {
            _apiUsageSemaphore.Wait();

            if (IsBeforeTodayPst(_apiUsageUpdatedAt))
            {
                _apiUsageAmount = 0;
                _apiUsageUpdatedAt = DateTime.UtcNow;
            }

            var apiUsage = _apiUsageAmount;
            _apiUsageSemaphore.Release();
            return apiUsage;
        }
    }

    public async Task IncrementApiUsage(int increment = 1)
    {
        await _apiUsageSemaphore.WaitAsync();

        try
        {
            using var scope = _services.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IAppUnitOfWork>();
            var apiQuotaUsage = await uow.ApiQuotaUsages.GetLatestByIdentifier(ApiUsageIdentifier);
            if (apiQuotaUsage != null && IsBeforeTodayPst(apiQuotaUsage.UpdatedAt))
            {
                apiQuotaUsage = null;
            }

            if (apiQuotaUsage == null)
            {
                apiQuotaUsage = new ApiQuotaUsage
                {
                    Identifier = ApiUsageIdentifier,
                    UpdatedAt = DateTime.UtcNow,
                };
                uow.ApiQuotaUsages.Add(apiQuotaUsage);
            }

            apiQuotaUsage.UsageAmount = Interlocked.Add(ref _apiUsageAmount, increment);
            uow.ApiQuotaUsages.Update(apiQuotaUsage);
            await uow.SaveChangesAsync();
        }
        finally
        {
            _apiUsageSemaphore.Release();
        }
    }

    private static TimeZoneInfo Pst => TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

    private static DateTime CurrentTimePst => ToPst(DateTime.UtcNow);

    private static DateTime ToPst(DateTime dateTime) =>
        TimeZoneInfo.ConvertTimeFromUtc(dateTime.ToUniversalTime(), Pst);

    private static bool IsTodayPst(DateTime dateTime) => ToPst(dateTime).Date == CurrentTimePst.Date;

    private static bool IsBeforeTodayPst(DateTime dateTime) => ToPst(dateTime).Date < CurrentTimePst.Date;

    public readonly object VideoUpdateLock = new();
    public bool VideoUpdateOngoing;

    public readonly object PlaylistUpdateLock = new();
    public bool PlaylistUpdateOngoing;
}