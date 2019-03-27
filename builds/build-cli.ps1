# Copyright (c) Polyrific, Inc 2018. All rights reserved.

param(
    [string]$configuration = "Release",
    [string]$url = "https://localhost:44305",
    [switch]$noConfig = $false,
    [switch]$openShell = $false
)

$rootPath = Split-Path $PSScriptRoot
$cliCsprojPath = Join-Path $rootPath "/src/CLI/Polyrific.Catapult.Cli/Polyrific.Catapult.Cli.csproj"
$cliPublishPath = Join-Path $rootPath "/publish/cli"
$cliDll = Join-Path $cliPublishPath "/occli.dll"

if ($openShell) {
    $host.UI.RawUI.WindowTitle = "OpenCatapult CLI";
    Write-Host "CLI is ready. You can start exploring available commands by running: dotnet occli.dll --help" -ForegroundColor Green
    Set-Location $cliPublishPath
} else {
    # build CLI
    Write-Output "Publishing the CLI..."
    Write-Output "dotnet publish $cliCsprojPath -c $configuration -o $cliPublishPath"
    $result = dotnet publish $cliCsprojPath -c $configuration -o $cliPublishPath
    if ($LASTEXITCODE -ne 0) {
        Write-Error -Message "[ERROR] $result"
        break
    }

    # Set ApiUrl
    if (!$noConfig) {
        Write-Output "Set ApiUrl config..."
        Write-Output "dotnet $cliDll config set -n ApiUrl -v $url"
        $result = dotnet $cliDll config set -n ApiUrl -v $url
        if ($LASTEXITCODE -ne 0) {
            Write-Error -Message "[ERROR] $result"
            break
        }
    }

    Write-Host "CLI component was published successfully."
}
