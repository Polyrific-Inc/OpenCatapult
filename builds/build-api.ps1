# Copyright (c) Polyrific, Inc 2018. All rights reserved.

param(
    [string]$configuration = "Release",
    [string]$connString = "",
    [string]$dbProvider = "",
    [string]$http = "http://localhost:8005",
    [string]$https = "https://localhost:44305",
    [string]$environment = "Development",
    [switch]$noBuild = $false,
    [switch]$noRun = $false,
    [switch]$noPrompt = $false
)

$host.UI.RawUI.WindowTitle = "OpenCatapult API";

$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = $true
$env:ASPNETCORE_ENVIRONMENT = $environment

# define paths
$rootPath = Split-Path $PSScriptRoot
$appSettingsPath = Join-Path $rootPath "/src/API/Polyrific.Catapult.Api/appsettings.json"
$appSettingsEnvPath = Join-Path $rootPath "/src/API/Polyrific.Catapult.Api/appsettings.$environment.json"
$apiCsprojPath = Join-Path $rootPath "/src/API/Polyrific.Catapult.Api/Polyrific.Catapult.Api.csproj"
$apiPublishPath = Join-Path $rootPath "/publish/api"
$apiDll = Join-Path $apiPublishPath "/ocapi.dll"
$dataCsprojPath = Join-Path $rootPath "/src/API/Polyrific.Catapult.Api.Data/Polyrific.Catapult.Api.Data.csproj"

$defaultMsSqlConnectionString = "Server=localhost;Database=opencatapult.db;User ID=sa;Password=password;"
$defaultSqliteDbFile = "opencatapult.db"

$appSettingsEnvContent = [PSCustomObject]@{
    DatabaseProvider = "sqlite"
    ConnectionStrings = [PSCustomObject]@{
        DefaultConnection = ""
    }
}

# create or load appsettings.[env].json
if (!(Test-Path $appSettingsEnvPath)) {
    if (($dbProvider -eq "") -or ($dbProvider -eq "sqlite")) {
        if ($connString -eq "") {
            $appSettingsEnvContent.ConnectionStrings.DefaultConnection = Join-Path $apiPublishPath $defaultSqliteDbFile
        } else {
            $appSettingsEnvContent.ConnectionStrings.DefaultConnection = $connString
        }
    } else {
        $appSettingsEnvContent.DatabaseProvider = $dbProvider

        if ($connString -eq "") {
            $appSettingsEnvContent.ConnectionStrings.DefaultConnection = $defaultMsSqlConnectionString
        } else {
            $appSettingsEnvContent.ConnectionStrings.DefaultConnection = $connString
        }
    }
    
    $_ = New-Item -Path $appSettingsEnvPath -ItemType "file" -Value ($appSettingsEnvContent | ConvertTo-Json)

    Write-Host "New `"appsettings.$environment.json`" file has been created"
} else {
    $appSettingsEnvContent = Get-Content -Path $appSettingsEnvPath | ConvertFrom-Json
}

if (!$noBuild) {
    $currentDbProvider = $appSettingsEnvContent.DatabaseProvider

    # ask for database provider
    if ($dbProvider -eq "") {
        $dbProvider = $currentDbProvider

        Write-Output "Current database provider is `"$dbProvider`""

        if (!$noPrompt) {
            $enteredDbProvider = Read-Host -Prompt "Please enter new database provider (or just ENTER if you want to use current value)"
            if (![string]::IsNullOrWhiteSpace($enteredDbProvider)) {
                $dbProvider = $enteredDbProvider
            }
        }
    }

    # update database provider
    if ($dbProvider -ne $currentDbProvider) {
        $appSettingsEnvContent.DatabaseProvider = $dbProvider

        try {
            $appSettingsEnvContent | ConvertTo-Json -Depth 10 | Out-File -FilePath $appSettingsEnvPath -Encoding utf8 -Force    
        }
        catch {
            Write-Error -Message "[ERROR] $_" -ErrorAction Stop
        }

        Write-Output "Database Provider has been updated"
    }

    $success = $false;
    while (!$success) {
        $currentConnString = $appSettingsEnvContent.ConnectionStrings.DefaultConnection

        # ask for new connection string
        if ($connString -eq "") {
            $connString = $currentConnString

            Write-Output "Current connection string is `"$connString`""

            if (!$noPrompt) {
                $enteredConnString = Read-Host -Prompt "Please enter new connection string (or just ENTER if you want to use current value)"    
                if (![string]::IsNullOrWhiteSpace($enteredConnString)) {
                    $connString = $enteredConnString
                }
            }
        }

        # update connection string
        if ($connString -ne $currentConnString) {
            $appSettingsEnvContent.ConnectionStrings.DefaultConnection = $connString

            try {
                $appSettingsEnvContent | ConvertTo-Json -Depth 10 | Out-File -FilePath $appSettingsEnvPath -Encoding utf8 -Force    
            }
            catch {
                Write-Error -Message "[ERROR] $_" -ErrorAction Stop
            }

            Write-Output "Connection string has been updated"
        }

        # apply migration
        $dbcontext = "CatapultDbContext"
        if ($dbProvider -eq "sqlite") {
            $dbcontext = "CatapultSqliteDbContext"
        }

        Write-Output "Applying migration..."
        Write-Output "dotnet ef database update --startup-project $apiCsprojPath --project $dataCsprojPath --context $dbcontext"

        $result = dotnet ef database update --startup-project $apiCsprojPath --project $dataCsprojPath --context $dbcontext
        if ($LASTEXITCODE -ne 0) {
            Write-Error -Message "[ERROR] $result"
            Write-Host "Error occured while trying to migrate database. Do you want to retry entering another connection string? (y/n)" -ForegroundColor Yellow
            $retry = Read-Host
            if ($retry -ne "y") {
                break
            }
            
            $connString = ""
        }
        else {
            $success = $true
        }	
    }

    if (!$success) {
        break;
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
}

# run the API
if ($noRun) {
    Write-Output "API is ready. Please run: dotnet $apiDll --urls `"$http;$https`""
} else {
    Write-Output "Running API..."
    Write-Host "--------------------------------------------------------------" -ForegroundColor Yellow 
    Write-Host "|This terminal window should remain open for catapult to work|" -ForegroundColor Yellow 
    Write-Host "--------------------------------------------------------------" -ForegroundColor Yellow 
    Write-Host "To learn more about catapult components, please follow this link: https://docs.opencatapult.net/home/intro#the-components" -ForegroundColor Green 
    Write-Output "dotnet $apiDll --urls `"$http;$https`""
    Set-Location $apiPublishPath
    dotnet $apiDll --urls "$http;$https"
}