IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230302070542_InitialCreate')
BEGIN
    IF SCHEMA_ID(N'event_manager') IS NULL EXEC(N'CREATE SCHEMA [event_manager];');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230302070542_InitialCreate')
BEGIN
    CREATE TABLE [event_manager].[social_events] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(300) NOT NULL,
        [Date] datetime2 NOT NULL,
        CONSTRAINT [PK_social_events] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230302070542_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230302070542_InitialCreate', N'7.0.3');
END;
GO

COMMIT;
GO

