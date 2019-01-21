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

function Start-InNewWindowMacOS {
  param(
     [Parameter(Mandatory)] [ScriptBlock] $ScriptBlock,
     [Switch] $NoProfile,
     [Switch] $NoExit
  )

  # Construct the shebang line 
  $shebangLine = '#!/usr/bin/env powershell'
  # Add options, if specified:
  # As an aside: Fundamentally, this wouldn't work on Linux, where
  # the shebang line only supports *1* argument, which is `powershell` in this case.
  if ($NoExit) { $shebangLine += ' -NoExit' }
  if ($NoProfile) { $shebangLine += ' -NoProfile' }

  # Create a temporary script file
  $tmpScript = New-TemporaryFile

  # Add the shebang line, the self-deletion code, and the script-block code.
  # Note: 
  #      * The self-deletion code assumes that the script was read *as a whole*
  #        on execution, which assumes that it is reasonably small.
  #        Ideally, the self-deletion code would use 
  #        'Remove-Item -LiteralPath $PSCommandPath`, but, 
  #        as of PowerShell Core v6.0.0-beta.6, this doesn't work due to a bug 
  #        - see https://github.com/PowerShell/PowerShell/issues/4217
  #      * UTF8 encoding is desired, but -Encoding utf8, regrettably, creates
  #        a file with BOM. For now, use ASCII.
  #        Once v6 is released, BOM-less UTF8 will be the *default*, in which
  #        case you'll be able to use `> $tmpScript` instead.
  $shebangLine, "Remove-Item -LiteralPath '$tmpScript'", $ScriptBlock.ToString() | 
    Set-Content -Encoding Ascii -LiteralPath $tmpScript

  # Make the script file executable.
  chmod +x $tmpScript

  # Invoke it in a new terminal window via `open -a Terminal`
  # Note that `open` is a macOS-specific utility.
  open -a Terminal -- $tmpScript
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
            $command = "`""
            $command += Join-Path -Path $PSScriptRoot -ChildPath $script
            $command += "`" "
            $command += $scriptArgs
        
            if ($wait) {
                Start-InNewWindowMacOS $command 
            }
            else {
                Start-InNewWindowMacOS -NoExit $command 
            }
            
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