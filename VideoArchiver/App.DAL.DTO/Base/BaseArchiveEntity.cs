namespace App.DAL.DTO.Base;

public abstract class BaseArchiveEntity : BaseArchiveEntityNonMonitored
{
    public bool Monitor { get; set; } = true;
    public bool Download { get; set; } = true;
}