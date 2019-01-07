# Copyright (c) Polyrific, Inc 2018. All rights reserved.

$dotnetSdkVersion = [System.Version]"2.1.500"
$allGood = $true

$currentSdkVersion = dotnet --version

if ([System.Version]$currentSdkVersion -lt $dotnetSdkVersion) {
    Write-Output "You need to install dotnet SDK version $dotnetSdkVersion."
    Write-Host "Please run this command in an elevated shell: .\builds\dotnet-install.ps1 -Channel 2.1 -InstallDir `$env:ProgramFiles\dotnet"

    $allGood = $false
}

if ($allGood){
    Write-Output "All good, all required resources are in place."
}