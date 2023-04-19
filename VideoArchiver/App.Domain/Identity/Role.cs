using Domain.Base;
using Microsoft.AspNetCore.Identity;

namespace App.Domain.Identity;

public class Role : IdentityRole<Guid>, IIdDatabaseEntity
{
}