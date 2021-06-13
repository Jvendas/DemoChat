CREATE TABLE [dbo].[Users] (
    [Id]                 INT         IDENTITY (1, 1) NOT NULL,
    [Username]           NCHAR (10)  NOT NULL,
    [SaltedPasswordHash] BINARY (32) NOT NULL,
    [Salt]               BINARY (8)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);