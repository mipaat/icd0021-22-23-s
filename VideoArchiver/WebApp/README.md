## Configuration
Rename/copy `appsettings.example.json` to `appsettings.json` (or `appsettings.Environment.json`)
  
Fill in the "DefaultConnection" connection string with details for the PostgreSQL database that you want to use.  

Or, alternatively, remove that option entirely to use a local SQLite database.  
(The default path for the SQLite database can be printed out by running `dotnet run --project -- PrintSqlitePath` in the solution root directory or `dotnet run -- PrintSqlitePath` in the WebApp project directory. The argument `PrintSqlitePath` is case-insensitive.)

## Migrations
(In solution root directory - VideoArchiver)  
`dotnet ef migrations add --context DbContextClassName MigrationName --project DAL --startup-project WebApp`  
`dotnet ef migrations remove --context DbContextClassName --project DAL --startup-project WebApp`  
`dotnet ef database update --context DbContextClassName --project DAL --startup-project WebApp`  
`dotnet ef database update --context DbContextClassName MigrationName --project DAL --startup-project WebApp`  

## ASP.NET DB Scaffolding
(In WebApp directory)  
`dotnet aspnet-codegenerator controller -m Entity -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`
