# Copyright (c) Polyrific, Inc 2018. All rights reserved.

param(
    [switch]$noPrompt = $false,
    [string]$configuration = "Release",
    [string]$url = "https://localhost:44305"
)

$rootPath = Split-Path $PSScriptRoot
$cliCsprojPath = "$rootPath\src\CLI\Polyrific.Catapult.Cli\Polyrific.Catapult.Cli.csproj"
$cliPublishPath = "$rootPath\publish\cli"
$cliDll = "$cliPublishPath\occli.dll"

# build CLI
Write-Output "Publishing the CLI..."
Write-Output "dotnet publish $cliCsprojPath -c $configuration -o $cliPublishPath"
$result = dotnet publish $cliCsprojPath -c $configuration -o $cliPublishPath
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