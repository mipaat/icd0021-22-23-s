using App.DTO;

namespace App.BLL.Exceptions;

public class EntityAlreadyAddedException : Exception
{
    public readonly Entity Entity;

    public EntityAlreadyAddedException(Entity entity)
    {
        Entity = entity;
    }
}