CREATE TABLE [uac].[RolePermissions]
(
    [RoleId]        INT                             NOT NULL, 
    [PermissionId]  INT                             NOT NULL, 
    CONSTRAINT [PK_RolePermissions]                 PRIMARY KEY CLUSTERED ([RoleId] ASC, [PermissionId] ASC), 
    CONSTRAINT [FK_RolePermissions_Roles]           FOREIGN KEY ([RoleId]) REFERENCES [uac].[Roles]([Id]) ON UPDATE CASCADE,
    CONSTRAINT [FK_RolePermissions_Permissions]     FOREIGN KEY ([PermissionId]) REFERENCES [uac].[Permissions]([Id]) ON UPDATE CASCADE
)
