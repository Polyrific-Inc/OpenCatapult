# Copyright (c) Polyrific, Inc 2018. All rights reserved.

param(
    [switch]$noPrompt = $false,
    [string]$configuration = "Release",
    [string]$url = "https://localhost:44305"
)

$rootPath = Split-Path $PSScriptRoot
$engineCsprojPath = "$rootPath\src\Engine\Polyrific.Catapult.Engine\Polyrific.Catapult.Engine.csproj"
$engineDll = "$rootPath\src\Engine\Polyrific.Catapult.Engine\bin\$configuration\ocengine.dll"

# build engine
Write-Output "Building the Engine..."
Write-Output "dotnet build $engineCsprojPath -c $configuration"
$result = dotnet build $engineCsprojPath -c $configuration
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

Write-Output "Engine is ready. Please run: dotnet $engineDll [command] [options]"