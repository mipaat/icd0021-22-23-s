#pragma warning disable 1591
namespace WebApp.Config;

public class DataInitializationSettings
{
    public const string SectionKey = "DataInitialization";

    public bool DropDatabase { get; set; }
    public bool MigrateDatabase { get; set; } = true;
    public bool SeedIdentity { get; set; } = true;
    public bool SeedAppData { get; set; } = true;
    public bool SeedDemoIdentity { get; set; }
}