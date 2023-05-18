namespace App.Contracts.DAL;

public interface IDbInitializer
{
    public void RunDbInit(DbInitSettings config);
}