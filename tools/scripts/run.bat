:: This script will run the OpenCatapult components locally
::
:: Usage: run.bat [api | cli | engine | web]
::        (if the component is not given, it will run all components)
::
:: Note: This batch script should be put in the root folder, outside the component folders

@ECHO OFF

SET runapi=0
SET runcli=0
SET runengine=0
SET runweb=0

if [%1]==[] (
    SET runapi=1
    SET runcli=1
    SET runengine=1
    SET runweb=1
)
if "%~1"=="api" SET runapi=1
if "%~1"=="cli" SET runcli=1
if "%~1"=="engine" SET runengine=1
if "%~1"=="web" SET runweb=1

if %runapi%==1 CALL :Run_Api
if %runcli%==1 CALL :Run_Cli
if %runengine%==1 CALL :Run_Engine
if %runweb%==1 CALL :Run_Web
EXIT /B %ERRORLEVEL%

:Run_Api
ECHO Running API
START cmd.exe /k run-api.bat
EXIT /B 0

:Run_Cli
ECHO Running CLI
START cmd.exe /k run-cli.bat
EXIT /B 0

:Run_Engine
ECHO Running Engine
START cmd.exe /k run-engine.bat
EXIT /B 0

:Run_Web
ECHO Running Web
START cmd.exe /k run-web.bat
EXIT /B 0