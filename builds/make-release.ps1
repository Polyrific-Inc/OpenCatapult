param (
    [string]$version = "",
    [string]$runtime = "win-x64",
    [switch]$noCompress = $false
)

if ($version -eq "") {
    Write-Error -Message "This process cannot continue without specifying version." -ErrorAction Stop
}

$rootPath = Split-Path $PSScriptRoot
$publishOuterPath = Join-Path $rootPath "/release/opencatapult-v$version-$runtime"

function Publish-Component {
    Param(
        [parameter(Position=1)][string]$csprojPath,
        [parameter(Position=2)][string]$publishPath,
        [parameter(Position=3)][switch]$isTaskProvider = $false,
        [parameter(Position=4)][switch]$copyCert = $false
    )

    Write-Host "Publishing component..."

    dotnet publish $csprojPath -c Release -o $publishPath -r $runtime --self-contained false /p:Version=$version

    Remove-Item "$publishPath/*" -Include appsettings.*.json

    if ($copyCert) {
        $certPath = Join-Path $rootPath "/tools/certs/opencatapultlocal.pfx"

        Copy-Item -Path $certPath -Destination $publishPath
    }
    
    if (!$noCompress) {
        Write-Host "Compressing the package..."

        $compressSrcPath = "$publishPath/*"
        $compressDestPath = "$publishPath-v$version-$runtime.zip"

        if ($isTaskProvider) {
            $compressSrcPath = Join-Path $publishOuterPath "/plugins/*"
            $taskProviderName = Split-Path $publishPath -Leaf
            $compressDestPath = Join-Path $publishOuterPath "$taskProviderName-v$version-$runtime.zip"
        }

        Compress-Archive -Path $compressSrcPath -DestinationPath $compressDestPath -Force
        
        Write-Host "Deleting temporary files.."

        $removeItemPath = $publishPath

        if ($isTaskProvider) {
            $removeItemPath = Join-Path $publishOuterPath "/plugins"
        }

        Remove-Item -Path $removeItemPath -Recurse -Force

        Write-Host "Done. The package has been published to $compressDestPath." -ForegroundColor Green
    } else {
        Write-Host "Done. The component has been published to $publishPath." -ForegroundColor Green
    }
}

Write-Host "Attempting to create component packages for v$version release..."
Write-Host

Write-Host "1. Processing API component" -ForegroundColor Yellow
$apiCsprojPath = Join-Path $rootPath "/src/API/Polyrific.Catapult.Api/Polyrific.Catapult.Api.csproj"
$apiPublishPath = Join-Path $publishOuterPath "/api"
Publish-Component $apiCsprojPath $apiPublishPath $false $true
Write-Host ""

Write-Host "2. Processing CLI component" -ForegroundColor Yellow
$cliCsprojPath = Join-Path $rootPath "/src/CLI/Polyrific.Catapult.Cli/Polyrific.Catapult.Cli.csproj"
$cliPublishPath = Join-Path $publishOuterPath "/cli"
Publish-Component $cliCsprojPath $cliPublishPath
Write-Host ""

Write-Host "3. Processing Engine component" -ForegroundColor Yellow
$engineCsprojPath = Join-Path $rootPath "/src/Engine/Polyrific.Catapult.Engine/Polyrific.Catapult.Engine.csproj"
$enginePublishPath = Join-Path $publishOuterPath "/engine"
Publish-Component $engineCsprojPath $enginePublishPath
Write-Host ""

Write-Host "4. Processing Web component" -ForegroundColor Yellow
$webProjectPath = Join-Path $rootPath "/src/Web/Polyrific.Catapult.Web/Polyrific.Catapult.Web.csproj"
$webPublishPath = Join-Path $publishOuterPath "/web"
Publish-Component $webProjectPath $webPublishPath $false $true
Write-Host ""

Write-Host "5. Processing Task Provider components" -ForegroundColor Yellow

$aspNetCoreMvcCsprojPath = Join-Path $rootPath "/src/Plugins/GeneratorProvider/Polyrific.Catapult.TaskProviders.AspNetCoreMvc/src/Polyrific.Catapult.TaskProviders.AspNetCoreMvc.csproj"
$aspNetCoreMvcPublishPath = Join-Path $publishOuterPath "/plugins/GeneratorProvider/Polyrific.Catapult.TaskProviders.AspNetCoreMvc"
$azureAppServiceCsprojPath = Join-Path $rootPath "/src/Plugins/HostingProvider/Polyrific.Catapult.TaskProviders.AzureAppService/src/Polyrific.Catapult.TaskProviders.AzureAppService.csproj"
$azureAppServicePublishPath = Join-Path $publishOuterPath "/plugins/HostingProvider/Polyrific.Catapult.TaskProviders.AzureAppService"
$dotNetCoreCsprojPath = Join-Path $rootPath "/src/Plugins/BuildProvider/Polyrific.Catapult.TaskProviders.DotNetCore/src/Polyrific.Catapult.TaskProviders.DotNetCore.csproj"
$dotNetCorePublishPath = Join-Path $publishOuterPath "/plugins/BuildProvider/Polyrific.Catapult.TaskProviders.DotNetCore"
$dotNetCoreTestCsprojPath = Join-Path $rootPath "/src/Plugins/TestProvider/Polyrific.Catapult.TaskProviders.DotNetCoreTest/src/Polyrific.Catapult.TaskProviders.DotNetCoreTest.csproj"
$dotNetCoreTestPublishPath = Join-Path $publishOuterPath "/plugins/TestProvider/Polyrific.Catapult.TaskProviders.DotNetCoreTest"
$entityFrameworkCoreCsprojPath = Join-Path $rootPath "/src/Plugins/DatabaseProvider/Polyrific.Catapult.TaskProviders.EntityFrameworkCore/src/Polyrific.Catapult.TaskProviders.EntityFrameworkCore.csproj"
$entityFrameworkCorePublishPath = Join-Path $publishOuterPath "/plugins/DatabaseProvider/Polyrific.Catapult.TaskProviders.EntityFrameworkCore"
$genericCommandCsprojPath = Join-Path $rootPath "/src/Plugins/GenericTaskProvider/Polyrific.Catapult.TaskProviders.GenericCommand/src/Polyrific.Catapult.TaskProviders.GenericCommand.csproj"
$genericCommandPublishPath = Join-Path $publishOuterPath "/plugins/GenericTaskProvider/Polyrific.Catapult.TaskProviders.GenericCommand"
$gitHubCsprojPath = Join-Path $rootPath "/src/Plugins/RepositoryProvider/Polyrific.Catapult.TaskProviders.GitHub/src/Polyrific.Catapult.TaskProviders.GitHub.csproj"
$gitHubPublishPath = Join-Path $publishOuterPath "/plugins/RepositoryProvider/Polyrific.Catapult.TaskProviders.GitHub"

$plugins = [System.Tuple]::Create($aspNetCoreMvcCsprojPath, $aspNetCoreMvcPublishPath),
[System.Tuple]::Create($azureAppServiceCsprojPath, $azureAppServicePublishPath),
[System.Tuple]::Create($dotNetCoreCsprojPath, $dotNetCorePublishPath),
[System.Tuple]::Create($dotNetCoreTestCsprojPath, $dotNetCoreTestPublishPath),
[System.Tuple]::Create($entityFrameworkCoreCsprojPath, $entityFrameworkCorePublishPath),
[System.Tuple]::Create($genericCommandCsprojPath, $genericCommandPublishPath),
[System.Tuple]::Create($gitHubCsprojPath, $gitHubPublishPath)

foreach ($p in $plugins) {
    Publish-Component $p.Item1 $p.Item2 $true
    Write-Host ""
}

if ($noCompress) {

    $pluginsFolder = Join-Path $publishOuterPath "/plugins"
    Move-Item "$pluginsFolder" -Destination "$enginePublishPath"

    Write-Host "6. Copying additional files" -ForegroundColor Yellow

    $certs = Join-Path $rootPath "/tools/certs"
    $dest = Join-Path $publishOuterPath "/certs"
    Copy-Item "$certs" -Destination "$dest" -Recurse

    $runScripts = Join-Path $rootPath "/tools/scripts/run*"
    Copy-Item "$runScripts" -Destination "$publishOuterPath"

    $efDll = Join-Path $rootPath "/tools/ef.dll"
    $dest = Join-Path $publishOuterPath "/tools"
    if (!(Test-Path $dest)) {
        New-Item $dest -ItemType Directory | Out-Null
    }
    Copy-Item "$efDll" -Destination "$dest"

    Write-Host "Done." -ForegroundColor Green
    Write-Host ""
}