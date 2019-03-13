$host.UI.RawUI.WindowTitle = "OpenCatapult Web";

$rootPath = Split-Path $PSScriptRoot
$webLocation = Join-Path $rootPath "/src/Web/opencatapultweb"

Set-Location -Path $webLocation

npm install

npm run start -- --ssl --host localhost --port 44300