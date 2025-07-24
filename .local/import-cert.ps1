# ⚠️ Run as Administrator

$certUrl   = "https://localhost:8081/_explorer/emulator.pem"
$certPath  = "$env:TEMP\emulatorcert.crt"
$storePath = "Cert:\LocalMachine\Root"
$friendlyName = "CosmosEmulatorContainerCertificate"

Write-Host "🌐 Downloading Cosmos Emulator certificate..." -ForegroundColor Cyan

# Download cert (PowerShell 7+ allows SkipCertificateCheck)
Invoke-WebRequest -Uri $certUrl -OutFile $certPath -SkipCertificateCheck

# Remove older certs with the same friendly name
Get-ChildItem -Path $storePath |
    Where-Object { $_.FriendlyName -eq $friendlyName } |
    Remove-Item -Force -ErrorAction SilentlyContinue

# Import new cert
Import-Certificate -FilePath $certPath -CertStoreLocation $storePath |
    ForEach-Object { $_.FriendlyName = $friendlyName }

Write-Host "🎉 Certificate imported successfully!" -ForegroundColor Green
