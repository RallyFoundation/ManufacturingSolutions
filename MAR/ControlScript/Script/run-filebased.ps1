param([System.String]$RootDir, [System.String]$Architecture = "amd64", [ref]$OutResult);

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

[xml]$OemHardwareReportData = [xml]'<OEMOptionalInfo/>';


#$OA3ToolPath = $RootDir + "\OA3Tool\amd64\oa3tool.exe";

$OA3ToolConfigurationFilePath = $RootDir + "\Config\OA3Tool-ServerBased.cfg";
$SKULNPLookupConfigurationFilePath = $RootDir + "\Config\lookup.xml";

#if($Architecture.ToLower() -eq "x86")
#{
#   $OA3ToolPath = $RootDir + "\OA3Tool\x86\oa3tool.exe";
#}

$OA3ToolPath = $RootDir + "\OA3Tool";

$OSInfo = Get-CimInstance -ClassName Win32_OperatingSystem;

$OSInfo;

$OSInfoJson = $OSInfo | ConvertTo-Json;

$OSInfoJson;

[System.String]$OSArchitecture = $OSInfo.CimInstanceProperties.Item("OSArchitecture").Value;

$OSArchitecture;

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

$DPKFilePath = $RootDir + "\Input\";
$DPKOutputFilePath = $RootDir + "\Output\";

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

$OA3OutputBinFilePath = $RootDir + "\Input\" + $TransactionID + ".bin";
$OA3OutputXmlFilePath = $RootDir + "\Input\" + $TransactionID + ".xml";

[System.String]$SerialNumber = "";

[System.String]$Message = "";

"Transaction ID: " | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
$TransactionID | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
$OSInfo | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
$OSArchitecture | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
$OSSKU | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
$OSCaption | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
$OA3ToolPath | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

$OSInfoJson | Out-File -FilePath ($LogPath + "\" + $TransactionID + "-osinfo.json") -Encoding utf8 -Force;

"Serial Number: " | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

#Reads out serial number from BIOS
try
{
   $Message = [System.String]::Format("Reading serial number from BIOS..., {0}", [System.DateTime]::Now);
   $Message;
   $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

   $computerBIOS = Get-CimInstance CIM_BIOSElement;
   $SerialNumber = $computerBIOS.SerialNumber;

   $Message = [System.String]::Format("Serial number: {0}", $SerialNumber);
   $Message;
   $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
 
   if([System.String]::IsNullOrEmpty($SerialNumber) -eq $true)
   {
       $Host.UI.RawUI.BackgroundColor = "Red";
       $Host.UI.RawUI.ForegroundColor = "Yellow";
       Write-Host -Object "Serial number from BIOS is empty!";
       Read-Host -Prompt "Press any key to exit...";
       exit;
   }
}
catch [System.Exception]
{  
    $Message = $Error[0].Exception;
    $Message;
    $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

    $Host.UI.RawUI.BackgroundColor = "Red";
    $Host.UI.RawUI.ForegroundColor = "Yellow";
    Write-Host -Object "Error(s) occurred reading serial number from BIOS!";
    Read-Host -Prompt "Press any key to exit...";
    exit;
}

$SerialNumber | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

#Invokes OA3Tool.exe /Validate to test if there is already a DPK injected
try
{
    $Message = [System.String]::Format("Validating ACPI MSDM table..., {0}", [System.DateTime]::Now);
    $Message;
    $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
       
    $Message = & ($OA3ToolPath) @("/Validate");
    $Message;
    $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

    if($Message.Contains("The operation completed successfully.") -eq $true)
    {

       #$Host.UI.RawUI.BackgroundColor = "Red";
       $Host.UI.RawUI.ForegroundColor = "Yellow";
       Write-Host -Object "The board has already got a DPK injected, and the ACPI MSDM table is NOT empty...a MAR process is going to be taken...";
       #Read-Host -Prompt "Press any key to exit...";
       #exit;
    }
    else
    {
       Write-Host -Object "OK.";
    }
}
catch [System.Exception]
{
    $Message = $Error[0].Exception;
    $Message;
    $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

    $Host.UI.RawUI.BackgroundColor = "Red";
    $Host.UI.RawUI.ForegroundColor = "Yellow";
    Write-Host -Object "Error(s) occurred during ACPI MSDM table validation!";
    Read-Host -Prompt "Press any key to exit...";
    exit;
}

[xml]$SKULPNLookupConfigurationXml = Get-Content -Path $SKULNPLookupConfigurationFilePath -Encoding UTF8;

#Commented out and reserved for future extension.
#if($SKULPNLookupConfigurationXml.Mapping.Key -ne $OSSKU)
#{
#	$Host.UI.RawUI.BackgroundColor = "Red";
#    $Host.UI.RawUI.ForegroundColor = "Yellow";
#    Write-Host -Object "The SKU found in the machine does NOT match the one configured in the SKU / LPN mapping!";
#    Read-Host -Prompt "Press any key to exit...";
#    exit;
#}

#Sets the output file paths for .bin file and .xml file in OA3Tool configuration file;
#And appends OHR data, serial number, CloudConfigurationID to OA3Tool configuration file (if there is not any for each):
#try
#{
#    [xml]$OA3ToolConfigurationXml = Get-Content -Path $OA3ToolConfigurationFilePath -Encoding UTF8;

#    if($OA3ToolConfigurationXml.OA3.ServerBased.Parameters.BusinessID -eq $null)
#    {
#       $Host.UI.RawUI.BackgroundColor = "Red";
#       $Host.UI.RawUI.ForegroundColor = "Yellow";
#       Write-Host -Object "The OA3Tool configuration file does NOT have a BusinessID, please set the BusinessID!";
#       Read-Host -Prompt "Press any key to exit...";
#       exit;
#    }

#    try
#    {
#       Write-Host -Object "Testing connection to KPS...";
#       (New-Object Net.Sockets.TcpClient).Connect($OA3ToolConfigurationXml.OA3.ServerBased.KeyProviderServerLocation.IPAddress, $OA3ToolConfigurationXml.OA3.ServerBased.KeyProviderServerLocation.EndPoint);
#    }
#    catch [System.Exception]
#    {
#      $Message = $Error[0].Exception;
#      $Message;
#      $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

#      $Host.UI.RawUI.BackgroundColor = "Red";
#      $Host.UI.RawUI.ForegroundColor = "Yellow";
#      Write-Host -Object "Connection test to KPS failed! Please check your KPS status and the OA3Tool Configuration File.";
#      Read-Host -Prompt "Press any key to exit...";
#      exit;
#    }

#    $OA3ToolConfigurationXml.OA3.OutputData.AssembledBinaryFile = $OA3OutputBinFilePath;

#    $OA3ToolConfigurationXml.OA3.OutputData.ReportedXMLFile = $OA3OutputXmlFilePath;
    
#    if($OA3ToolConfigurationXml.OA3.ServerBased.Parameters.OEMOptionalInfo -eq $null)
#    {
#        #$OHRNodes = $OA3ToolConfigurationXml.ImportNode($OemHardwareReportData.OEMOptionalInfo, $true);

#		$OHRNodes = $OA3ToolConfigurationXml.ImportNode($OemHardwareReportData.FirstChild, $true);
  
#        #$OA3ToolConfigurationXml.OA3.ServerBased.Parameters.InsertBefore($OHRNodes, $OA3ToolConfigurationXml.OA3.ServerBased.Parameters.CloudConfigurationID);
#        $OA3ToolConfigurationXml.OA3.ServerBased.Parameters.InsertBefore($OHRNodes, $OA3ToolConfigurationXml.OA3.ServerBased.Parameters.SelectSingleNode("BusinessID"));
#    }

#    ##Adding serial number to OA3Tool configuration file:
#    #if($OA3ToolConfigurationXml.OA3.ServerBased.Parameters.SerialNumber -ne $null)
#    #{
#    #   $OA3ToolConfigurationXml.OA3.ServerBased.Parameters.SerialNumber = $SerialNumber;
#    #}
#    #else
#    #{
#    #   [xml]$SerialNumberXml = [System.String]::Format("<SerialNumber>{0}</SerialNumber>", $SerialNumber);

#    #   $SerialNumberNode = $OA3ToolConfigurationXml.ImportNode($SerialNumberXml.FirstChild, $true);

#    #   #$OA3ToolConfigurationXml.OA3.ServerBased.Parameters.InsertAfter($SerialNumberNode, $OA3ToolConfigurationXml.OA3.ServerBased.Parameters.CloudConfigurationID);
#    #   #$OA3ToolConfigurationXml.OA3.ServerBased.Parameters.InsertAfter($SerialNumberNode, $OA3ToolConfigurationXml.OA3.ServerBased.Parameters.SelectSingleNode("CloudConfigurationID"));
#    #   $OA3ToolConfigurationXml.OA3.ServerBased.Parameters.InsertBefore($SerialNumberNode, $OA3ToolConfigurationXml.OA3.ServerBased.Parameters.SelectSingleNode("BusinessID"));

#    #   $OA3ToolConfigurationXml.OA3.ServerBased.Parameters.SerialNumber;
#    #}

#    $OA3ToolConfigurationFilePath = $RootDir + "\Input\" + $TransactionID + "_server.cfg";

#    $OA3ToolConfigurationXml.Save($OA3ToolConfigurationFilePath);
#}
#catch [System.Exception]
#{
#    $Message = $Error[0].Exception;
#    $Message;
#    $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

#    $Host.UI.RawUI.BackgroundColor = "Red";
#    $Host.UI.RawUI.ForegroundColor = "Yellow";
#    Write-Host -Object "Errors occurred!";
#    Read-Host -Prompt "Press any key to exit...";
#    exit;
#}


try
{
   [System.String[]]$DPKInputFiles = [System.IO.Directory]::GetFiles(($RootDir + "\Input\"), "*.xml", [System.IO.SearchOption]::AllDirectories);

   if(($DPKInputFiles -ne $null) -and ($DPKInputFiles.Length -gt 0))
   {
      $DPKFilePath = $DPKInputFiles[0];

      $Message = [System.String]::Format("{0} matche(s) found, and '{1}' has been picked up. {2}", $DPKInputFiles.Length, $DPKFilePath, [System.DateTime]::Now);

      $Message;

      $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
   }

   $DPKFilePath;   
}
catch [System.Exception]
{
  $Message = $Error[0].Exception;
  $Message;
  $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
  
  $Host.UI.RawUI.BackgroundColor = "Red";
  $Host.UI.RawUI.ForegroundColor = "Yellow";
  Write-Host -Object "Errors occurred!";
  Read-Host -Prompt "Press any key to exit...";
  exit;
}

[xml]$OA3ToolConfiguration = [xml]'<?xml version="1.0" encoding="utf-8"?>
                                 <OA3> 
                                  <FileBased> 
                                    <InputKeyXMLFile />
                                    <Parameters/>
                                  </FileBased> 
                                  <OutputData> 
                                    <AssembledBinaryFile /> 
                                    <ReportedXMLFile />
                                  </OutputData> 
                                </OA3>';

#Sets the input file in the oa3tool configuration
$OA3ToolConfiguration.OA3.FileBased.InputKeyXMLFile = $DPKFilePath;

#Sets the ouput .bin file in the oa3tool configuration
$OA3ToolConfiguration.OA3.OutputData.AssembledBinaryFile = $OA3OutputBinFilePath;

#Sets the ouput .xml report file in the oa3tool configuration
$OA3ToolConfiguration.OA3.OutputData.ReportedXMLFile = $OA3OutputXmlFilePath;
    
$OA3ToolConfigurationFilePath = ($OA3OutputXmlFilePath + ".cfg");

#Saves the content of oa3tool configuration to the specified file location
$OA3ToolConfiguration.Save($OA3ToolConfigurationFilePath);

#Invokes OA3Tool.exe /Assemble to generate .bin file and output DPK info xml file
try
{
    $Message = [System.String]::Format("Assembling DPK..., {0}", [System.DateTime]::Now);
    $Message;
    $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
       
    #& ($RootDir + "\OA3Tool9600\amd64\oa3tool.exe") @("/Assemble",  ("/ConfigFile=" + $OA3ToolConfigurationFilePath)) | Out-File -FilePath ($RootDir + "\production-log.log") -Append;
    #Start-Process -FilePath $OA3ToolPath -ArgumentList @("/Assemble",  ("/ConfigFile=" + $OA3ToolConfigurationFilePath)) -Wait -NoNewWindow -RedirectStandardOutput ($LogPath + "\oa3tool-assemble-" + $TransactionID + ".log");
    Start-Process -FilePath $OA3ToolPath -ArgumentList @("/Assemble",  ("/ConfigFile=" + $OA3ToolConfigurationFilePath)) -Wait -NoNewWindow -RedirectStandardOutput ($LogPath + "\" + $TransactionID + "-oa3tool-assemble.log");
}
catch [System.Exception]
{
    $Message = $Error[0].Exception;
    $Message;
    $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

    $Host.UI.RawUI.BackgroundColor = "Red";
    $Host.UI.RawUI.ForegroundColor = "Yellow";
    Write-Host -Object "Errors occurred!";
    Read-Host -Prompt "Press any key to exit...";
    exit;
}

[xml]$ProductKeyInfo = [xml](Get-Content -Path $DPKFilePath); 

$ProductKeyID = $ProductKeyInfo.Key.ProductKeyID;

$ProductKey = $ProductKeyInfo.Key.ProductKey;

#Copies the output DPK xml file to the directory that archives all of the DPK xml files, and renames it to be in the form of "{product_key_id}.assemble.xml"
Copy-Item -Path $DPKFilePath -Destination ($DPKOutputFilePath + $ProductKeyID + ".assemble.xml") -Force;

#Copies the output .bin file to the directory that archives all of the DPK .bin files, and renames it to be in the form of "{product_key_id}.bin"
Copy-Item -Path $OA3OutputBinFilePath -Destination ($DPKOutputFilePath + $ProductKeyID + ".bin") -Force;

#Commented out and reserved for future extension.
#if($SKULPNLookupConfigurationXml.Mapping.Value -ne $ProductKeyInfo.Key.ProductKeyPartNumber)
#{
#	$Host.UI.RawUI.BackgroundColor = "Red";
#	$Host.UI.RawUI.ForegroundColor = "Yellow";
#	Write-Host -Object "The Product Key Part Number regarding the DPK does NOT match the one configured in the SKU / LPN mapping!";
#	Read-Host -Prompt "Press any key to exit...";
#	exit;
#}

##Runs slmgr.vbs /ipk to install the 5x5 here
#Intalls 5x5 with WMI Software Licensing Service here
try
{
    $Message = [System.String]::Format("Installing 5x5..., {0}", [System.DateTime]::Now);
    $Message;

	$ProductKey;

	#Start-Process -FilePath "slmgr.vbs" -ArgumentList @(("/ipk " + $ProductKey));
	#Start-Process -FilePath "slui.exe" -Wait;

	$ComputerName = GC ENV:COMPUTERNAME;
	$SoftwareLicensingService = Get-WmiObject -Query "SELECT * FROM SoftwareLicensingService" -ComputerName $ComputerName;
	$SoftwareLicensingService.InstallProductKey($ProductKey);
	$SoftwareLicensingService.RefreshLicenseStatus();

	$SoftwareLicensingService;

    $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
}
catch [System.Exception]
{
    $Message = $Error[0].Exception;
    $Message;
    $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
    
    $Host.UI.RawUI.BackgroundColor = "Red";
    $Host.UI.RawUI.ForegroundColor = "Yellow";
    Write-Host -Object "Errors occurred!";
    Read-Host -Prompt "Press any key to exit...";
    exit;
}

#Invokes OA3Tool.exe /Report /NoKeyCheck to generate output DPK info xml file
try
{
    $Message = [System.String]::Format("Reporting DPK..., {0}", [System.DateTime]::Now);
    $Message;
    $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

    #& ($RootDir + "\OA3Tool9600\amd64\oa3tool.exe") @("/Report",  ("/ConfigFile=" + $OA3ToolConfigurationFilePath), ("/LogTrace=" + $OA3OutputXmlFilePath + ".log.xml")) | Out-File -FilePath ($RootDir + "\production-log.log") -Append;
    #Start-Process -FilePath $OA3ToolPath -ArgumentList @("/Report",  ("/ConfigFile=" + $OA3ToolConfigurationFilePath), ("/LogTrace=" + $OA3OutputXmlFilePath + ".log.xml")) -Wait -NoNewWindow -RedirectStandardOutput ($LogPath + "\oa3tool-report-" + $TransactionID + ".log");
    Start-Process -FilePath $OA3ToolPath -ArgumentList @("/Report", "/NoKeyCheck",  ("/ConfigFile=" + $OA3ToolConfigurationFilePath), ("/LogTrace=" + $OA3OutputXmlFilePath + ".log.xml")) -Wait -NoNewWindow -RedirectStandardOutput ($LogPath + "\" + $TransactionID + "-oa3tool-report.log");
}
catch [System.Exception]
{
    $Message = $Error[0].Exception;
    $Message;
    $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
  
    $Host.UI.RawUI.BackgroundColor = "Red";
    $Host.UI.RawUI.ForegroundColor = "Yellow";
    Write-Host -Object "Errors occurred!";
    Read-Host -Prompt "Press any key to exit...";
    exit;
}

#Appends ProductKeyID, OHR data and Serial Number to DPK info xml file generated, and changes the key state to 3 (Bound)
try
{
    [xml]$OA3OutputXml = Get-Content -Path $OA3OutputXmlFilePath -Encoding UTF8;

	$OA3OutputXml.Key.ProductKeyState = "3";

	if($OA3OutputXml.Key.ProductKeyID -ne $null)
	{
		$OA3OutputXml.Key.ProductKeyID = $ProductKeyID;
	}
	else
	{
		[xml]$KeyNodeXml = ("<ProductKeyID>{0}</ProductKeyID>" -f $ProductKeyID);
		$KeyNode = $OA3OutputXml.ImportNode($KeyNodeXml.FirstChild, $true);
		$OA3OutputXml.Key.InsertBefore($KeyNode, $OA3OutputXml.Key.SelectSingleNode("ProductKeyState"));
	}
    
    if($OA3OutputXml.Key.OEMOptionalInfo -ne $null)
    {
        $OA3OutputXml.key.RemoveChild($OA3OutputXml.Key.OEMOptionalInfo);
    }

	if($OA3ToolConfigurationXml.OA3.ServerBased.Parameters.OEMOptionalInfo.FirstChild -ne $null)
	{
        $OHRNodes = $OA3OutputXml.ImportNode($OA3ToolConfigurationXml.OA3.ServerBased.Parameters.OEMOptionalInfo, $true);
	}
	else
	{
		$OHRNodes = $OA3OutputXml.ImportNode($OemHardwareReportData.FirstChild, $true);
	}
  
    #$OA3OutputXml.Key.AppendChild($OHRNodes);

    #$OA3OutputXml.Key.InsertAfter($OHRNodes, $OA3OutputXml.Key.HardwareHash);
    $OA3OutputXml.Key.InsertAfter($OHRNodes, $OA3OutputXml.Key.SelectSingleNode("HardwareHash"));

	#Adding serial number to OA3Tool output XML:
    if($OA3OutputXml.Key.SerialNumber -ne $null)
    {
		$OA3OutputXml.Key.SerialNumber = $SerialNumber;
    }
    else
    {
		[xml]$SerialNumberXml = ("<SerialNumber>{0}</SerialNumber>" -f $SerialNumber);

		$SerialNumberNode = $OA3OutputXml.ImportNode($SerialNumberXml.FirstChild, $true);

		$OA3OutputXml.Key.AppendChild($SerialNumberNode);

		$OA3OutputXml.Key.SerialNumber;
    }

    $OA3OutputXml.Save($OA3OutputXmlFilePath);
}
catch [System.Exception]
{
    $Message = $Error[0].Exception;
    $Message;
    $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
}

[xml]$ProductKeyInfo = [xml](Get-Content -Path $OA3OutputXmlFilePath); 

$ProductKeyID = $ProductKeyInfo.Key.ProductKeyID;

#Copies the output DPK xml file to the directory that archives all of the DPK xml files, and renames it to be in the form of "{product_key_id}.report.xml"
$OA3ToolOutputXmlArchivePath = ($DPKOutputFilePath  + $ProductKeyID + ".report.xml");
Copy-Item -Path $OA3OutputXmlFilePath -Destination $OA3ToolOutputXmlArchivePath -Force;

$NewDPKInputFileName = $DPKFilePath + ".bak";
$NewDPKOutputFileName = $OA3OutputXmlFilePath + ".bak";

#Renames the current DPK input xml in the folder that contains the DPK xml files exported from FFKI to be ended with ".bak"
Rename-Item $DPKFilePath -NewName $NewDPKInputFileName;

#Renames the current DPK output xml in the folder that contains the DPK xml files exported from FFKI to be ended with ".bak"
Rename-Item $OA3OutputXmlFilePath -NewName $NewDPKOutputFileName;

$Message = [System.String]::Format("Finish processing '{0}', {1}", $DPKFilePath, [System.DateTime]::Now);
$Message;
$Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;


##Creates DPKID-SN pair, and saves the pair to the xml file of DPKID-SN.xml in the root directory
#if($MatchCount -eq 2)
if([System.String]::IsNullOrEmpty($SerialNumber) -eq $false)
{
   try
   {
      Import-Module ($RootDir + "\Module\PSDPKSNBinder\PowerShellOA3DPKSNBinder.dll");

      #[xml]$ProductKeyInfo = [xml](Get-Content -Path $OA3OutputXmlFilePath); 

	  [xml]$ProductKeyInfo = [xml](Get-Content -Path $OA3ToolOutputXmlArchivePath); 

      $ProductKeyID = $ProductKeyInfo.Key.ProductKeyID;

      if($ProductKeyID -ne $null)
      {
         $PairID = Add-DPKIDSNBinding -ProductKeyID $ProductKeyID -SerialNumber $SerialNumber -TransactionID $TransactionID -PersistencyMode FileSystemXML -FilePath ($RootDir + "\DPKID-SN.xml");

         $Message = [System.String]::Format("Pair created, Product Key ID: {0}, Serial Number: {1}, Pair ID: {2}, {3}", $ProductKeyID, $SerialNumber, $PairID, [System.DateTime]::Now);
         $Message;
         $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

         #$Message = [System.String]::Format("Shutting down..., {0}", [System.DateTime]::Now);

         $Host.UI.RawUI.BackgroundColor = "Green";
         $Host.UI.RawUI.ForegroundColor = "Black";
         $Message = "OA3.0 process completed successfully!";
         $Message;
         $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

         #& ("wpeutil") @("shutdown");

         [System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms");

         $DialogResult = [System.Windows.Forms.MessageBox]::Show("OA3.0 MAR process completed successfully! View OA3Tool /Report result XML now?" , "Success" , 4);

         if($DialogResult -eq "YES")
         {
             Start-Process -FilePath $OA3ToolOutputXmlArchivePath;
         }
		 else
		 {
			 #Stop-Computer -ComputerName "localhost";
		 }
      }
      else
      {
         $Host.UI.RawUI.BackgroundColor = "Red";
         $Host.UI.RawUI.ForegroundColor = "Yellow";
         $Message = [System.String]::Format("Could not find product key ID, {0}", [System.DateTime]::Now);
         $Message;
         $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
      }
   }
   catch [System.Exception]
   {
      $Message = $Error[0].Exception;
      $Message;
      $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
   }
} 