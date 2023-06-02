namespace App.DAL.Contracts;

public interface IDbInitializer
{
    public void RunDbInit(DbInitSettings config);
}