SET QUOTED_IDENTIFIER ON;
GO

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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230302145855_InitialIdentityServerPersistedGrantDbMigration')
BEGIN
    CREATE TABLE [DeviceCodes] (
        [UserCode] nvarchar(200) NOT NULL,
        [DeviceCode] nvarchar(200) NOT NULL,
        [SubjectId] nvarchar(200) NULL,
        [SessionId] nvarchar(100) NULL,
        [ClientId] nvarchar(200) NOT NULL,
        [Description] nvarchar(200) NULL,
        [CreationTime] datetime2 NOT NULL,
        [Expiration] datetime2 NOT NULL,
        [Data] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_DeviceCodes] PRIMARY KEY ([UserCode])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230302145855_InitialIdentityServerPersistedGrantDbMigration')
BEGIN
    CREATE TABLE [Keys] (
        [Id] nvarchar(450) NOT NULL,
        [Version] int NOT NULL,
        [Created] datetime2 NOT NULL,
        [Use] nvarchar(450) NULL,
        [Algorithm] nvarchar(100) NOT NULL,
        [IsX509Certificate] bit NOT NULL,
        [DataProtected] bit NOT NULL,
        [Data] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Keys] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230302145855_InitialIdentityServerPersistedGrantDbMigration')
BEGIN
    CREATE TABLE [PersistedGrants] (
        [Id] bigint NOT NULL IDENTITY,
        [Key] nvarchar(200) NULL,
        [Type] nvarchar(50) NOT NULL,
        [SubjectId] nvarchar(200) NULL,
        [SessionId] nvarchar(100) NULL,
        [ClientId] nvarchar(200) NOT NULL,
        [Description] nvarchar(200) NULL,
        [CreationTime] datetime2 NOT NULL,
        [Expiration] datetime2 NULL,
        [ConsumedTime] datetime2 NULL,
        [Data] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_PersistedGrants] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230302145855_InitialIdentityServerPersistedGrantDbMigration')
BEGIN
    CREATE TABLE [ServerSideSessions] (
        [Id] int NOT NULL IDENTITY,
        [Key] nvarchar(100) NOT NULL,
        [Scheme] nvarchar(100) NOT NULL,
        [SubjectId] nvarchar(100) NOT NULL,
        [SessionId] nvarchar(100) NULL,
        [DisplayName] nvarchar(100) NULL,
        [Created] datetime2 NOT NULL,
        [Renewed] datetime2 NOT NULL,
        [Expires] datetime2 NULL,
        [Data] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_ServerSideSessions] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230302145855_InitialIdentityServerPersistedGrantDbMigration')
BEGIN
    CREATE UNIQUE INDEX [IX_DeviceCodes_DeviceCode] ON [DeviceCodes] ([DeviceCode]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230302145855_InitialIdentityServerPersistedGrantDbMigration')
BEGIN
    CREATE INDEX [IX_DeviceCodes_Expiration] ON [DeviceCodes] ([Expiration]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230302145855_InitialIdentityServerPersistedGrantDbMigration')
BEGIN
    CREATE INDEX [IX_Keys_Use] ON [Keys] ([Use]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230302145855_InitialIdentityServerPersistedGrantDbMigration')
BEGIN
    CREATE INDEX [IX_PersistedGrants_ConsumedTime] ON [PersistedGrants] ([ConsumedTime]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230302145855_InitialIdentityServerPersistedGrantDbMigration')
BEGIN
    CREATE INDEX [IX_PersistedGrants_Expiration] ON [PersistedGrants] ([Expiration]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230302145855_InitialIdentityServerPersistedGrantDbMigration')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_PersistedGrants_Key] ON [PersistedGrants] ([Key]) WHERE [Key] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230302145855_InitialIdentityServerPersistedGrantDbMigration')
BEGIN
    CREATE INDEX [IX_PersistedGrants_SubjectId_ClientId_Type] ON [PersistedGrants] ([SubjectId], [ClientId], [Type]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230302145855_InitialIdentityServerPersistedGrantDbMigration')
BEGIN
    CREATE INDEX [IX_PersistedGrants_SubjectId_SessionId_Type] ON [PersistedGrants] ([SubjectId], [SessionId], [Type]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230302145855_InitialIdentityServerPersistedGrantDbMigration')
BEGIN
    CREATE INDEX [IX_ServerSideSessions_DisplayName] ON [ServerSideSessions] ([DisplayName]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230302145855_InitialIdentityServerPersistedGrantDbMigration')
BEGIN
    CREATE INDEX [IX_ServerSideSessions_Expires] ON [ServerSideSessions] ([Expires]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230302145855_InitialIdentityServerPersistedGrantDbMigration')
BEGIN
    CREATE UNIQUE INDEX [IX_ServerSideSessions_Key] ON [ServerSideSessions] ([Key]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230302145855_InitialIdentityServerPersistedGrantDbMigration')
BEGIN
    CREATE INDEX [IX_ServerSideSessions_SessionId] ON [ServerSideSessions] ([SessionId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230302145855_InitialIdentityServerPersistedGrantDbMigration')
BEGIN
    CREATE INDEX [IX_ServerSideSessions_SubjectId] ON [ServerSideSessions] ([SubjectId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230302145855_InitialIdentityServerPersistedGrantDbMigration')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230302145855_InitialIdentityServerPersistedGrantDbMigration', N'7.0.3');
END;
GO

COMMIT;
GO

