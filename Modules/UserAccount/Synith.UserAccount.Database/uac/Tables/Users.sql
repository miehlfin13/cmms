CREATE TABLE [uac].[Users] (
    [Id]           INT              IDENTITY (1, 1) NOT NULL,
    [Username]     VARCHAR (200)    NOT NULL,
    [Password]     VARCHAR (200)    NOT NULL,
    [Email]        VARCHAR (200)    NOT NULL,
    [Status]       INT              CONSTRAINT [DF_Users_Status] DEFAULT 2 NOT NULL,
    [CreatedDate]  DATETIME         CONSTRAINT [DF_Users_CreatedDate] DEFAULT GETUTCDATE() NOT NULL,
    [LastLogin]    DATETIME         NULL,
    [InactiveDate] DATETIME         NULL,
    [LanguageId]   INT              NOT NULL, 
    [RetryCount]   INT              CONSTRAINT [DF_Users_RetryCount] DEFAULT 0 NOT NULL, 
    CONSTRAINT [PK_Users]           PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UK_Users_Username]  UNIQUE NONCLUSTERED ([Username]), 
    CONSTRAINT [UK_Users_Email]     UNIQUE NONCLUSTERED ([Email]), 
    CONSTRAINT [FK_Users_Languages] FOREIGN KEY ([LanguageId]) REFERENCES [uac].[Languages]([Id]) ON UPDATE CASCADE,
);

