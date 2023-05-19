using App.Common.Enums;

namespace App.DAL.EF.Extensions;

public static class AccessExtensions
{
    public static IQueryable<Domain.Video> WhereUserIsAllowedToAccessVideoOrVideoIsPublic(
        this IQueryable<Domain.Video> query, AbstractAppDbContext dbContext, Guid? userId)
    {
        if (userId != null)
        {
            return query.Where(v => v.InternalPrivacyStatus == EPrivacyStatus.Public ||
                                    dbContext.EntityAccessPermissions.Any(p =>
                                        p.VideoId == v.Id && p.UserId == userId) ||
                                    dbContext.PlaylistVideos.Any(p => p.VideoId == v.Id &&
                                                                      dbContext.EntityAccessPermissions.Any(e =>
                                                                          e.UserId == userId &&
                                                                          e.PlaylistId == p.PlaylistId)));
        }

        return query.Where(v => v.InternalPrivacyStatus == EPrivacyStatus.Public);
    }
}