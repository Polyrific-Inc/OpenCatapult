# Copyright (c) Polyrific, Inc 2018. All rights reserved.

param(
    [string]$configuration = "Release",
    [string]$url = "https://localhost:44305",
    [switch]$noConfig = $false
)

$host.UI.RawUI.WindowTitle = "OpenCatapult Engine";

$rootPath = Split-Path $PSScriptRoot
$engineCsprojPath = Join-Path $rootPath "/src/Engine/Polyrific.Catapult.Engine/Polyrific.Catapult.Engine.csproj"
$enginePublishPath = Join-Path $rootPath "/publish/engine"
$engineDll = Join-Path $enginePublishPath "/ocengine.dll"

$aspNetCoreMvcCsprojPath = Join-Path $rootPath "/src/Plugins/GeneratorProvider/Polyrific.Catapult.TaskProviders.AspNetCoreMvc/src/Polyrific.Catapult.TaskProviders.AspNetCoreMvc.csproj"
$aspNetCoreMvcPublishPath = Join-Path $enginePublishPath "/plugins/GeneratorProvider/Polyrific.Catapult.TaskProviders.AspNetCoreMvc"
$azureAppServiceCsprojPath = Join-Path $rootPath "/src/Plugins/HostingProvider/Polyrific.Catapult.TaskProviders.AzureAppService/src/Polyrific.Catapult.TaskProviders.AzureAppService.csproj"
$azureAppServicePublishPath = Join-Path $enginePublishPath "/plugins/HostingProvider/Polyrific.Catapult.TaskProviders.AzureAppService"
$dotNetCoreCsprojPath = Join-Path $rootPath "/src/Plugins/BuildProvider/Polyrific.Catapult.TaskProviders.DotNetCore/src/Polyrific.Catapult.TaskProviders.DotNetCore.csproj"
$dotNetCorePublishPath = Join-Path $enginePublishPath "/plugins/BuildProvider/Polyrific.Catapult.TaskProviders.DotNetCore"
$dotNetCoreTestCsprojPath = Join-Path $rootPath "/src/Plugins/TestProvider/Polyrific.Catapult.TaskProviders.DotNetCoreTest/src/Polyrific.Catapult.TaskProviders.DotNetCoreTest.csproj"
$dotNetCoreTestPublishPath = Join-Path $enginePublishPath "/plugins/TestProvider/Polyrific.Catapult.TaskProviders.DotNetCoreTest"
$entityFrameworkCoreCsprojPath = Join-Path $rootPath "/src/Plugins/DatabaseProvider/Polyrific.Catapult.TaskProviders.EntityFrameworkCore/src/Polyrific.Catapult.TaskProviders.EntityFrameworkCore.csproj"
$entityFrameworkCorePublishPath = Join-Path $enginePublishPath "/plugins/DatabaseProvider/Polyrific.Catapult.TaskProviders.EntityFrameworkCore"
$gitHubCsprojPath = Join-Path $rootPath "/src/Plugins/RepositoryProvider/Polyrific.Catapult.TaskProviders.GitHub/src/Polyrific.Catapult.TaskProviders.GitHub.csproj"
$gitHubPublishPath = Join-Path $enginePublishPath "/plugins/RepositoryProvider/Polyrific.Catapult.TaskProviders.GitHub"

$plugins = [System.Tuple]::Create($aspNetCoreMvcCsprojPath, $aspNetCoreMvcPublishPath),
[System.Tuple]::Create($azureAppServiceCsprojPath, $azureAppServicePublishPath),
[System.Tuple]::Create($dotNetCoreCsprojPath, $dotNetCorePublishPath),
[System.Tuple]::Create($dotNetCoreTestCsprojPath, $dotNetCoreTestPublishPath),
[System.Tuple]::Create($entityFrameworkCoreCsprojPath, $entityFrameworkCorePublishPath),
[System.Tuple]::Create($gitHubCsprojPath, $gitHubPublishPath)

# publish engine
Write-Output "Publishing the Engine..."
Write-Output "dotnet publish $engineCsprojPath -c $configuration -o $enginePublishPath"
$result = dotnet publish $engineCsprojPath -c $configuration -o $enginePublishPath
if ($LASTEXITCODE -ne 0) {
    Write-Error -Message "[ERROR] $result"
    break
}

# Set ApiUrl
if (!$noConfig) {
    Write-Output "Set ApiUrl config..."
    Write-Output "dotnet $engineDll config set -n ApiUrl -v $url"
    $result = dotnet $engineDll config set -n ApiUrl -v $url
    if ($LASTEXITCODE -ne 0) {
        Write-Error -Message "[ERROR] $result"
        break
    }
}

# publish plugins
Write-Output "Publishing plugins..."
foreach ($p in $plugins) {
    "dotnet publish {0} -c {1} -o {2}" -f $p.Item1, $configuration, $p.Item2
    $result = dotnet publish $p.Item1 -c $configuration -o $p.Item2
    if ($LASTEXITCODE -ne 0) {
        Write-Error -Message "[ERROR] $result"
        break
    }
}

Write-Host "Engine is ready. You can start exploring available commands by running: dotnet ocengine.dll --help" -ForegroundColor Green

Set-Location $enginePublishPath