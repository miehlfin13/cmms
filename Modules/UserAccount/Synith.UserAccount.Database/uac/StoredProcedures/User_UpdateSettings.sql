CREATE PROCEDURE [uac].[User_UpdateSettings]
	  @UserId	INT
	, @Name		VARCHAR(50)
	, @Value	VARCHAR(250)
AS
BEGIN

	IF EXISTS (SELECT 1 FROM [uac].[UserSettings] WHERE UserId = @UserId AND [Name] = @Name)
	BEGIN
		UPDATE [uac].[UserSettings]
		SET [Value] = @Value
		WHERE UserId = @UserId
			AND [Name] = @Name
	END
	ELSE
	BEGIN
		INSERT INTO [uac].[UserSettings] (UserId, [Name], [Value])
		VALUES (@UserId, @Name, @Value)
	END

END
