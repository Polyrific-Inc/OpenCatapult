$rootPath = Split-Path $PSScriptRoot
$webLocation = Join-Path $rootPath "/src/Web/opencatapultweb"

Set-Location -Path $webLocation

npm install

npm run start