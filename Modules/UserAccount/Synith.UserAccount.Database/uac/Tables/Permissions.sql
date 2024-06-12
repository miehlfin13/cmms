CREATE TABLE [uac].[Permissions]
(
	[Id]        INT                     IDENTITY (1, 1) NOT NULL,
    [Code]      VARCHAR(50)             NOT NULL, 
    [ModuleId]  INT                     NOT NULL, 
    [ParentId]  INT                     NULL,
    [SortIndex]	INT						CONSTRAINT [DF_Roles_SortIndex] DEFAULT 0 NOT NULL,
    CONSTRAINT [PK_Permissions]         PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Permissions_Modules] FOREIGN KEY ([ModuleId]) REFERENCES [uac].[Modules]([Id]) ON UPDATE CASCADE,
    CONSTRAINT [FK_Permissions_Self]    FOREIGN KEY ([ParentId]) REFERENCES [uac].[Permissions]([Id]),
    CONSTRAINT [UK_Permissions_Code]    UNIQUE NONCLUSTERED ([Code])
)
