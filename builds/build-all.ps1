# Copyright (c) Polyrific, Inc 2018. All rights reserved.
param(
    [switch]$noPrompt = $false,
    [string]$configuration = "Release",
    [string]$connString = "",
    [string]$http = "http://localhost:8005",
    [string]$https = "https://localhost:44305",
    [switch]$noRun = $false,
    [string]$environment = "Development",
    [string]$terminal = "",
    [switch]$noConfig = $false
)


function Run-BuildScript ([string]$script)
{
   $fullPathScript = Join-Path -Path $PSScriptRoot -ChildPath $script
   &$fullPathScript
}

function Run-BuildScriptNewWindow([string]$script, [string]$scriptArgs, [bool]$wait) 
{    
    if (!($PSVersionTable.Platform) -or $PSVersionTable.Platform -ne "Unix") {
        #Windows env
        if (!$wait) {
            $command = "-NoExit"
        }
        else {
            $command = ""
        }

        $command += " -file `""
        $command += Join-Path -Path $PSScriptRoot -ChildPath $script
        $command += "`" "
        $command += $scriptArgs
        write-host $command
        
        if ($wait) {
            Start-Process Powershell $command -Wait
        }
        else {
            Start-Process Powershell $command
        }
    }
    else {
        # Linux or Mac
        if ($IsMacOS) {
            
        }
        else {
            if (!$terminal) {
                $terminal = "gnome-terminal";
            }

            $command = "-- pwsh"
            
            
            if (!$wait) {
                $command = " -NoExit"
            }
            $command = " -file `""

            $command += Join-Path -Path $PSScriptRoot -ChildPath $script
            $command += "`" "
            $command += $scriptArgs
        
            if ($wait) {
                Start-Process $terminal $command -Wait
            }
            else {
                Start-Process $terminal $command
            }
        }
    }
}


Run-BuildScript "build-prerequisites.ps1"


## Build API
$args = "-configuration " + $configuration
$args += " -http " + $http
$args += " -https " + $https
$args += " -environment " + $environment
## Run API after engine & build window opened
$args += " -noRun" 

if ($noPrompt) {
    $args += " -noPrompt"
}
if ($connString) {
    $args += "-connString " + $connString
}
Run-BuildScriptNewWindow "build-api.ps1" $args $true


## Build Engine
$args = "-configuration " + $configuration
$args = " -url " + $https
if ($noConfig) {
    $args = " -noConfig"
}
Run-BuildScriptNewWindow "build-engine.ps1" $args

## Build CLI
Run-BuildScriptNewWindow "build-cli.ps1" $args

## Run API
if (!$noRun) {
    $rootPath = Split-Path $PSScriptRoot
    $apiPublishPath = Join-Path $rootPath "/publish/api"
    $apiDll = Join-Path $apiPublishPath "/ocapi.dll"
    Write-Output "Running API..."
    Write-Output "dotnet $apiDll --urls `"$http;$https`""
    Set-Location $apiPublishPath
    dotnet $apiDll --urls "$http;$https"
}