namespace Contracts.BLL;

public interface IAppUowContainer
{
    public Task<int> SaveChangesAsync();
}