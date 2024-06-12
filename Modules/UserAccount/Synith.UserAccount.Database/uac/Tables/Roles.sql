CREATE TABLE [uac].[Roles]
(
	[Id]            INT         IDENTITY (1, 1) NOT NULL, 
    [Name]          VARCHAR(50) NOT NULL, 
    [Status]        INT         CONSTRAINT [DF_Roles_Status] DEFAULT 1 NOT NULL, 
    [InactiveDate]  DATETIME    NULL,
    CONSTRAINT [PK_Roles]       PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [UK_Roles_Name]  UNIQUE NONCLUSTERED ([Name])
)
