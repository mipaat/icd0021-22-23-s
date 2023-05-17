using Microsoft.AspNetCore.Authentication.JwtBearer;

#pragma warning disable 1591
namespace WebApp.Authorization;

public static class AuthenticationSchemeDefaults
{
    public const string IdentityCookie = "Identity.Application";
    public const string JwtBearer = JwtBearerDefaults.AuthenticationScheme;
    public const string IdentityAndJwt = IdentityCookie + "," + JwtBearer;
}