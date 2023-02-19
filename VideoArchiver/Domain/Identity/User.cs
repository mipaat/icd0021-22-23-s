using Domain.Base;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity;

public class User : IdentityUser<Guid>, IIdDatabaseEntity
{
}