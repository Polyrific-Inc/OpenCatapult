param(
    [string]$http = "http://localhost:8000",
    [string]$https = "https://localhost:44300",
    [string]$configuration = "Release"
)

$env:ASPNETCORE_ENVIRONMENT = "Production"

$host.UI.RawUI.WindowTitle = "OpenCatapult Web";

$rootPath = Split-Path $PSScriptRoot
$webCsprojPath = Join-Path $rootPath "/src/Web/Polyrific.Catapult.Web/Polyrific.Catapult.Web.csproj"
$webPublishPath = Join-Path $rootPath "/publish/web"
$webDll = Join-Path $webPublishPath "/ocweb.dll"

if (!(Test-Path $webPublishPath)) {
	New-Item -Path $webPublishPath -ItemType directory | Out-Null
}

# publish Web
Write-Output "Publishing Web..."
Write-Output "dotnet publish $webCsprojPath -c $configuration -o $webPublishPath"
$result = dotnet publish $webCsprojPath -c $configuration -o $webPublishPath
if ($LASTEXITCODE -ne 0) {
	Write-Error -Message "[ERROR] $result"
	break
}

Write-Output "Running Web..."
Write-Output "dotnet $webDll --urls `"$http;$https`""
Set-Location $webPublishPath
dotnet $webDll --urls "$http;$https"
