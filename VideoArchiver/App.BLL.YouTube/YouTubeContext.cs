using App.Contracts.DAL;
using App.Domain;
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
        if (apiQuotaUsage != null && apiQuotaUsage.UsageDate == CurrentDateInPst())
        {
            _apiUsageAmount = apiQuotaUsage.UsageAmount;
            _apiUsageDate = CurrentDateInPst();
        }
        else
        {
            _apiUsageAmount = 0;
            _apiUsageDate = CurrentDateInPst();
        }
    }

    public event EventHandler<string>? NewCommentsQueued;
    public void QueueNewComments(string videoId) => Task.Run(() => NewCommentsQueued?.Invoke(null, videoId));

    private readonly SemaphoreSlim _apiUsageSemaphore = new(1);
    private DateTime _apiUsageDate;
    private int _apiUsageAmount;
    public int ApiUsage
    {
        get
        {
            _apiUsageSemaphore.Wait();

            if (_apiUsageDate < CurrentDateInPst())
            {
                _apiUsageAmount = 0;
                _apiUsageDate = CurrentDateInPst();
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
            if (apiQuotaUsage != null && apiQuotaUsage.UsageDate < CurrentDateInPst())
            {
                apiQuotaUsage = null;
            }

            if (apiQuotaUsage == null)
            {
                apiQuotaUsage = new ApiQuotaUsage
                {
                    Identifier = ApiUsageIdentifier,
                    UsageDate = CurrentDateInPst(),
                };
                uow.ApiQuotaUsages.Add(apiQuotaUsage);
            }

            apiQuotaUsage.UsageAmount = Interlocked.Add(ref _apiUsageAmount, increment);
            await uow.SaveChangesAsync();
        }
        finally
        {
            _apiUsageSemaphore.Release();
        }
    }

    private static DateTime CurrentDateInPst()
    {
        var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
        var dateTimeInPst = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);
        return dateTimeInPst.Date;
    }

    public readonly object VideoUpdateLock = new();
    public bool VideoUpdateOngoing;

    public readonly object PlaylistUpdateLock = new();
    public bool PlaylistUpdateOngoing;
}