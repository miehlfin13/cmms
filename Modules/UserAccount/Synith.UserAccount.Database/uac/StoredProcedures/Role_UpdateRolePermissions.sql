CREATE PROCEDURE [uac].[Role_UpdateRolePermissions]
	  @RoleId			INT
	, @PermissionIds	VARCHAR(MAX)
AS
BEGIN

	SET @PermissionIds = ISNULL(@PermissionIds, '')

	IF @PermissionIds = ''
	BEGIN
		DELETE FROM [uac].[RolePermissions]
		WHERE RoleId = @RoleId
	END
	ELSE
	BEGIN
		MERGE INTO [uac].[RolePermissions] [Target]
		USING (
			SELECT
				[Value] [PermissionId]
			FROM STRING_SPLIT(@PermissionIds, ',')
		) [Source]
		ON [Target].RoleId = @RoleId AND [Target].PermissionId = [Source].PermissionId
		WHEN NOT MATCHED BY TARGET THEN
			INSERT (RoleId, PermissionId) VALUES (@RoleId, [Source].PermissionId)
		WHEN NOT MATCHED BY SOURCE AND [Target].RoleId = @RoleId THEN
			DELETE;
	END

END
