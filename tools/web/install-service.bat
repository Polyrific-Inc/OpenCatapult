:: Please put this file in the same directory as the ocweb.exe file

@ECHO OFF

IF "%~1"=="/u" (CALL :Uninstall) ELSE (CALL :Install)
EXIT /B %ERRORLEVEL%

:Install
SETLOCAL
    SET ASPNETCORE_ENVIRONMENT=Development && DIR
ENDLOCAL

SET name="OpenCatapult Web"
SET desc="Service for OpenCatapult Web"
SET binpath="%~dp0ocweb.exe --service --urls \"http://localhost:8000\""

sc create ocweb DisplayName= %name% binPath= %binPath% start= auto
sc start ocweb
EXIT /B 0

:Uninstall
sc stop ocweb
sc delete ocweb
EXIT /B 0 