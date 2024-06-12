CREATE PROCEDURE [uac].[Role_UpdatePermission]
	  @Code			VARCHAR(50)
	, @ModuleId		INT
	, @ParentCode	VARCHAR(50)
	, @SortIndex	INT = 0
AS
BEGIN
	
	DECLARE @ParentId INT
	IF @ParentCode <> ''
	BEGIN
		SET @ParentId = (SELECT Id FROM [Permissions] WHERE Code = @ParentCode)
	END

	SET @SortIndex = ISNULL(@SortIndex, 0)

	IF EXISTS(SELECT 0 FROM [uac].[Permissions] WHERE ModuleId = @ModuleId AND Code = @Code)
	BEGIN
		UPDATE [uac].[Permissions]
		SET ParentId = @ParentId
		WHERE ModuleId = @ModuleId
			AND Code = @Code
			AND SortIndex = @SortIndex
	END
	ELSE
	BEGIN
		INSERT INTO [uac].[Permissions] (Code, ModuleId, ParentId, SortIndex)
		VALUES (@Code, @ModuleId, @ParentId, @SortIndex)
	END

END
