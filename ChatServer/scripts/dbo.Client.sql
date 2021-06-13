CREATE TABLE [dbo].[Client] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [ClientId]  INT           NOT NULL,
    [PublicKey] VARCHAR (MAX) NOT NULL,
    [aeskey]    VARCHAR (MAX) NOT NULL,
    [aesiv]     VARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);