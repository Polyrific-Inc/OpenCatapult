# Copyright (c) Polyrific, Inc 2018. All rights reserved.

$dotnetSdkVersion = [System.Version]"2.1.500"
$allGood = $true

$currentSdkVersion = dotnet --version

if ([System.Version]$currentSdkVersion -lt $dotnetSdkVersion) {	
	$allGood = $false
	Write-host "We need to install the dotnet SDK version $dotnetSdkVersion for you. Proceed? (y/n)" -ForegroundColor Yellow 
	$readInstallDotnet = Read-Host
	Switch ($readInstallDotnet) { 
		Y {$installDotnet=$true} 
		N {$installDotnet=$false} 
		Default {Write-Host "Invalid input" -ForegroundColor Red; $installDotnet=$false} 
	} 
	
	if ($installDotnet) {
		if (Test-Path ".\builds\dotnet-install.ps1") { 
			.\builds\dotnet-install.ps1 -Channel 2.1 -InstallDir $env:ProgramFiles\dotnet
		}
		elseif (Test-Path ".\dotnet-install.ps1") { 
			.\dotnet-install.ps1 -Channel 2.1 -InstallDir $env:ProgramFiles\dotnet
		}
		else { 
			Write-Host "Cannot find dotnet-install.ps1" -ForegroundColor Red
			Exit 1
		}
	}
}

if ($allGood){
    Write-Output "All good, all required resources are in place."
}