using Domain.Base;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity;

public class Role : IdentityRole<Guid>, IIdDatabaseEntity
{
}