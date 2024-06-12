CREATE PROCEDURE [uac].[User_UpdateUserRoles]
	  @UserId	INT
	, @RoleIds	VARCHAR(MAX)
AS
BEGIN

	SET @RoleIds = ISNULL(@RoleIds, '')

	IF @RoleIds = ''
	BEGIN
		DELETE FROM [uac].[UserRoles]
		WHERE UserId = @UserId
	END
	ELSE
	BEGIN
		MERGE INTO [uac].[UserRoles] [Target]
		USING (
			SELECT
				[Value] [RoleId]
			FROM STRING_SPLIT(@RoleIds, ',')
		) [Source]
		ON [Target].UserId = @UserId AND [Target].RoleId = [Source].RoleId
		WHEN NOT MATCHED BY TARGET THEN
			INSERT (UserId, RoleId) VALUES (@UserId, [Source].RoleId)
		WHEN NOT MATCHED BY SOURCE AND [Target].UserId = @UserId THEN
			DELETE;
	END

END
