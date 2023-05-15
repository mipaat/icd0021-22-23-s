using App.Common.Enums;
using Utils;

namespace App.BLL.Exceptions;

public class EntityNotFoundOnPlatformException : Exception
{
    public readonly string IdOnPlatform;
    public readonly EPlatform Platform;
    public readonly Type EntityType;

    public EntityNotFoundOnPlatformException(string idOnPlatform, EPlatform platform, Type entityType) :
        base($"{entityType.ToString().ToCapitalized()} with ID {idOnPlatform} not found on {platform}")
    {
        IdOnPlatform = idOnPlatform;
        Platform = platform;
        EntityType = entityType;
    }
}