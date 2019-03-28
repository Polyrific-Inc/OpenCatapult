# Copyright (c) Polyrific, Inc 2018. All rights reserved.
param(
    [string]$configuration = "Release",
    [string]$connString = "",
    [string]$environment = "Development",
    [string]$http = "http://localhost:8005",
    [string]$https = "https://localhost:44305",
    [string]$webHost = "localhost",
    [string]$webPort = "44300",
    [switch]$noConfig = $false,
    [switch]$noCli = $false,
    [switch]$noPrompt = $false,
    [switch]$noRun = $false,
    [switch]$noWeb = $false,
    [string]$terminal = ""
)

## Initially make sure the location are in root of opencatapult
$opencatapultRootPath = Split-Path $PSScriptRoot -Parent
Set-Location $opencatapultRootPath

function Start-InNewWindow {
    param(
        [string] $command
    )

    $psVersion = $PSVersionTable.PSVersion.Major
    if ($psVersion -lt 6) {
        Start-Process Powershell $command
    } else {
        Start-Process pwsh $command
    }

    return $true
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

    return $true
}

function Invoke-BuildScript ([string]$script, [string]$scriptArgs) {
    $fullPathScript += ".\builds\" + $script
    if ($scriptArgs) {
     $fullPathScript += " ";
     $fullPathScript += $scriptArgs;
    }
    Invoke-Expression $fullPathScript
 }

function Invoke-BuildScriptNewWindow([string]$script, [string]$scriptArgs) {    
    $done = $false

    if (!($PSVersionTable.Platform) -or $PSVersionTable.Platform -ne "Unix") {
        #Windows env
        $command = "-NoExit "
        $command += "-file `""
        $command += Join-Path -Path $PSScriptRoot -ChildPath $script
        $command += "`" "
        $command += $scriptArgs
        
        $done = Start-InNewWindow $command
    }
    else {
        # Linux or Mac
        if ($IsMacOS) {
            $command += Join-Path -Path $PSScriptRoot -ChildPath $script
            $command += " "
            $command += $scriptArgs
            $scriptBlock = [Scriptblock]::Create($command)
        
            $done = Start-InNewWindowMacOS -NoExit $scriptBlock
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

            $done = $true
        }
    }

    return $done
}

## Check pre-requisites
$buildPrerequisitesArgs = ""
if ($noWeb) {
    $buildPrerequisitesArgs += " -noWeb"
}
if ($noPrompt) {
    $buildPrerequisitesArgs += " -noPrompt"
}
$allGood = Invoke-BuildScript "build-prerequisites.ps1" $buildPrerequisitesArgs
if (!$allGood) {
    Write-Host "Some items don't meet the minimum requirement. Do you want to continue? (y/n)" -ForegroundColor Yellow
    $continue = Read-Host
    if ($continue -ne "y") {
        break
    }
}

$args = "-configuration " + $configuration
$args += " -url " + $https
if ($noConfig) {
    $args += " -noConfig"
}
$args += " -noOpenShell"

## Build CLI
if (!$noCli) {
	Write-Host "Publishing CLI..."
    $done = Invoke-BuildScript "build-cli.ps1" $args
    Invoke-BuildScriptNewWindow "build-cli.ps1" "-noBuild"
}

## Build Engine
Write-Host "Publishing Engine..."
$done = Invoke-BuildScript "build-engine.ps1" $args
Invoke-BuildScriptNewWindow "build-engine.ps1" "-noBuild"

## Build Web
if (!$noWeb) {
	Write-Host "Publishing Web UI..."
	$args = "-host " + $webHost
	$args += " -port " + $webPort
    $done = Invoke-BuildScriptNewWindow "build-web.ps1" $args
}

Write-Host "The Engine and CLI are in the new terminal windows. Please go ahead and try to run the commands available there." -ForegroundColor Green 
Write-Host "To learn more about OpenCatapult components, please follow this link: https://docs.opencatapult.net/home/intro#the-components" -ForegroundColor Green

## Build API
$args = "-configuration " + $configuration
$args += " -http " + $http
$args += " -https " + $https
$args += " -environment " + $environment
if ($noRun) {
    $args += " -noRun" 
}
if ($noPrompt) {
    $args += " -noPrompt"
}
if ($connString) {
    $args += "-connString " + $connString
}
Invoke-BuildScript "build-api.ps1" $args
