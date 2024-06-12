@ECHO off

SET module=%1
SET configuration=%2

ECHO Generating %module% client api.

SET moduleFile=%~dp0Synith.CMMS\bin\%configuration%\net8.0\Synith.CMMS.dll
SET openApiPath=%~dp0Synith.Client\src\app\openapi
SET specPath=%openApiPath%\specs
SET specFile=%specPath%\%module%.json

ECHO ModuleFile: %moduleFile%
ECHO OpenApiPath: %openApiPath%

IF NOT EXIST "%openApiPath%" (
	ECHO Creating OpenApi directory.
	MKDIR "%openApiPath%"
) ELSE (
	ECHO OpenApi directory found.
)

ECHO Generating specs file.
ECHO SpecFile: %specFile%.

IF NOT EXIST "%specPath%" (
	ECHO Creating spec directory.
	MKDIR "%specPath%"
) ELSE (
	ECHO Spec directory found.
)

IF EXIST "%specFile" (
	ECHO Removing old spec file.
	DEL "%specFile%"
)

swagger tofile --output "%specFile%" "%moduleFile%" v1

IF NOT EXIST "%specFile%" (
	ECHO Failed to generate spec file.
	GOTO :end
)

ECHO Generating typescript client api.
openapi-generator-cli generate -g typescript-angular -i "%specFile%" -o "%openApiPath%" --skip-validate-spec
ECHO Successfully generated %module% client api.

:end
