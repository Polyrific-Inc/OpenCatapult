# Copyright (c) Polyrific, Inc 2018. All rights reserved.

param(
    [switch]$noPrompt = $false,
    [string]$configuration = "Release",
    [string]$url = "https://localhost:44305"
)

$rootPath = Split-Path $PSScriptRoot
$engineCsprojPath = "$rootPath\src\Engine\Polyrific.Catapult.Engine\Polyrific.Catapult.Engine.csproj"
$enginePublishPath = "$rootPath\publish\engine"
$engineDll = "$enginePublishPath\ocengine.dll"

$plugins = [System.Tuple]::Create("$rootPath\src\Plugins\GeneratorProvider\AspNetCoreMvc\src\AspNetCoreMvc.csproj", "$enginePublishPath\plugins\GeneratorProvider\AspNetCoreMvc"),
[System.Tuple]::Create("$rootPath\src\Plugins\HostingProvider\AzureAppService\src\AzureAppService.csproj", "$enginePublishPath\plugins\HostingProvider\AzureAppService"),
[System.Tuple]::Create("$rootPath\src\Plugins\BuildProvider\DotNetCore\src\DotNetCore.csproj", "$enginePublishPath\plugins\BuildProvider\DotNetCore"),
[System.Tuple]::Create("$rootPath\src\Plugins\TestProvider\DotNetCoreTest\src\DotNetCoreTest.csproj", "$enginePublishPath\plugins\TestProvider\DotNetCoreTest"),
[System.Tuple]::Create("$rootPath\src\Plugins\DatabaseProvider\EntityFrameworkCore\src\EntityFrameworkCore.csproj", "$enginePublishPath\plugins\DatabaseProvider\EntityFrameworkCore"),
[System.Tuple]::Create("$rootPath\src\Plugins\RepositoryProvider\GitHub\src\GitHub.csproj", "$enginePublishPath\plugins\RepositoryProvider\GitHub")

# publish engine
Write-Output "Publishing the Engine..."
Write-Output "dotnet publish $engineCsprojPath -c $configuration -o $enginePublishPath"
$result = dotnet publish $engineCsprojPath -c $configuration -o $enginePublishPath
if ($LASTEXITCODE -ne 0) {
    Write-Error -Message "[ERROR] $result"
    break
}

# Set ApiUrl
Write-Output "Set ApiUrl config..."
Write-Output "dotnet $engineDll config set -n ApiUrl -v $url"
$result = dotnet $engineDll config set -n ApiUrl -v $url
if ($LASTEXITCODE -ne 0) {
    Write-Error -Message "[ERROR] $result"
    break
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

# Copy GitHub assemblies
Write-Output "Copying required files..."
Copy-Item "$enginePublishPath\plugins\RepositoryProvider\GitHub\runtimes\win-x64\native\*" -Destination "$enginePublishPath\plugins\RepositoryProvider\GitHub\" -Force

Write-Output "Engine is ready. Please run: dotnet $engineDll [command] [options]"