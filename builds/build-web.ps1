param(
    [string]$hostName = "localhost",
    [string]$portNumber = "44300"
)

$host.UI.RawUI.WindowTitle = "OpenCatapult Web";

$rootPath = Split-Path $PSScriptRoot
$webLocation = Join-Path $rootPath "/src/Web/opencatapultweb"

Set-Location -Path $webLocation


function TrustCertificateOnWindows([string]$certFile) 
{    
	$cert = new-object System.Security.Cryptography.X509Certificates.X509Certificate2($webLocation + "/ssl/server.crt")
	
	$certStore = new-object System.Security.Cryptography.X509Certificates.X509Store([System.Security.Cryptography.X509Certificates.StoreName]::Root, [System.Security.Cryptography.X509Certificates.StoreLocation]::CurrentUser)
	$certStore.Open([System.Security.Cryptography.X509Certificates.OpenFlags]::ReadWrite)
	
	$existingCert = $certStore.Certificates.Find([System.Security.Cryptography.X509Certificates.X509FindType]::FindByThumbprint, $cert.Thumbprint, $false)
	if ($existingCert.Count -gt 0) {
		Write-Host "Certificate already trusted. Skipping trust step"
		return
	}
		
	$certStore.Add($cert)
	$certStore.Close()
}

function TrustCertificateOnMac([string]$certFile) 
{    
	&'sudo security add-trusted-cert -d -r trustRoot -k "' + $certFile + '"'
}

#Import-Certificate -Filepath "ssl/server.crt" -CertStoreLocation cert:\CurrentUser\Root
if ($IsLinux) {
    Write-Host "The automated ssl certificate trust is currently only supported on Windows and Mac. For establishing certficate trust on other platforms please refer to the platform specific documentation." -ForegroundColor Yellow 
}
elseif ($IsMac) {
	TrustCertificateOnMac($webLocation + "/ssl/server.crt")
}
else {
	TrustCertificateOnWindows($webLocation + "/ssl/server.crt")
}

npm install

npm run start -- --ssl --host $hostName --port $portNumber --ssl-cert "ssl/server.crt" --ssl-key "ssl/server.key"