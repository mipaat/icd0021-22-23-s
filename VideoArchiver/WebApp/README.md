## Configuration
Rename/copy `appsettings.example.json` to `appsettings.json` (or `appsettings.Environment.json`)
  
Fill in the "DefaultConnection" connection string with details for the PostgreSQL database that you want to use.  
Or, alternatively, remove that option entirely to use a local SQLite database.

## Migrations
(In solution root directory - VideoArchiver)  
`dotnet ef migrations add --context DbContextClassName MigrationName --project DAL --startup-project WebApp`  
`dotnet ef migrations remove --context DbContextClassName --project DAL --startup-project WebApp`  
`dotnet ef database update --context DbContextClassName --project DAL --startup-project WebApp`  
`dotnet ef database update --context DbContextClassName MigrationName --project DAL --startup-project WebApp`  

## ASP.NET DB Scaffolding
(In WebApp directory)  
`dotnet aspnet-codegenerator controller -m Entity -dc ApplicationDbContext -udl -outDir Controllers --referenceScriptLibraries`
