$ErrorActionPreference = "Stop";

#Checking PowerShell version and CLR (.NET Framework) version:
$PowerShellVersionInfo = $PSVersionTable;

$PowerShellVersionInfo;

if($OutResult -ne $null)
{
   $OutResult.Value = "Unknown";
}

if($PowerShellVersionInfo.PSVersion.Major -lt 5)
{
   $Host.UI.RawUI.BackgroundColor = "Red";
   $Host.UI.RawUI.ForegroundColor = "Yellow";
   Write-Host -Object "PowerShell 5.1 is required!";
   Read-Host -Prompt "PowerShell 5.1 is required!`nPress any key to exit...";
   exit;

   if($StayInHost -eq $false)
   {
	  $Host.SetShouldExit(1);
   }
}

if($PowerShellVersionInfo.CLRVersion.Major -lt 4)
{
   $Host.UI.RawUI.BackgroundColor = "Red";
   $Host.UI.RawUI.ForegroundColor = "Yellow";
   Write-Host -Object ".NET Framework 4.52 is required!";
   Read-Host -Prompt ".NET Framework 4.52 is required!`nPress any key to exit...";
   exit;

   if($StayInHost -eq $false)
   {
	  $Host.SetShouldExit(1);
   }
}

#if($RootDir -eq $null)
if([System.String]::IsNullOrEmpty($RootDir) -eq $true)
{
   $RootDir = Split-Path -parent $MyInvocation.MyCommand.Definition;

   if($RootDir.EndsWith("\") -eq $true)
   {
      $RootDir = $RootDir.Substring(0, ($RootDir.Length -1));
   }

   if($RootDir.ToLower().EndsWith("\script") -eq $true)
   {
      $RootDir = $RootDir.Substring(0, ($RootDir.ToLower().LastIndexOf("\script")));
   }
}

if($RootDir.EndsWith("\") -eq $true)
{
  $RootDir = $RootDir.Substring(0, ($RootDir.Length -1));
}

#$RootDir = Split-Path -parent $MyInvocation.MyCommand.Definition;

cd $RootDir;

$TransactionID = [System.Guid]::NewGuid().ToString();

#$OA3ToolConfigurationFilePath = (Get-Item -Path "OA3Tool.cfg").FullName;

$OSInfo = Get-CimInstance -ClassName Win32_OperatingSystem;

$OSInfo;

$OSInfoJson = $OSInfo | ConvertTo-Json;

$OSInfoJson;

[System.String]$OSArchitecture = $OSInfo.CimInstanceProperties.Item("OSArchitecture").Value;

$OSArchitecture;

$OA3ToolPath = $RootDir + "\OA3Tool";

if(($OSArchitecture -eq "64-bit") -or ($OSArchitecture.Contains("64")))
{
	$OA3ToolPath += "\amd64\oa3tool.exe"; 
}
else
{
	$OA3ToolPath += "\x86\oa3tool.exe"; 
}

$OSSKU = $OSInfo.CimInstanceProperties.Item("OperatingSystemSKU").Value;

$OSSKU;

$OSCaption = $OSInfo.CimInstanceProperties.Item("Caption").Value;

$OSCaption;

$DPKFilePath = $RootDir + "\Output\";

$LogPath = $RootDir + "\Log";
if([System.IO.Directory]::Exists($LogPath) -eq $false)
{
    [System.IO.Directory]::CreateDirectory($LogPath);
	Start-Sleep -Milliseconds 1000;
}

$OutputPath = $RootDir + "\Output";
if([System.IO.Directory]::Exists($OutputPath) -eq $false)
{
    [System.IO.Directory]::CreateDirectory($OutputPath);
    Start-Sleep -Milliseconds 1000;
}

$InputPath = $RootDir + "\Input";
if([System.IO.Directory]::Exists($InputPath) -eq $false)
{
    [System.IO.Directory]::CreateDirectory($InputPath);
	Start-Sleep -Milliseconds 1000;
}

$TransactionID = [System.Guid]::NewGuid().ToString();

$LogPath = $LogPath + "\production-log.log";

Get-Date | Out-File -FilePath $LogPath -Append;

#$LogPath = (Get-Item -Path "..\..\Log\production-log.log").FullName;

$OA3ToolConfigurationFilePath = $RootDir + "\Config\OA3Tool-ServerBased.cfg";

[xml]$OA3ToolConfigurationXml = Get-Content -Path $OA3ToolConfigurationFilePath -Encoding UTF8;

if($OA3ToolConfigurationXml.OA3.ServerBased.Parameters.BusinessID -eq $null)
{
   $Host.UI.RawUI.BackgroundColor = "Red";
   $Host.UI.RawUI.ForegroundColor = "Yellow";
   Write-Host -Object "The OA3Tool configuration file does NOT have a BusinessID, please set the BusinessID!";
   Read-Host -Prompt "Press any key to exit...";
   exit;
}

#Testing connection to KPS
try
{
   Write-Host -Object "Testing connection to KPS...";
   (New-Object Net.Sockets.TcpClient).Connect($OA3ToolConfigurationXml.OA3.ServerBased.KeyProviderServerLocation.IPAddress, $OA3ToolConfigurationXml.OA3.ServerBased.KeyProviderServerLocation.EndPoint);
}
catch [System.Exception]
{
  $Message = $Error[0].Exception;
  $Message;
  $Message | Out-File -FilePath $LogPath -Append;

  $Host.UI.RawUI.BackgroundColor = "Red";
  $Host.UI.RawUI.ForegroundColor = "Yellow";
  Write-Host -Object "Connection test to KPS failed! Please check your KPS status and the OA3Tool Configuration File.";
  Read-Host -Prompt "Press any key to exit...";
  exit;    
}

$OA3ToolConfigurationXml.OA3.OutputData.AssembledBinaryFile = "OA3.bin";
$OA3ToolConfigurationXml.OA3.OutputData.ReportedXMLFile = "OA3.xml";

#Adding serial number to OA3Tool configuration file:
[System.String]$SerialNumber = $TransactionID;

if($OA3ToolConfigurationXml.OA3.ServerBased.Parameters.SerialNumber -ne $null)
{
   $OA3ToolConfigurationXml.OA3.ServerBased.Parameters.SerialNumber = $SerialNumber;
}
else
{
   [xml]$SerialNumberXml = [System.String]::Format("<SerialNumber>{0}</SerialNumber>", $SerialNumber);

   $SerialNumberNode = $OA3ToolConfigurationXml.ImportNode($SerialNumberXml.FirstChild, $true);

   $OA3ToolConfigurationXml.OA3.ServerBased.Parameters.InsertBefore($SerialNumberNode, $OA3ToolConfigurationXml.OA3.ServerBased.Parameters.SelectSingleNode("BusinessID"));

   $OA3ToolConfigurationXml.OA3.ServerBased.Parameters.SerialNumber;
}

$OA3ToolConfigurationXml.Save($OA3ToolConfigurationFilePath);

$OA3InputConfigFilePath = $RootDir + "\Input\" + $TransactionID + ".cfg.xml";

Copy-Item -Path $OA3ToolConfigurationFilePath -Destination $OA3InputConfigFilePath -Force;

Copy-Item -Path $OA3ToolConfigurationFilePath -Destination ($RootDir + "\Module\AMIToolKit\OA3Tool.cfg") -Force;

Copy-Item -Path $OA3ToolPath -Destination ($RootDir + "\Module\AMIToolKit\MakeKey_Windows\OA3Tool.exe") -Force;

"Transaction ID: " | Out-File -FilePath $LogPath -Append;
$TransactionID | Out-File -FilePath $LogPath -Append;

"Serial Number: " | Out-File -FilePath $LogPath -Append;
$SerialNumber | Out-File -FilePath $LogPath -Append;

#Start

cd ($RootDir + "\Module\AMIToolKit");

.\01_Main.bat

[xml]$ProductKeyInfo = [xml](Get-Content -Path .\OA3.Assemble.xml); 

$ProductKeyID = $ProductKeyInfo.Key.ProductKeyID;

$OA3InputBinFilePath = $RootDir + "\Input\" + $ProductKeyID + "_" + $TransactionID + ".bin";
$OA3InputXmlFilePath = $RootDir + "\Input\" + $ProductKeyID + "_" + $TransactionID + ".assemble.xml";

$ProductKeyInfo = [xml](Get-Content -Path .\OA3.Report.xml); 

$ProductKeyID = $ProductKeyInfo.Key.ProductKeyID;

$OA3OutputBinFilePath = $RootDir + "\Output\" + $ProductKeyID + "_" + $TransactionID + ".bin";
$OA3OutputXmlFilePath = $RootDir + "\Output\" + $ProductKeyID + "_" + $TransactionID + ".report.xml";
$OA3OutputTraceFilePath = $RootDir + "\Output\" + $ProductKeyID + "_" + $TransactionID + ".trace.xml";
$OA3OutputHWDecodeFilePath = $RootDir + "\Output\" + $ProductKeyID + "_" + $TransactionID + ".hwdecode.xml";
$OA3OutputConfigFilePath = $RootDir + "\Output\" + $ProductKeyID + "_" + $TransactionID + ".cfg.xml";

Copy-Item -Path .\OA3.bin -Destination $OA3InputBinFilePath -Force;
Copy-Item -Path .\OA3.Assemble.xml -Destination $OA3InputXmlFilePath -Force;
Copy-Item -Path .\OA3.bin -Destination $OA3OutputBinFilePath -Force;
Copy-Item -Path .\OA3.Report.xml -Destination $OA3OutputXmlFilePath -Force;
Copy-Item -Path .\OA3.Trace.xml -Destination $OA3OutputTraceFilePath -Force;
Copy-Item -Path .\OA3.HWDecode.xml -Destination $OA3OutputHWDecodeFilePath -Force;
Copy-Item -Path .\OA3Tool.cfg -Destination $OA3OutputConfigFilePath -Force;

Remove-Item -Path .\OA3.bin -Force;
Remove-Item -Path .\OA3.Assemble.xml -Force;
Remove-Item -Path .\OA3.Report.xml -Force;
Remove-Item -Path .\OA3.Trace.xml -Force;
Remove-Item -Path .\OA3.HWDecode.xml -Force;

Remove-Item -Path .\Flash_afuwin\OA3.bin -Force;
Remove-Item -Path .\Flash_afuwin\OA3.Assemble.xml -Force;
Remove-Item -Path .\Flash_afuwin\OA3.Report.xml -Force;
Remove-Item -Path .\Flash_afuwin\OA3.Trace.xml -Force;
Remove-Item -Path .\Flash_afuwin\OA3.HWDecode.xml -Force;

Remove-Item -Path .\MakeKey_Windows\OA3.bin -Force;
Remove-Item -Path .\MakeKey_Windows\OA3.xml -Force;
Remove-Item -Path .\MakeKey_Windows\OA3.Trace.xml -Force;
Remove-Item -Path .\MakeKey_Windows\OA3.HWDecode.xml -Force;
Remove-Item -Path .\MakeKey_Windows\OA3Tool.cfg -Force;

"Product Key ID: " | Out-File -FilePath $LogPath -Append;
$ProductKeyID | Out-File -FilePath $LogPath -Append;

cd $RootDir;

##Creates DPKID-SN pair, and saves the pair to the xml file of DPKID-SN.xml in the root directory
if([System.String]::IsNullOrEmpty($SerialNumber) -eq $false)
{
   try
   {
      #Import-Module (Get-Item -Path "..\..\Module\PSDPKSNBinder\PowerShellOA3DPKSNBinder.dll").FullName;

	  Import-Module ($RootDir + "\Module\PSDPKSNBinder\PowerShellOA3DPKSNBinder.dll");

      $ProductKeyInfo = [xml](Get-Content -Path (Get-Item -Path $OA3OutputXmlFilePath).FullName); 

      $ProductKeyID = $ProductKeyInfo.Key.ProductKeyID;

      if($ProductKeyID -ne $null)
      {
         #$PairID = Add-DPKIDSNBinding -ProductKeyID $ProductKeyID -SerialNumber $SerialNumber -TransactionID $TransactionID -PersistencyMode FileSystemXML -FilePath (Get-Item -Path "..\..\DPKID-SN.xml").FullName;

		 $PairID = Add-DPKIDSNBinding -ProductKeyID $ProductKeyID -SerialNumber $SerialNumber -TransactionID $TransactionID -PersistencyMode FileSystemXML -FilePath ($RootDir + "\DPKID-SN.xml");

         $Message = [System.String]::Format("Pair created, Product Key ID: {0}, Serial Number: {1}, Pair ID: {2}, {3}", $ProductKeyID, $SerialNumber, $PairID, [System.DateTime]::Now);
         $Message;
         $Message | Out-File -FilePath $LogPath -Append;
      }
      else
      {
         $Host.UI.RawUI.BackgroundColor = "Red";
         $Host.UI.RawUI.ForegroundColor = "Yellow";
         $Message = [System.String]::Format("Could not find product key ID, {0}", [System.DateTime]::Now);
         $Message;
         $Message | Out-File -FilePath $LogPath -Append;
      }
   }
   catch [System.Exception]
   {
      $Message = $Error[0].Exception;
      $Message;
      $Message | Out-File -FilePath $LogPath -Append;
   }
} 


#Uploading result, config and log files to FFKI API (if configured as enabled)
try
{
	[xml]$FFKIAPISettings = Get-Content -Path ($RootDir + "\Config\ffki-api-config.xml") -Encoding UTF8;

	$FFKIAPISettings;

	if(($FFKIAPISettings.settings.enabled -eq "true") -or ($FFKIAPISettings.settings.enabled -eq "1"))
	{
		[System.String]$FFKIAPIServicePoint = $FFKIAPISettings.settings.servicePoint;

		if($FFKIAPIServicePoint.EndsWith("/") -eq $false)
		{
		   $FFKIAPIServicePoint += "/";
		}

		$FFKIAPIServicePoint;

		$Result = Invoke-RestMethod -Method Get -Uri $FFKIAPIServicePoint;

		$Result;

		if($Result -eq "Welcome to OA3.0!")
		{
			[System.Net.WebClient]$webClient = New-Object -TypeName System.Net.WebClient;

			$FFKIAPIReportUrl = ($FFKIAPIServicePoint + "OA3/Report/");

			$FFKIAPIConfigUrl = ($FFKIAPIServicePoint + "OA3/Config/");

			$FFKIAPILogUrl = ($FFKIAPIServicePoint + "OA3/Log/" + $TransactionID);

			$webClient.UploadFile($FFKIAPIReportUrl, $OA3OutputXmlFilePath);

			$webClient.UploadFile($FFKIAPIConfigUrl, $OA3OutputConfigFilePath);

			$webClient.UploadFile($FFKIAPILogUrl, $OA3OutputTraceFilePath);

			$webClient.UploadFile($FFKIAPILogUrl, $OA3OutputHWDecodeFilePath);

			$webClient.UploadFile($FFKIAPILogUrl, $LogPath);
		}
	}
}
catch [System.Exception]
{
	$Message = $Error[0].Exception;
    $Message;
    $Message | Out-File -FilePath $LogPath -Append;
}


#Shutting down machine
#$Message = [System.String]::Format("Shutting down..., {0}", [System.DateTime]::Now);

$Host.UI.RawUI.BackgroundColor = "Green";
$Host.UI.RawUI.ForegroundColor = "Black";
$Message = "OA3.0 process completed successfully!";
$Message;
$Message | Out-File -FilePath $LogPath -Append;

#& ("wpeutil") @("shutdown");

[System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms");

$DialogResult = [System.Windows.Forms.MessageBox]::Show("OA3.0 process completed successfully! Shutdown unit now?" , "Success" , 4);

if($DialogResult -eq "YES")
{
    Stop-Computer -ComputerName "localhost";
}