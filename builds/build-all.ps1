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

## Initially make sure the location are in root of opencatapult
$opencatapultRootPath = Split-Path $PSScriptRoot -Parent
Set-Location $opencatapultRootPath


function Invoke-BuildScript ([string]$script, [string]$scriptArgs)
{
   $fullPathScript += ".\builds\" + $script
   if ($scriptArgs) {
    $fullPathScript += " ";
    $fullPathScript += $scriptArgs;
   }
   Invoke-Expression $fullPathScript
}

function Start-InNewWindowMacOS {
  param(
     [Parameter(Mandatory)] [ScriptBlock] $ScriptBlock,
     [Switch] $NoProfile,
     [Switch] $NoExit
  )

  # Construct the shebang line 
  $shebangLine = '#!/usr/bin/env pwsh'
  # Add options, if specified:
  # As an aside: Fundamentally, this wouldn't work on Linux, where
  # the shebang line only supports *1* argument, which is `powershell` in this case.
  if ($NoExit) { $shebangLine += ' -NoExit' }
  if ($NoProfile) { $shebangLine += ' -NoProfile' }

  # Create a temporary script file
  $tmpScript = New-TemporaryFile

  $shebangLine, "Remove-Item -LiteralPath '$tmpScript'", $ScriptBlock.ToString() | 
    Set-Content -Encoding Ascii -LiteralPath $tmpScript

  # Make the script file executable.
  chmod +x $tmpScript

  # Invoke it in a new terminal window via `open -a Terminal`
  # Note that `open` is a macOS-specific utility.
  open -a Terminal -- $tmpScript
}

function Invoke-BuildScriptNewWindow([string]$script, [string]$scriptArgs) 
{    
    if (!($PSVersionTable.Platform) -or $PSVersionTable.Platform -ne "Unix") {
        #Windows env
        $command = "-NoExit "
        $command += "-file `""
        $command += Join-Path -Path $PSScriptRoot -ChildPath $script
        $command += "`" "
        $command += $scriptArgs
        
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
            $command += Join-Path -Path $PSScriptRoot -ChildPath $script
            $command += " "
            $command += $scriptArgs
            $scriptBlock = [Scriptblock]::Create($command)
        
            Start-InNewWindowMacOS -NoExit $scriptBlock
        }
        else {
            if (!$terminal) {
                $terminal = "gnome-terminal";
            }

            $command = "-- pwsh"
            
            $command += " -NoExit"
            $command += " -file `""

            $command += Join-Path -Path $PSScriptRoot -ChildPath $script
            $command += "`" "
            $command += $scriptArgs
        
            Start-Process $terminal $command
        }
    }
}


Invoke-BuildScript "build-prerequisites.ps1"


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
Invoke-BuildScript "build-api.ps1" $args


## Build Engine
$args = "-configuration " + $configuration
$args = " -url " + $https
if ($noConfig) {
    $args = " -noConfig"
}
Invoke-BuildScriptNewWindow "build-engine.ps1" $args

## Build CLI
Invoke-BuildScriptNewWindow "build-cli.ps1" $args

Write-Host "The Engine and CLI are in the new terminal windows. Please go ahead and try to run the commands available there." -ForegroundColor Green 
Write-Host "To learn more about OpenCatapult components, please follow this link: https://docs.opencatapult.net/home/intro#the-components" -ForegroundColor Green 

## Run API
if (!$noRun) {
    $rootPath = Split-Path $PSScriptRoot
    $apiPublishPath = Join-Path $rootPath "/publish/api"
    $apiDll = Join-Path $apiPublishPath "/ocapi.dll"
    Write-Output "Running API..."
    Write-Host "--------------------------------------------------------------" -ForegroundColor Yellow 
    Write-Host "|This terminal window should remain open for catapult to work|" -ForegroundColor Yellow 
    Write-Host "--------------------------------------------------------------" -ForegroundColor Yellow     
    Write-Output "dotnet $apiDll --urls `"$http;$https`""
    Set-Location $apiPublishPath
    dotnet $apiDll --urls "$http;$https"
}