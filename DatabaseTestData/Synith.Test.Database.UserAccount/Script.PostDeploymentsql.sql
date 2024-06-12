-- ADD ROLES
MERGE INTO [uac].[Roles] [Target]
USING (
		  SELECT 'General Administrator' [Name]
	UNION SELECT 'Organization Administrator'
	UNION SELECT 'User Access Administrator'
	UNION SELECT 'Employee Administrator'
) [Source]
ON [Target].[Name] = [Source].[Name]
WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Name]) VALUES ([Source].[Name]);

DECLARE @GeneralAdmin INT = (SELECT Id FROM [uac].[Roles] WHERE [Name] = 'General Administrator')
DECLARE @OrganizationAdmin INT = (SELECT Id FROM [uac].[Roles] WHERE [Name] = 'Organization Administrator')
DECLARE @UserAccessAdmin INT = (SELECT Id FROM [uac].[Roles] WHERE [Name] = 'User Access Administrator')
DECLARE @EmployeeAdmin INT = (SELECT Id FROM [uac].[Roles] WHERE [Name] = 'Employee Administrator')

DECLARE @OrganizationModule INT = 1
DECLARE @UserAccessModule INT = 2
DECLARE @EmployeeModule INT = 3

-- ADD ROLE PERMISSIONS
MERGE INTO [uac].[RolePermissions] [Target]
USING (
	SELECT
		  R.Id [RoleId]
		, P.Id [PermissionId]
	FROM [uac].[Roles] R
		INNER JOIN [uac].[Permissions] P ON
				R.Id = @GeneralAdmin
			OR (R.Id = @OrganizationAdmin AND P.ModuleId = @OrganizationModule)
			OR (R.Id = @UserAccessAdmin AND P.ModuleId = @UserAccessModule)
			OR (R.Id = @EmployeeAdmin AND P.ModuleId = @EmployeeModule)
) [Source]
ON [Target].RoleId = [Source].RoleId AND [Target].PermissionId = [Source].PermissionId
WHEN NOT MATCHED BY TARGET THEN
	INSERT (RoleId, PermissionId) VALUES (Source.RoleId, Source.PermissionId);

DECLARE @EnglishId INT = (SELECT Id FROM [uac].[Languages] WHERE Code = 'US')

-- ADD USER
MERGE INTO [uac].[Users] [Target]
USING (
	SELECT
		  'CWrUkLj81eGunQjv5kyYpg==' [Username] -- admin
		, '$2a$12$x6dmZBI7HTlG2IbAAEiNzef4kbu467Fc9h8nitFpCab6p/fRyleou' [Password] -- admin
		, 'ux+YZAssjRAkslresCnu5we39v3ACcTHiiVj0JrhjI4=' [Email] -- admin@synith.com
) [Source]
ON [Target].Username = [Source].Username AND [Target].Email = [Source].Email
WHEN NOT MATCHED BY TARGET THEN
	INSERT (Username, [Password], Email, [Status], LanguageId)
	VALUES ([Source].Username, [Source].[Password], [Source].Email, 1, @EnglishId);

DECLARE @AdminUser INT = (SELECT Id FROM [uac].[Users] WHERE Username = 'CWrUkLj81eGunQjv5kyYpg==')

-- ADD USER ROLE
MERGE INTO [uac].[UserRoles] [Target]
USING (
	SELECT @AdminUser [UserId], @GeneralAdmin [RoleId]
) [Source]
ON [Target].UserId = [Source].UserId AND [Target].RoleId = [Source].RoleId
WHEN NOT MATCHED BY TARGET THEN
	INSERT (UserId, RoleId) VALUES ([Source].UserId, [Source].RoleId);
