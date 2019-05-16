@ECHO OFF

echo  ====================
echo    OpenCatapult API
echo  ====================
echo.

certutil -addstore -f -enterprise -v root "certs\opencatapultlocal.cer"

cd api
dotnet exec --depsfile "ocapi.deps.json" --runtimeconfig "ocapi.runtimeconfig.json" "..\tools\ef.dll" database update --assembly "Polyrific.Catapult.Api.Data.dll" --startup-assembly "ocapi.dll" --verbose --context CatapultSqliteDbContext
ocapi.exe --urls "http://localhost:8005;https://localhost:44305"