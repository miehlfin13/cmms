-- ADD LANGUAGES
MERGE INTO [uac].[Languages] [Target]
USING (
		  SELECT 'US' [Code], 'English - US' [Name], 'English - US' [LocaleName]
	UNION SELECT 'JP', 'Japanese', N'日本語'
	UNION SELECT 'NL', 'Dutch', 'Nederlands'
	UNION SELECT 'DE', 'German', 'Deutsch'
) [Source]
ON [Target].Code = [Source].Code
WHEN NOT MATCHED BY TARGET THEN
	INSERT (Code, [Name], LocaleName) VALUES ([Source].Code, [Source].[Name], [Source].LocaleName);

-- ADD MODULES
DECLARE @OrganizationModule INT = 1
DECLARE @UserAccessModule INT = 2
DECLARE @EmployeeModule INT = 3

SET IDENTITY_INSERT [uac].[Modules] ON
MERGE INTO [uac].[Modules] [Target]
USING (
		  SELECT @OrganizationModule [Id], 'ORGANIZATION' [Code]
	UNION SELECT @UserAccessModule, 'USERACCESS'
	UNION SELECT @EmployeeModule, 'EMPLOYEE'
) [Source]
ON [Target].Id = [Source].Id
WHEN MATCHED AND [Target].Code <> [Source].[Code] THEN
	UPDATE SET [Target].Code = [Source].[Code]
WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id], Code) VALUES ([Source].Id, [Source].[Code]);

SET IDENTITY_INSERT [uac].[Modules] OFF

-- ADD PERMISSIONS
EXEC [uac].[Role_UpdatePermission] 'COMPANY_VIEW', @OrganizationModule, '', 1
EXEC [uac].[Role_UpdatePermission] 'COMPANY_ADD', @OrganizationModule, 'COMPANY_VIEW', 2
EXEC [uac].[Role_UpdatePermission] 'COMPANY_EDIT', @OrganizationModule, 'COMPANY_VIEW', 3
EXEC [uac].[Role_UpdatePermission] 'COMPANY_DEACTIVATE', @OrganizationModule, 'COMPANY_VIEW', 4
EXEC [uac].[Role_UpdatePermission] 'AREA_VIEW', @OrganizationModule, '', 11
EXEC [uac].[Role_UpdatePermission] 'AREA_ADD', @OrganizationModule, 'AREA_VIEW', 12
EXEC [uac].[Role_UpdatePermission] 'AREA_EDIT', @OrganizationModule, 'AREA_VIEW', 13
EXEC [uac].[Role_UpdatePermission] 'AREA_DEACTIVATE', @OrganizationModule, 'AREA_VIEW', 14
EXEC [uac].[Role_UpdatePermission] 'BRANCH_VIEW', @OrganizationModule, '', 21
EXEC [uac].[Role_UpdatePermission] 'BRANCH_ADD', @OrganizationModule, 'BRANCH_VIEW', 22
EXEC [uac].[Role_UpdatePermission] 'BRANCH_EDIT', @OrganizationModule, 'BRANCH_VIEW', 23
EXEC [uac].[Role_UpdatePermission] 'BRANCH_DEACTIVATE', @OrganizationModule, 'BRANCH_VIEW', 24

EXEC [uac].[Role_UpdatePermission] 'ROLE_VIEW', @UserAccessModule, '', 1
EXEC [uac].[Role_UpdatePermission] 'ROLE_ADD', @UserAccessModule, 'ROLE_VIEW', 2
EXEC [uac].[Role_UpdatePermission] 'ROLE_EDIT', @UserAccessModule, 'ROLE_VIEW', 3
EXEC [uac].[Role_UpdatePermission] 'ROLE_DEACTIVATE', @UserAccessModule, 'ROLE_VIEW', 4
EXEC [uac].[Role_UpdatePermission] 'USER_VIEW', @UserAccessModule, '', 11
EXEC [uac].[Role_UpdatePermission] 'USER_ADD', @UserAccessModule, 'USER_VIEW', 12
EXEC [uac].[Role_UpdatePermission] 'USER_EDIT', @UserAccessModule, 'USER_VIEW', 13
EXEC [uac].[Role_UpdatePermission] 'USER_DEACTIVATE', @UserAccessModule, 'USER_VIEW', 14

EXEC [uac].[Role_UpdatePermission] 'EMPLOYEE_VIEW', @EmployeeModule, '', 1
EXEC [uac].[Role_UpdatePermission] 'EMPLOYEE_ADD', @EmployeeModule, 'EMPLOYEE_VIEW', 2
EXEC [uac].[Role_UpdatePermission] 'EMPLOYEE_EDIT', @EmployeeModule, 'EMPLOYEE_VIEW', 3
EXEC [uac].[Role_UpdatePermission] 'EMPLOYEE_DEACTIVATE', @EmployeeModule, 'EMPLOYEE_VIEW', 4
