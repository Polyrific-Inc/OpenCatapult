@ECHO OFF

echo  ====================
echo    OpenCatapult API
echo  ====================
echo.

certutil -addstore -f -enterprise -v root "certs\opencatapultlocal.cer"

cd api
ocapi.exe --urls "http://localhost:8005;https://localhost:44305"