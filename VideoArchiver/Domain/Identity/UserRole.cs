using Domain.Base;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity;

public class UserRole : IdentityUserRole<Guid>, IIdDatabaseEntity
{
    public Guid Id { get; set; }
}