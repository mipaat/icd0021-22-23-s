using System.Text;
using App.Domain.Enums;
using Utils;

namespace App.BLL.Exceptions;

public class EntityNotFoundInArchiveException : Exception
{
    public readonly Guid? Id;
    public readonly string? IdOnPlatform;
    public readonly Platform? Platform;
    public readonly Type EntityType;

    public EntityNotFoundInArchiveException(Guid id, Type entityType) : base(GetMessage(id: id, entityType: entityType))
    {
        Id = id;
        EntityType = entityType;
    }

    public EntityNotFoundInArchiveException(string idOnPlatform, Platform platform, Type entityType) :
        base(GetMessage(idOnPlatform: idOnPlatform, platform: platform, entityType: entityType))
    {
        IdOnPlatform = idOnPlatform;
        Platform = platform;
        EntityType = entityType;
    }

    public EntityNotFoundInArchiveException(Guid id, string idOnPlatform, Platform platform, Type entityType) :
        base(GetMessage(id: id, idOnPlatform: idOnPlatform, platform: platform, entityType: entityType))
    {
        Id = id;
        IdOnPlatform = idOnPlatform;
        Platform = platform;
        EntityType = entityType;
    }

    private static string GetMessage(Type entityType, Guid? id = null, string? idOnPlatform = null,
        Platform? platform = null)
    {
        var builder = new StringBuilder(entityType.ToString().ToCapitalized());
        var identifiers = new List<string>();

        if (id != null)
        {
            identifiers.Add($"with ID: '{id}'");
        }

        if (idOnPlatform != null)
        {
            identifiers.Add($"with ID on platform: '{idOnPlatform}'");
        }

        if (platform != null)
        {
            identifiers.Add($"on platform {platform}");
        }

        if (identifiers.Count > 0)
        {
            builder.Append(" (");
            builder.AppendJoin(", ", identifiers);
            builder.Append(')');
        }

        builder.Append(" not found in archive");

        return builder.ToString();
    }
}