# Copyright (c) Polyrific, Inc 2018. All rights reserved.

#Requires -RunAsAdministrator

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
$apiCsprojPath = Join-Path $rootPath "/src/API/Polyrific.Catapult.Api/Polyrific.Catapult.Api.csproj"
$apiPublishPath = Join-Path $rootPath "/publish/api"
$apiDll = Join-Path $apiPublishPath "/ocapi.dll"
$appSettingsPath = Join-Path $apiPublishPath "/appsettings.json"
$appSettingsEnvPath = Join-Path $apiPublishPath "/appsettings.$environment.json"

$cerPath = Join-Path $rootPath "/tools/certs/opencatapultlocal.cer"
$pfxPath = Join-Path $rootPath "/tools/certs/opencatapultlocal.pfx"

$defaultMsSqlConnectionString = "Server=localhost;Database=opencatapult.db;User ID=sa;Password=password;"
$defaultSqliteDbFile = "opencatapult.db"

$appSettingsEnvContent = [PSCustomObject]@{
    DatabaseProvider = "sqlite"
    ConnectionStrings = [PSCustomObject]@{
        DefaultConnection = ""
    }
}

if (!$noBuild) {
    if (!(Test-Path $apiPublishPath)) {
        New-Item -Path $apiPublishPath -ItemType directory | Out-Null
    }

    # publish API
    Write-Output "Publishing API..."
    Write-Output "dotnet publish $apiCsprojPath -c $configuration -o $apiPublishPath"
    $result = dotnet publish $apiCsprojPath -c $configuration -o $apiPublishPath
    if ($LASTEXITCODE -ne 0) {
        Write-Error -Message "[ERROR] $result"
        break
    } else {
        Copy-Item $pfxPath -Destination $apiPublishPath
    }
}

# create or load appsettings.[env].json
if (!(Test-Path $appSettingsEnvPath)) {
    if (($dbProvider -eq "") -or ($dbProvider -eq "sqlite")) {
        if ($connString -eq "") {
            $sqliteDbPath = Join-Path $apiPublishPath $defaultSqliteDbFile
            $appSettingsEnvContent.ConnectionStrings.DefaultConnection = "Data Source=$sqliteDbPath"
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
        Write-Output "Applying migration..."

        $dbcontext = "CatapultDbContext"
        if ($dbProvider -eq "sqlite") {
            $dbcontext = "CatapultSqliteDbContext"
        }
        $efDll = Join-Path $rootPath "/tools/ef.dll"

        Set-Location $apiPublishPath
        
        Write-Output "dotnet exec --depsfile `"ocapi.deps.json`" --runtimeconfig `"ocapi.runtimeconfig.json`" `"$efDll`" database update --assembly `"Polyrific.Catapult.Api.Data.dll`" --startup-assembly `"ocapi.dll`" --verbose --context $dbcontext"
        
        $result = dotnet exec --depsfile "ocapi.deps.json" --runtimeconfig "ocapi.runtimeconfig.json" "$efDll" database update --assembly "Polyrific.Catapult.Api.Data.dll" --startup-assembly "ocapi.dll" --verbose --context $dbcontext
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

    # import dev cert
    certutil -addstore -f -enterprise -v root "$cerPath" 
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