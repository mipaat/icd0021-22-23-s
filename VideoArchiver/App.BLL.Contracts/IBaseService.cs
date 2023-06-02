namespace App.BLL.Contracts;

public interface IBaseService
{
    public IServiceUow ServiceUow { get; }
}