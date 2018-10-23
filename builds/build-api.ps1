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

# read connection string in appsettings.json
$appSettingsFile = ([System.IO.File]::ReadAllText($appSettingsPath)  | ConvertFrom-Json)
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
    $appSettingsFile | ConvertTo-Json | Out-File -FilePath $appSettingsPath -Encoding utf8 -Force

    Write-Output "Connection string has been updated"
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
dotnet run -p $apiCsprojPath -c $configuration --urls $url