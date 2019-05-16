:: Please put this file in the same directory as the ocapi.exe file

@ECHO OFF

IF "%~1"=="/u" (CALL :Uninstall) ELSE (CALL :Install)
EXIT /B %ERRORLEVEL%

:Install
SET name="OpenCatapult API"
SET desc="Service for OpenCatapult API"
SET binpath="%~dp0ocapi.exe --service --urls \"http://localhost:8005\""

sc create ocapi DisplayName= %name% binPath= %binPath% start= auto
sc start ocapi
EXIT /B 0

:Uninstall
sc stop ocapi
sc delete ocapi
EXIT /B 0