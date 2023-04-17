using Domain.Base;

namespace Domain.Identity;

public class RefreshToken : BaseRefreshToken
{
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public RefreshToken(double expiresInDays = 7) : base(TimeSpan.FromDays(expiresInDays))
    {
    }
}