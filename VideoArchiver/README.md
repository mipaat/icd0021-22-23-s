# Running and configuration
## If not using docker
Enter `WebApp` directory.  
Rename/copy `appsettings.example.json` to `appsettings.json` (or `appsettings.Environment.json`).  
Fill in the "DefaultConnection" connection string with details for the PostgreSQL database that you want to use.  
Change other settings as required.

### If only using docker for DB
Run `docker compose up db` in solution root directory to run DB.  
In this case, the default DB connection string in `appsettings.example.json` should not be changed.

### Regarding yt-dlp
This app uses yt-dlp to fetch information and download media from YouTube. **The yt-dlp tool requires python3 to work.**

## If using docker (compose)
Make sure a `.env` file containing necessary configuration options exists in the solution root directory (same directory as `docker-compose.yml`).  

**NB! The DB connection string configured in the .env file gets ignored in `docker-compose.yml` in order to properly connect to the DB container.**

The WebApp should be accessible via `localhost://8080`

### Easy ways to create the `.env` file
* Run `./appsettings_to_env.sh > .env` to generate a `.env` file based on `WebApp/appsettings.json`.
* Run `./appsettings_to_env.sh path/to/settings_file.json > .env` to generate a `.env` file based on a different json file.
* Copy and modify `example.env`

(The parse errors printed by the `appsettings_to_env.sh` script can probably be ignored)

### Running using docker compose
* Start everything: `docker compose up`
* Start only DB: `docker compose up db`
* Start only WebApp: `docker compose up webapp`
* Re-build `docker compose build`

# Migrations
(In solution root directory - VideoArchiver)  
`dotnet ef migrations add --context PostgresAppDbContext MigrationName --project App.DAL.EF --startup-project WebApp`  
`dotnet ef migrations remove --context PostgresAppDbContext --project App.DAL.EF --startup-project WebApp`  
`dotnet ef database update --context PostgresAppDbContext --project App.DAL.EF --startup-project WebApp`  
`dotnet ef database update --context PostgresAppDbContext MigrationName --project App.DAL.EF --startup-project WebApp`  
