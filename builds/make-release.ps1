param (
    [string]$version = "",
    [string]$runtime = "win-x64"
)

if ($version -eq "") {
    Write-Error -Message "This process cannot continue without specifying version." -ErrorAction Stop
}

$rootPath = Split-Path $PSScriptRoot
$publishOuterPath = Join-Path $rootPath "/release"

function Publish-Component {
    Param(
        [parameter(Position=1)][string]$csprojPath,
        [parameter(Position=2)][string]$publishPath,
        [parameter(Position=3)][switch]$isTaskProvider = $false
    )

    Write-Host "Publishing component..."

    dotnet publish $csprojPath -c Release -o $publishPath -r $runtime /p:Version=$version
    
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
}

Write-Host "Attempting to create component packages for v$version release..."
Write-Host

Write-Host "1. Processing API component" -ForegroundColor Yellow
$apiCsprojPath = Join-Path $rootPath "/src/API/Polyrific.Catapult.Api/Polyrific.Catapult.Api.csproj"
$apiPublishPath = Join-Path $publishOuterPath "/api"
Publish-Component $apiCsprojPath $apiPublishPath
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

Write-Host "4. Processing Task Provider components" -ForegroundColor Yellow

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
$gitHubCsprojPath = Join-Path $rootPath "/src/Plugins/RepositoryProvider/Polyrific.Catapult.TaskProviders.GitHub/src/Polyrific.Catapult.TaskProviders.GitHub.csproj"
$gitHubPublishPath = Join-Path $publishOuterPath "/plugins/RepositoryProvider/Polyrific.Catapult.TaskProviders.GitHub"

$plugins = [System.Tuple]::Create($aspNetCoreMvcCsprojPath, $aspNetCoreMvcPublishPath),
[System.Tuple]::Create($azureAppServiceCsprojPath, $azureAppServicePublishPath),
[System.Tuple]::Create($dotNetCoreCsprojPath, $dotNetCorePublishPath),
[System.Tuple]::Create($dotNetCoreTestCsprojPath, $dotNetCoreTestPublishPath),
[System.Tuple]::Create($entityFrameworkCoreCsprojPath, $entityFrameworkCorePublishPath),
[System.Tuple]::Create($gitHubCsprojPath, $gitHubPublishPath)

foreach ($p in $plugins) {
    Publish-Component $p.Item1 $p.Item2 $true
    Write-Host ""
}