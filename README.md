# star-wars-filter-app
A simple WPF MVVM desktop app to search and filter Star Wars characters stored in a MySQL database.

## Features
- Search Star Wars characters by name and other fields
- Filter results (by species, homeworld, affiliation, etc.)
- MVVM architecture for easy maintenance and testing
- MySQL-backed data storage

## Requirements
- Windows (WPF desktop app)
- Visual Studio (2019 / 2022) with .NET desktop development workload, or the appropriate .NET SDK if the project targets .NET Core / .NET 5+
- MySQL server (local or remote)
- MySQL Connector/NET (MySql.Data) if not already referenced in the project

## Quickstart — Run locally
- Clone the repo: git clone https://github.com/scarlet-enlight/star-wars-filter-app.git
- Open the solution in Visual Studio (the .sln file) and build.
- Create the MySQL database (example below).
- Configure the app's MySQL connection string (see Configuration).
- Run the app from Visual Studio (F5) or by launching the built executable.
- Database — example schema & seed
- Create a database named starwars and a simple Characters table. Example SQL:
```sql
CREATE DATABASE IF NOT EXISTS starwars; USE starwars;

CREATE TABLE IF NOT EXISTS Characters ( Id INT AUTO_INCREMENT PRIMARY KEY, Name VARCHAR(255) NOT NULL, Species VARCHAR(100), Homeworld VARCHAR(100), Affiliation VARCHAR(100), Notes TEXT );

INSERT INTO Characters (Name, Species, Homeworld, Affiliation, Notes) VALUES ('Luke Skywalker', 'Human', 'Tatooine', 'Rebel Alliance', 'Jedi Knight'), ('Darth Vader', 'Human', 'Tatooine', 'Galactic Empire', 'Sith Lord'), ('Leia Organa', 'Human', 'Alderaan', 'Rebel Alliance', 'Princess');
```
Adjust fields as needed to match the app's model.

## Configuration
The app needs a MySQL connection string. Add or update it in your configuration (for example, App.config or an appropriate settings file used by the project). Example connection string for MySQL:
```
Server=localhost;Port=3306;Database=starwars;Uid=root;Pwd=your_password;
```

Make sure the connection string name matches what the project expects (check the project's config or data-access code and update accordingly).

If you prefer Docker for MySQL, run:
```bash
docker run --name mysql-sw -e MYSQL_ROOT_PASSWORD=example -e MYSQL_DATABASE=starwars -p 3306:3306 -d mysql:8
```
Then use the same connection string with Pwd=example.

## Building (CLI)
If the project is SDK-style: dotnet build dotnet run --project path/to/YourApp.csproj

Otherwise, open and run in Visual Studio.

## Contributing
- Fork the repository and open a PR for changes.
- Keep UI and data access changes separate where possible (MVVM pattern).
- Add tests where applicable and document any DB schema changes.
## Troubleshooting
- Connection errors: verify MySQL server is running and the connection string credentials/port are correct.
- Missing MySQL provider: install MySql.Data via NuGet.
- If the app expects a different table/schema, inspect the model/data-access code and update the DB accordingly.
## License
MIT License — see LICENSE file for full text.

Copyright (c) 2025 scarlet-enlight
