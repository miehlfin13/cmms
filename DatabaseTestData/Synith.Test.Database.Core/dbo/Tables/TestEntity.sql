CREATE TABLE [dbo].[TestEntities] (
    [Id]              INT          IDENTITY (1, 1) NOT NULL,
    [Status]          INT          CONSTRAINT [DF_TestEntities_Status] DEFAULT 1 NOT NULL,
    [InactiveDate]    DATETIME     NULL,
    CONSTRAINT [PK_Companies]      PRIMARY KEY CLUSTERED ([Id] ASC)
);

