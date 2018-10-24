# Copyright (c) Polyrific, Inc 2018. All rights reserved.

param(
    [switch]$noPrompt = $false,
    [string]$configuration = "Release",
    [string]$url = "https://localhost:44305"
)

$rootPath = Split-Path $PSScriptRoot
$cliCsprojPath = "$rootPath\src\CLI\Polyrific.Catapult.Cli\Polyrific.Catapult.Cli.csproj"
$cliDll = "$rootPath\src\CLI\Polyrific.Catapult.Cli\bin\$configuration\occli.dll"

# build CLI
Write-Output "Building the CLI..."
Write-Output "dotnet build $cliCsprojPath -c $configuration"
$result = dotnet build $cliCsprojPath -c $configuration
if ($LASTEXITCODE -ne 0) {
    Write-Error -Message "[ERROR] $result"
    break
}

# Set ApiUrl
Write-Output "Set ApiUrl config..."
Write-Output "dotnet $cliDll config set -n ApiUrl -v $url"
$result = dotnet $cliDll config set -n ApiUrl -v $url
if ($LASTEXITCODE -ne 0) {
    Write-Error -Message "[ERROR] $result"
    break
}

Write-Output "CLI is ready. Please run: dotnet $cliDll [command] [options]"