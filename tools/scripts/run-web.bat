@ECHO OFF

echo  ===================================
echo    OpenCatapult Web User Interface
echo  ===================================
echo.

cd web
ocweb.exe --urls "http://localhost:8000;https://localhost:44300"