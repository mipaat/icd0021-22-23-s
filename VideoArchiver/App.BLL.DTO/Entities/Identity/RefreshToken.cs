using Domain.Base;

namespace App.BLL.DTO.Entities.Identity;

public class RefreshToken : BaseRefreshToken
{
    public RefreshToken(int expiresInDays) : base(TimeSpan.FromDays(expiresInDays))
    {
    }

    public RefreshToken() : this(7)
    {
    }

    public RefreshToken(TimeSpan expiresIn) : base(expiresIn)
    {
    }
}