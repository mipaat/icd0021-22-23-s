using App.Domain.Enums;
using Utils;

namespace App.BLL.Exceptions;

public class EntityNotFoundOnPlatformException : Exception
{
    public readonly string IdOnPlatform;
    public readonly Platform Platform;
    public readonly Type EntityType;

    public EntityNotFoundOnPlatformException(string idOnPlatform, Platform platform, Type entityType) :
        base($"{entityType.ToString().ToCapitalized()} with ID {idOnPlatform} not found on {platform}")
    {
        IdOnPlatform = idOnPlatform;
        Platform = platform;
        EntityType = entityType;
    }
}