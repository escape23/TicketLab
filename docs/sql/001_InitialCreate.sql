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
CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Email] nvarchar(200) NOT NULL,
    [Role] nvarchar(20) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

CREATE TABLE [Tickets] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(200) NOT NULL,
    [Description] nvarchar(4000) NOT NULL,
    [Priority] nvarchar(20) NOT NULL,
    [Category] nvarchar(20) NOT NULL,
    [Status] nvarchar(20) NOT NULL,
    [CreatedById] int NOT NULL,
    [AssignedToId] int NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Tickets] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Tickets_Users_AssignedToId] FOREIGN KEY ([AssignedToId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Tickets_Users_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Comments] (
    [Id] int NOT NULL IDENTITY,
    [Text] nvarchar(2000) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [TicketId] int NOT NULL,
    [AuthorId] int NOT NULL,
    CONSTRAINT [PK_Comments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Comments_Tickets_TicketId] FOREIGN KEY ([TicketId]) REFERENCES [Tickets] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Comments_Users_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [TicketHistory] (
    [Id] int NOT NULL IDENTITY,
    [Field] nvarchar(50) NOT NULL,
    [OldValue] nvarchar(200) NULL,
    [NewValue] nvarchar(200) NULL,
    [ChangedAt] datetime2 NOT NULL,
    [TicketId] int NOT NULL,
    [ChangedById] int NOT NULL,
    CONSTRAINT [PK_TicketHistory] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TicketHistory_Tickets_TicketId] FOREIGN KEY ([TicketId]) REFERENCES [Tickets] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_TicketHistory_Users_ChangedById] FOREIGN KEY ([ChangedById]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Email', N'Name', N'Role') AND [object_id] = OBJECT_ID(N'[Users]'))
    SET IDENTITY_INSERT [Users] ON;
INSERT INTO [Users] ([Id], [Email], [Name], [Role])
VALUES (1, N'anna@ticketlab.local', N'Anna User', N'User'),
(2, N'ola@ticketlab.local', N'Ola Developer', N'Developer'),
(3, N'kari@ticketlab.local', N'Kari Admin', N'Admin');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Email', N'Name', N'Role') AND [object_id] = OBJECT_ID(N'[Users]'))
    SET IDENTITY_INSERT [Users] OFF;

CREATE INDEX [IX_Comments_AuthorId] ON [Comments] ([AuthorId]);

CREATE INDEX [IX_Comments_TicketId] ON [Comments] ([TicketId]);

CREATE INDEX [IX_TicketHistory_ChangedById] ON [TicketHistory] ([ChangedById]);

CREATE INDEX [IX_TicketHistory_TicketId] ON [TicketHistory] ([TicketId]);

CREATE INDEX [IX_Tickets_AssignedToId] ON [Tickets] ([AssignedToId]);

CREATE INDEX [IX_Tickets_CreatedById] ON [Tickets] ([CreatedById]);

CREATE INDEX [IX_Tickets_Status] ON [Tickets] ([Status]);

CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261001100115_InitialCreate', N'10.0.12');

COMMIT;
GO

