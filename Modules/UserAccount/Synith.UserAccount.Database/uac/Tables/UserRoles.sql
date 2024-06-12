CREATE TABLE [uac].[UserRoles]
(
    [UserId]    INT                     NOT NULL, 
    [RoleId]    INT                     NOT NULL, 
    CONSTRAINT [PK_UserRoles]           PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC), 
    CONSTRAINT [FK_UserRoles_Users]     FOREIGN KEY ([UserId]) REFERENCES [uac].[Users]([Id]) ON UPDATE CASCADE,
    CONSTRAINT [FK_UserRoles_Roles]     FOREIGN KEY ([RoleId]) REFERENCES [uac].[Roles]([Id]) ON UPDATE CASCADE,
)
