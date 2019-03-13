$rootPath = Split-Path $PSScriptRoot
$webLocation = Join-Path $rootPath "/src/Web/opencatapultweb"

Set-Location -Path $webLocation

npm install

Import-Certificate -Filepath "ssl/server.crt" -CertStoreLocation cert:\CurrentUser\Root

npm run start -- --ssl --host localhost --port 44300 --ssl-cert "ssl/server.crt" --ssl-key "ssl/server.key"