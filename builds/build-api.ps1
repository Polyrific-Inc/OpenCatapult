# Copyright (c) Polyrific, Inc 2018. All rights reserved.

param(
    [switch]$noPrompt = $false,
    [string]$configuration = "Release",
    [string]$connString = "",
    [string]$http = "http://localhost:8005",
    [string]$https = "https://localhost:44305",
    [switch]$noRun = $false,
    [string]$environment = "Development"
)

$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = $true
$env:ASPNETCORE_ENVIRONMENT = $environment

# define paths
$rootPath = Split-Path $PSScriptRoot
$appSettingsPath = "$rootPath\src\API\Polyrific.Catapult.Api\appsettings.json"
$apiCsprojPath = "$rootPath\src\API\Polyrific.Catapult.Api\Polyrific.Catapult.Api.csproj"
$apiPublishPath = "$rootPath\publish\api"
$apiDll = "$apiPublishPath\ocapi.dll"
$dataCsprojPath = "$rootPath\src\API\Polyrific.Catapult.Api.Data\Polyrific.Catapult.Api.Data.csproj"

# read connection string in appsettings.json
try {
    $appSettingsFile = ([System.IO.File]::ReadAllText($appSettingsPath) | ConvertFrom-Json)
}
catch {
    Write-Error -Message "[ERROR] $_" -ErrorAction Stop
}

$currentConnString = $appSettingsFile.ConnectionStrings.DefaultConnection

# ask for new connection string
if ($connString -eq "") {
    $connString = $currentConnString

    Write-Output "Current connection string is `"$currentConnString`""

    if (!$noPrompt) {
        $enteredConnString = Read-Host -Prompt "Please enter new connection string, or just ENTER if you want to use current value"    
        if ($enteredConnString -ne "") {
            $connString = $enteredConnString
        }
    }
}

# update connection string
if ($connString -ne $currentConnString) {
    $appSettingsFile.ConnectionStrings.DefaultConnection = $connString

    try {
        $appSettingsFile | ConvertTo-Json -Depth 10 | Out-File -FilePath $appSettingsPath -Encoding utf8 -Force    
    }
    catch {
        Write-Error -Message "[ERROR] $_" -ErrorAction Stop
    }

    Write-Output "Connection string has been updated"
}

# apply migration
Write-Output "Applying migration..."
Write-Output "dotnet ef database update --startup-project $apiCsprojPath --project $dataCsprojPath"
$result = dotnet ef database update --startup-project $apiCsprojPath --project $dataCsprojPath
if ($LASTEXITCODE -ne 0) {
    Write-Error -Message "[ERROR] $result"
    break
}

# check for dev cert
$certCheck = dotnet dev-certs https --check --verbose
if ($certCheck -eq "No valid certificate found."){
    Write-Output "dotnet dev-certs https --trust"
    dotnet dev-certs https --trust
} else {
    Write-Output $certCheck
}

# publish API
Write-Output "Publishing API..."
Write-Output "dotnet publish $apiCsprojPath -c $configuration -o $apiPublishPath"
$result = dotnet publish $apiCsprojPath -c $configuration -o $apiPublishPath
if ($LASTEXITCODE -ne 0) {
    Write-Error -Message "[ERROR] $result"
    break
}

# run the API
if ($noRun) {
    Set-Location $apiPublishPath
    Write-Output "API is ready. Please run: dotnet $apiDll --urls `"$http;$https`""
} else {
    Write-Output "Running API..."
    Write-Output "dotnet $apiDll --urls `"$http;$https`""
    Set-Location $apiPublishPath
    dotnet $apiDll --urls "$http;$https"
}