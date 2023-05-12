namespace App.DAL.DTO.Enums;

public enum EAuthorRole
{
    Publisher
}

public static class AuthorRoleExtensions
{
    public static App.Domain.Enums.EAuthorRole ToDomainAuthorRole(this EAuthorRole authorRole)
    {
        return authorRole switch
        {
            EAuthorRole.Publisher => Domain.Enums.EAuthorRole.Publisher,
            _ => throw new ArgumentException($"Invalid {typeof(EAuthorRole)}: {authorRole}", nameof(authorRole)),
        };
    }
}