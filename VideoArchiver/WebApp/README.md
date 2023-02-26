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
`dotnet aspnet-codegenerator controller -m Author -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m AuthorCategory -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m AuthorHistory -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m AuthorPubSub -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m AuthorRating -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m AuthorSubscription -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m Category -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m Comment -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m CommentReplyNotification -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m ExternalUserToken -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m Game -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m Playlist -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m PlaylistAuthor -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m PlaylistCategory -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m PlaylistHistory -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m PlaylistRating -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m PlaylistSubscription -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m PlaylistVideo -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m PlaylistVideoPositionHistory -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m QueueItem -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m StatusChangeEvent -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m StatusChangeNotification -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m Video -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m VideoAuthor -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m VideoCategory -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m VideoGame -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m VideoHistory -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m VideoRating -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
`dotnet aspnet-codegenerator controller -m VideoUploadNotification -dc AbstractAppDbContext -udl -outDir Controllers --referenceScriptLibraries`  
