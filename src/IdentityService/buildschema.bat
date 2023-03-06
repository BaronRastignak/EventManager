rmdir /S /Q "Data/Migrations"

dotnet ef migrations add Users -c ApplicationDbContext -o Data/Migrations
dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations --no-build
dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations --no-build

dotnet ef migrations script -i -c ApplicationDbContext -o Data/Migrations/Scripts/Users.sql
dotnet ef migrations script -i -c PersistedGrantDbContext -o Data/Migrations/Scripts/InitialIdentityServerPersistedGrantDbMigration.sql
dotnet ef migrations script -i -c ConfigurationDbContext -o Data/Migrations/Scripts/InitialIdentityServerConfigurationDbMigration.sql
