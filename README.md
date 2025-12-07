To make this run without complicated setup. I've setup the project to use a docker container for both the SQL (database) and game instance.

First you need to download and install [Docker Desktop](https://docs.docker.com/desktop/setup/install/windows-install/),

In the install process there would be an option "use WSL 2 instead of Hyper-V", check that. It may be necessary to restart your computer, do so. After installation open the project directory in command prompt. In simple terms, navigate to the project folder via file explorer and then right click in the folder without selecting anything and chose "open in terminal."

In the terminal run:
```powershell
docker compose up --build -d
```
This would take a considerable amount of time in the first run, be patient. If there's an issue like the following:

```powershell
D:\Coding\Project\Morgan Thieves  docker compose up --build -d

  unable to get image 'mcr.microsoft.com/mssql/server:2022-latest': request returned 500 Internal Server Error for API route and version http://%2F%2F.%2Fpipe%2FdockerDesktopLinuxEngine/v1.51/images/mcr.microsoft.com/mssql/server:2022-latest/json, check if the server supports the requested API version 
```

Go to system tray locate the docker icon "the whale", and then quit desktop. Then search docker in the start menu to run. Then run the command.

The following prompt means Successful Setup:
```powershell
[+] Running 3/3
 ✔ morganthieves-game              Built                                                                                                                                               0.0s
 ✔ Container sql_server_container  Healthy                                                                                                                                             1.0s
 ✔ Container pirate_game           Started                                                                                                                                             1.2s
``` 

Execute the following to run the actual game:
```powershell
docker attach pirate_game
```

If there's no problem this should bring you to the game click any button 1 time to actually view the loading screen. Don't double click because that would skip the loading screen.

## Microsoft Server

Prerequisite of the following instruction is that the previous should be successful.

Open a new terminal. Execute the following command.

```powershell
docker exec -it sql_server_container /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'PirateKing123!' -C
```
If successful that should turn the prompt to the following:
```sql
1>
```
To list the database.

```sql
SELECT name FROM sys.databases;
GO
```
Then use the actual db.

```sql
USE PirateGameDB;
GO
```
All sql query should work as intended  that after running it by pressing enter. Follow it with "GO" to actually run. To exit:

```sql
quit
```
To rerun the game start from the `docker compose up --build -d` command.

