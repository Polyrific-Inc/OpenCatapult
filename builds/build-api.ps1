# Copyright (c) Polyrific, Inc 2018. All rights reserved.

param(
    [switch]$noPrompt = $false,
    [string]$configuration = "Release",
    [string]$connString = "",
    [string]$url = "https://localhost:44305"
)

$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = $true

# define paths
$rootPath = Split-Path $PSScriptRoot
$appSettingsPath = "$rootPath\src\API\Polyrific.Catapult.Api\appsettings.json"
$apiCsprojPath = "$rootPath\src\API\Polyrific.Catapult.Api\Polyrific.Catapult.Api.csproj"
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
        $appSettingsFile | ConvertTo-Json | Out-File -FilePath $appSettingsPath -Encoding utf8 -Force    
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

# run the API
Write-Output "Running API..."
Write-Output "dotnet run -p $apiCsprojPath -c $configuration --urls $url"
$result = dotnet run -p $apiCsprojPath -c $configuration --urls $url
if ($LASTEXITCODE -ne 0) {
    Write-Error -Message "[ERROR] $result"
    break
}