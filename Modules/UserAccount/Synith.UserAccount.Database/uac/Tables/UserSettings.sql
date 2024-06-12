CREATE TABLE [uac].[UserSettings]
(
	[UserId]							INT				NOT NULL,
	[Name]								VARCHAR(50) 	NOT NULL,
	[Value]								VARCHAR(250)	NOT NULL,
    CONSTRAINT [PK_UserSettings]		PRIMARY KEY ([UserId] ASC, [Name] ASC), 
    CONSTRAINT [FK_UserSettings_Users]	FOREIGN KEY ([UserId]) REFERENCES [uac].[Users]([Id])
)
