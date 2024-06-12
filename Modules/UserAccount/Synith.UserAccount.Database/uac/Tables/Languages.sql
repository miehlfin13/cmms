CREATE TABLE [uac].[Languages]
(
	[Id]            INT                     IDENTITY (1, 1) NOT NULL, 
    [Code]          VARCHAR(5)             NOT NULL, 
    [Name] VARCHAR(20) NOT NULL, 
    [LocaleName] NVARCHAR(20) NOT NULL, 
    CONSTRAINT [PK_Languages]               PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT [UK_Languages_LanguageCode]  UNIQUE NONCLUSTERED ([Code])
)
