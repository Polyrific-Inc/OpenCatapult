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
$versionPattern = [Regex]::new("(\d+\.)(\d+\.)(\d+)")
$parsedSdkVersion = $versionPattern.Matches($currentSdkVersion)

if ([System.Version]$parsedSdkVersion.Value -lt $dotnetSdkVersion) {	
	$allGood = $false
	# Ask user if she want to automatically install the dotnet SDK
	Write-host "We need to install the dotnet SDK version $dotnetSdkVersion for you. Proceed? (y/n)" -ForegroundColor Yellow 
	$readInstallDotnet = Read-Host
	Switch ($readInstallDotnet) { 
		Y {$installDotnet=$true} 
		N {$installDotnet=$false} 
		Default {Write-Error "Invalid input"; break;} 
	} 
	
	if ($installDotnet) {
		if (Test-Path ".\builds\dotnet-install.ps1") { 
			.\builds\dotnet-install.ps1 -Channel 2.1 -InstallDir $env:ProgramFiles\dotnet
		}
		elseif (Test-Path ".\dotnet-install.ps1") { 
			.\dotnet-install.ps1 -Channel 2.1 -InstallDir $env:ProgramFiles\dotnet
		}
		else { 
			Write-Error "Cannot find dotnet-install.ps1"
			break
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
				Default {Write-Error "Invalid input"; break;} 
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
					Write-Host "You need Administrator access to start the service. Please execute the script in a shell with elevated permission or start the service manually." -ForegroundColor Yellow
					Write-Host "Do you want to continue with the update migration process now? (y/n)" -ForegroundColor Yellow
					$readContinue = Read-Host
					Switch ($readContinue) { 
						Y {$continueBuild=$true} 
						N {$continueBuild=$false} 
						Default {Write-Error "Invalid input"; break;} 
					}
					
					if (!$continueBuild) {
						break;
					}
				}
			}
		}
	}
	else {
		$allGood = $false
		Write-Host "SQL Server is not found in the local machine. Please install it if you want to use localhost as the db server. Otherwise, you can provide a remote db server." -ForegroundColor Red
	}
}
elseif ($IsMacOS) {
	Write-Host "You are running on Mac OS which currently does not support SQL Server. Please provide a remote db server when building API." -ForegroundColor Red
}
else {
	# Check SQL Instance in linux
	$linuxSqlResult = systemctl status mssql-server
	if ($linuxSqlResult -like "*Active: active*") {
		"SQL Server available in local machine."
	}
	elseif ($linuxSqlResult -like "*Active: inactive*") {	
		# Ask user if she want to automatically install the dotnet SDK
		$allGood = $false
		Write-Host "SQL instance is found in the local machine, but the service is not running. Do you want to start it now? (y/n)" -ForegroundColor Yellow
		$readRunSqlLinux = Read-Host
		Switch ($readRunSqlLinux) { 
			Y {$runSqlLinux=$true} 
			N {$runSqlLinux=$false} 
			Default {Write-Error "Invalid input"; break;} 
		} 
		
		if ($runSqlLinux) {
			sudo systemctl start mssql-server
		}
	}
	else {
		Write-Host "SQL Server is not found in the local machine. Please install it if you want to use localhost as the db server. Otherwise, you can provide a remote db server." -ForegroundColor Red
	}
}

if ($allGood){
    Write-Output "All good, all required resources are in place."
}