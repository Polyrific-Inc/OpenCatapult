# Copyright (c) Polyrific, Inc 2018. All rights reserved.

function Get-SqlServiceStatus ([string]$server)
{
   if(Test-Connection $server -Count 2 -Quiet)
   {
    Get-WmiObject win32_Service -Computer $server |
     where {$_.DisplayName -match "SQL Server \("} | 
     select SystemName, DisplayName, Name, State, Status, StartMode, StartName
   }
}

$dotnetSdkVersion = [System.Version]"2.1.500"
$allGood = $true

$currentSdkVersion = dotnet --version


if ([System.Version]$currentSdkVersion -lt $dotnetSdkVersion) {	
	$allGood = $false
	# Ask user if she want to automatically install the dotnet SDK
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

Write-Host "Checking sql server local instance..."
if (!($PSVersionTable.Platform) -or $PSVersionTable.Platform -ne "Unix") {
	# Check SQL Instance in windows
	$sqlResult = Get-SqlServiceStatus -server localhost
	if ($sqlResult) {
		if ($sqlResult.State -eq "Running") {
			"SQL Server available in local machine."
		}
		else {
			# Ask user if she want to automatically run the sql service
			$allGood = $false
			Write-Host "SQL instance is found in the local machine, but the service is not running. Do you want to start it now? (y/n)" -ForegroundColor Yellow
			$readRunSql = Read-Host
			Switch ($readRunSql) { 
				Y {$runSql=$true} 
				N {$runSql=$false} 
				Default {Write-Host "Invalid input" -ForegroundColor Red; $runSql=$false} 
			} 

			if ($runSql) {
				$currentPrincipal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
				
				if ($currentPrincipal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {                
					if ($sqlResult.State -eq "Paused") {
						Resume-Service -Name $sqlResult.Name
					}
					else {
						Start-Service -Name $sqlResult.Name
					}
				}
				else {
					Write-Host "You need Administrator access to start the service" -ForegroundColor Red
				}
			}
		}
	}
	else {
		$allGood = $false
		Write-Host "SQL Server is not found in the local machine. Please install it if you want to use localhost as the db server. Otherwise, you can provide a remote db server." -ForegroundColor Red
	}
}
else {
	# Check SQL Instance in linux
	$linuxSqlResult = systemctl status mssql-server
	if ($linuxSqlResult -Contains "Active: active") {
		"SQL Server available in local machine."
	}
	elseif ($linuxSqlResult -Contains "Active: inactive") {	
		# Ask user if she want to automatically install the dotnet SDK
		$allGood = $false
		Write-Host "SQL instance is found in the local machine, but the service is not running. Do you want to start it now? (y/n)" -ForegroundColor Yellow
		$readRunSqlLinux = Read-Host
		Switch ($readRunSqlLinux) { 
			Y {$runSqlLinux=$true} 
			N {$runSqlLinux=$false} 
			Default {Write-Host "Invalid input" -ForegroundColor Red; $runSqlLinux=$false} 
		} 
		
		if ($runSqlLinux) {
			sudo systemctl enable mssql-server
		}
	}
	else {
		Write-Host "SQL Server is not found in the local machine. Please install it if you want to use localhost as the db server. Otherwise, you can provide a remote db server." -ForegroundColor Red
	}
}

if ($allGood){
    Write-Output "All good, all required resources are in place."
}