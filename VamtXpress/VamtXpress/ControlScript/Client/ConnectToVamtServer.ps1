$clientHostName = hostname

#If you want to use the client machine's IPv4 Address instead, just uncomment the following 2 lines, replacing the value of -InterfaceAlias corrosponding to your NIC configuration:
#$clientIPv4Config = Get-NetIPAddress -AddressFamily IPv4 -InterfaceAlias "Wi-Fi"
#$clientHostName = $clientIPv4Config.IPAddress.ToString()

$clientUserName = "testusr002"
$clientPassword = ""
$clientNetBiosDomainUserName = $clientHostName + "\" + $clientUserName

$serverHostname = "MININT-3988G0R";
$serverUserName = "VAMT";
$serverPassword = "W@lcome!";
$serverSecurePassword = ConvertTo-SecureString $serverPassword -AsPlainText -Force
$serverNetBiosDomainUserName = $serverHostname + "\" + $serverUserName
$serverCredentail = New-Object System.Management.Automation.PSCredential($serverNetBiosDomainUserName, $serverSecurePassword)

$wrmClientConfigValue = '@{TrustedHosts="' + $serverHostname + '"}'

Enable-PSRemoting -Force -SkipNetworkProfileCheck

winrm set winrm/config/client $wrmClientConfigValue

try
{
  Invoke-Command -ComputerName $serverHostname -Credential $serverCredentail -ConfigurationName Microsoft.PowerShell32 {param($hostname, $username, $passwd) Set-ExecutionPolicy -ExecutionPolicy Bypass -Force -Scope Process; C:\InvokeVamt.ps1 -ClientHostName $hostname -ClientUserName $username -ClientPassword $passwd} -ArgumentList $clientHostName, $clientNetBiosDomainUserName, $clientPassword
}
catch [Exception]
{
  $session = New-PSSession -ComputerName $serverHostname -ConfigurationName Microsoft.PowerShell32 -Credential $serverCredentail
  Invoke-Command -Session $session -ScriptBlock {param($hostname, $username, $passwd)$clientHostname=$hostname;$clientUsername=$username;$clientPassword=$passwd;} -ArgumentList $clientHostName, $clientNetBiosDomainUserName, $clientPassword
  Invoke-Command -Session $session {Set-ExecutionPolicy -ExecutionPolicy Bypass -Force -Scope Process; C:\InvokeVamt.ps1 -ClientHostName $clientHostname -ClientUserName $clientUsername -ClientPassword $clientPassword}
} 

winrm set winrm/config/client '@{TrustedHosts=""}'

Disable-PSRemoting -Force