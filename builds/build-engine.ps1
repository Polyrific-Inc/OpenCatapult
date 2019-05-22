# Copyright (c) Polyrific, Inc 2018. All rights reserved.

param(
    [string]$configuration = "Release",
    [string]$url = "https://localhost:44305",
    [switch]$noConfig = $false,
    [switch]$noOpenShell = $false,
    [switch]$noBuild = $false
)

$rootPath = Split-Path $PSScriptRoot
$engineCsprojPath = Join-Path $rootPath "/src/Engine/Polyrific.Catapult.Engine/Polyrific.Catapult.Engine.csproj"
$enginePublishPath = Join-Path $rootPath "/publish/engine"
$engineDll = Join-Path $enginePublishPath "/ocengine.dll"

if (!$noOpenShell) {
    $host.UI.RawUI.WindowTitle = "OpenCatapult Engine";
} 

if (!$noBuild) {

    $aspNetCoreMvcCsprojPath = Join-Path $rootPath "/src/TaskProviders/GeneratorProvider/Polyrific.Catapult.TaskProviders.AspNetCoreMvc/src/Polyrific.Catapult.TaskProviders.AspNetCoreMvc.csproj"
    $aspNetCoreMvcPublishPath = Join-Path $enginePublishPath "/taskproviders/GeneratorProvider/Polyrific.Catapult.TaskProviders.AspNetCoreMvc"
    $azureAppServiceCsprojPath = Join-Path $rootPath "/src/TaskProviders/HostingProvider/Polyrific.Catapult.TaskProviders.AzureAppService/src/Polyrific.Catapult.TaskProviders.AzureAppService.csproj"
    $azureAppServicePublishPath = Join-Path $enginePublishPath "/taskproviders/HostingProvider/Polyrific.Catapult.TaskProviders.AzureAppService"
    $dotNetCoreCsprojPath = Join-Path $rootPath "/src/TaskProviders/BuildProvider/Polyrific.Catapult.TaskProviders.DotNetCore/src/Polyrific.Catapult.TaskProviders.DotNetCore.csproj"
    $dotNetCorePublishPath = Join-Path $enginePublishPath "/taskproviders/BuildProvider/Polyrific.Catapult.TaskProviders.DotNetCore"
    $dotNetCoreTestCsprojPath = Join-Path $rootPath "/src/TaskProviders/TestProvider/Polyrific.Catapult.TaskProviders.DotNetCoreTest/src/Polyrific.Catapult.TaskProviders.DotNetCoreTest.csproj"
    $dotNetCoreTestPublishPath = Join-Path $enginePublishPath "/taskproviders/TestProvider/Polyrific.Catapult.TaskProviders.DotNetCoreTest"
    $entityFrameworkCoreCsprojPath = Join-Path $rootPath "/src/TaskProviders/DatabaseProvider/Polyrific.Catapult.TaskProviders.EntityFrameworkCore/src/Polyrific.Catapult.TaskProviders.EntityFrameworkCore.csproj"
    $entityFrameworkCorePublishPath = Join-Path $enginePublishPath "/taskproviders/DatabaseProvider/Polyrific.Catapult.TaskProviders.EntityFrameworkCore"
    $gitHubCsprojPath = Join-Path $rootPath "/src/TaskProviders/RepositoryProvider/Polyrific.Catapult.TaskProviders.GitHub/src/Polyrific.Catapult.TaskProviders.GitHub.csproj"
    $gitHubPublishPath = Join-Path $enginePublishPath "/taskproviders/RepositoryProvider/Polyrific.Catapult.TaskProviders.GitHub"
    $genericCommandCsprojPath = Join-Path $rootPath "/src/TaskProviders/GenericTaskProvider/Polyrific.Catapult.TaskProviders.GenericCommand/src/Polyrific.Catapult.TaskProviders.GenericCommand.csproj"
    $genericCommandPublishPath = Join-Path $enginePublishPath "/taskproviders/GenericTaskProvider/Polyrific.Catapult.TaskProviders.GenericCommand"

    $taskProviders = [System.Tuple]::Create($aspNetCoreMvcCsprojPath, $aspNetCoreMvcPublishPath),
    [System.Tuple]::Create($azureAppServiceCsprojPath, $azureAppServicePublishPath),
    [System.Tuple]::Create($dotNetCoreCsprojPath, $dotNetCorePublishPath),
    [System.Tuple]::Create($dotNetCoreTestCsprojPath, $dotNetCoreTestPublishPath),
    [System.Tuple]::Create($entityFrameworkCoreCsprojPath, $entityFrameworkCorePublishPath),
    [System.Tuple]::Create($gitHubCsprojPath, $gitHubPublishPath),
    [System.Tuple]::Create($genericCommandCsprojPath, $genericCommandPublishPath)

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

    # publish task providers
    Write-Output "Publishing task providers..."
    foreach ($tp in $taskProviders) {
        "dotnet publish {0} -c {1} -o {2}" -f $tp.Item1, $configuration, $tp.Item2
        $result = dotnet publish $tp.Item1 -c $configuration -o $tp.Item2
        if ($LASTEXITCODE -ne 0) {
            Write-Error -Message "[ERROR] $result"
            break
        }
    }

    Write-Host "Engine component was published successfully."
}

if (!$noOpenShell) {
    Write-Host "Engine is ready. You can start exploring available commands by running: dotnet ocengine.dll --help" -ForegroundColor Green
    Set-Location $enginePublishPath
} 