param([System.String]$TraceFilePath, [System.String]$DecodeFilePath, [System.String]$ReportFilePath, [System.String]$TransactionID, [System.String]$RootDir, [System.String]$ServiceUrl);

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

if([System.String]::IsNullOrEmpty($TransactionID) -eq $true)
{
    $TransactionID = [System.Guid]::NewGuid().ToString();
}

$LogPath = $RootDir +  "\Log";

$DataProcessingModulePath = $RootDir + "\Module\DataProcessing\PowerShellDataProcessing.dll";

$XsltExtensionObjectPath = $RootDir + "\Module\XsltExtension\XsltExtension.cs";

$LoadingHtmlPath = $RootDir + "\Module\UI\Loading.html";

Import-Module ($DataProcessingModulePath);

Add-Type -Path $XsltExtensionObjectPath;

#$IE = New-Object -COM InternetExplorer.Application;
#$IE.FullScreen = $false;
#$IE.TopLevelContainer = $true;
#$IE.Left = 0;
#$IE.Top = 0;

#while([System.String]::IsNullOrEmpty($ReportFilePath))
#{
#    $ReportFilePath = Get-InputPath -Title "Specify OA3Tool /Report File" -Message "Click 'Browse' to locate the output file from OA3Tool.exe /Report:" -ErrorMessage "Please specify the location to the output file from OA3Tool.exe /Report!";
#}

if([System.String]::IsNullOrEmpty($ReportFilePath) -or [System.String]::IsNullOrEmpty($DecodeFilePath) -or [System.String]::IsNullOrEmpty($TraceFilePath))
#if([System.String]::IsNullOrEmpty($ReportFilePath) -eq $false)
{
    #$IE.Navigate2([System.String]::Format("file:///{0}", $LoadingHtmlPath));
    #$IE.Visible = $true;

    $ReportFilePath = $RootDir + "\Input\" + $TransactionID + "_Report.xml";

	$DecodeFilePath = $RootDir + "\Input\" + $TransactionID + "_Decode.xml";

	$TraceFilePath = $RootDir + "\Input\" + $TransactionID + "_Trace.xml";

    $OA3ToolConfigurationFilePath = $RootDir + "\Config\OA3Tool-FileBased.cfg";


	[xml]$OA3ToolConfigurationXml = Get-Content -Path $OA3ToolConfigurationFilePath -Encoding UTF8;

	$OA3ToolConfigurationXml.OA3.OutputData.ReportedXMLFile = $ReportFilePath;

	$OA3ToolConfigurationFilePath = $RootDir + "\Input\" + $TransactionID + "_Config.xml";

	$OA3ToolConfigurationXml.Save($OA3ToolConfigurationFilePath);


	$OA3ToolPath = $RootDir + "\OA3Tool";

	$OSInfo = Get-CimInstance -ClassName Win32_OperatingSystem;

	$OSInfoJson = $OSInfo | ConvertTo-Json;

	$OSArchitecture = $OSInfo.CimInstanceProperties.Item("OSArchitecture").Value;

	$OSSKU = $OSInfo.CimInstanceProperties.Item("OperatingSystemSKU").Value;

	if($OSArchitecture -eq "64-bit")
	{
		$OA3ToolPath += "\amd64\oa3tool.exe"; 
	}
	else
	{
		$OA3ToolPath += "\x86\oa3tool.exe"; 
	}

	#Invokes OA3Tool.exe /ValidateSMBIOS to test if the SMBIOS data is valid
	#try
	#{
	#	$Message = [System.String]::Format("Validating SMBIOS data..., {0}", [System.DateTime]::Now);
	#	$Message;
       
	#	$Message = & ($OA3ToolPath) @("/ValidateSMBIOS");
	#	$Message;

	#	if(($Message.Contains("The operation completed successfully.") -eq $false) -and ($Message.Contains("SMBIOS has valid data.") -eq $false))
	#	{

	#	   $Host.UI.RawUI.BackgroundColor = "Red";
	#	   $Host.UI.RawUI.ForegroundColor = "Yellow";
	#	   Write-Host -Object "The SMBIOS data in the board is invalid!";
	#	   #Read-Host -Prompt "Press any key to exit...";
	#	   #exit;
	#	}
	#	else
	#	{
	#	   Write-Host -Object "OK.";
	#	}
	#}
	#catch [System.Exception]
	#{
	#	$Message = $Error[0].Exception;
	#	$Message;

	#	$Host.UI.RawUI.BackgroundColor = "Red";
	#	$Host.UI.RawUI.ForegroundColor = "Yellow";
	#	Write-Host -Object "Error(s) occurred during SMBIOS data validation!";
	#	Read-Host -Prompt "Press any key to exit...";
	#	exit;
	#}

	#Invokes OA3Tool.exe /Validate to test if there is already a DPK injected
	try
	{
		$Message = [System.String]::Format("Validating ACPI MSDM table..., {0}", [System.DateTime]::Now);
		$Message;
       
		$Message = & ($OA3ToolPath) @("/Validate");
		$Message;

		if($Message.Contains("The operation completed successfully.") -eq $false)
		{

		   $Host.UI.RawUI.BackgroundColor = "Red";
		   $Host.UI.RawUI.ForegroundColor = "Yellow";
		   Write-Host -Object "The board has NOT got a DPK injected, and the ACPI MSDM table is empty!";
		   Read-Host -Prompt "Press any key to exit...";
		   exit;
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

		$Host.UI.RawUI.BackgroundColor = "Red";
		$Host.UI.RawUI.ForegroundColor = "Yellow";
		Write-Host -Object "Error(s) occurred during ACPI MSDM table validation!";
		Read-Host -Prompt "Press any key to exit...";
		exit;
	}

	#Invokes OA3Tool.exe /Report to generate output DPK info xml file
	try
	{
		$Message = [System.String]::Format("Reporting DPK..., {0}", [System.DateTime]::Now);
		$Message;
		$Message | Out-File -FilePath ($LogPath + "\validation-log.log") -Append;

		Start-Process -FilePath $OA3ToolPath -ArgumentList @("/Report",  ("/ConfigFile=`"" + $OA3ToolConfigurationFilePath + "`""), ("/LogTrace=`"" + $TraceFilePath + "`"")) -Wait -NoNewWindow -RedirectStandardOutput ($LogPath + "\" + $TransactionID + "-oa3tool-report.log");
	}
	catch [System.Exception]
	{
		$Message = $Error[0].Exception;
		$Message;
		$Message | Out-File -FilePath ($LogPath + "\validation-log.log") -Append;
  
		$Host.UI.RawUI.BackgroundColor = "Red";
		$Host.UI.RawUI.ForegroundColor = "Yellow";
		Write-Host -Object "Errors occurred!";
		Read-Host -Prompt "Press any key to exit...";
		exit;
	}

	#Invokes OA3Tool.exe /CheckHwHash to generate log trace
	#try
	#{
	#	$Message = [System.String]::Format("Checking haraware hash..., {0}", [System.DateTime]::Now);
	#	$Message;
	#	$Message | Out-File -FilePath ($LogPath + "\validation-log.log") -Append;

	#	Start-Process -FilePath $OA3ToolPath -ArgumentList @(("/CheckHwHash=" + $ReportFilePath), ("/LogTrace=" + $TraceFilePath)) -Wait -NoNewWindow -RedirectStandardOutput ($LogPath + "\" + $TransactionID + "-oa3tool-checkhwhash.log");
	#}
	#catch [System.Exception]
	#{
	#	$Message = $Error[0].Exception;
	#	$Message;
	#	$Message | Out-File -FilePath ($LogPath + "\validation-log.log") -Append;
  
	#	$Host.UI.RawUI.BackgroundColor = "Red";
	#	$Host.UI.RawUI.ForegroundColor = "Yellow";
	#	Write-Host -Object "Errors occurred!";
	#	Read-Host -Prompt "Press any key to exit...";
	#	exit;
	#}

	#Invokes OA3Tool.exe /DecodeHwHash to generate decoded hardware hash info
	try
	{
		$Message = [System.String]::Format("Decoding hardware hash..., {0}", [System.DateTime]::Now);
		$Message;
		$Message | Out-File -FilePath ($LogPath + "\validation-log.log") -Append;

		Start-Process -FilePath $OA3ToolPath -ArgumentList @(("/DecodeHwHash=`"" + $ReportFilePath + "`""), ("/LogTrace=`"" + $DecodeFilePath + "`"")) -Wait -NoNewWindow -RedirectStandardOutput ($LogPath + "\" + $TransactionID + "-oa3tool-decode.log");
	}
	catch [System.Exception]
	{
		$Message = $Error[0].Exception;
		$Message;
		$Message | Out-File -FilePath ($LogPath + "\validation-log.log") -Append;
  
		$Host.UI.RawUI.BackgroundColor = "Red";
		$Host.UI.RawUI.ForegroundColor = "Yellow";
		Write-Host -Object "Errors occurred!";
		Read-Host -Prompt "Press any key to exit...";
		exit;
	}
}

[System.String]$ReportTraceString = Get-Content -Path $TraceFilePath;

if($ReportTraceString.EndsWith("]>") -and $ReportTraceString.Contains("<![CDATA"))
{
   $ReportTraceCData = $ReportTraceString.Substring($ReportTraceString.IndexOf("<![CDATA"));

   $ReportTraceString = $ReportTraceString.Replace($ReportTraceCData, "");

   $ReportTraceString = $ReportTraceString.Insert($ReportTraceString.IndexOf("</HardwareVerificationData>"), $ReportTraceCData);
}

$ReportTraceString = $ReportTraceString.Substring(0, ($ReportTraceString.LastIndexOf(">") + 1));

[xml]$ReportTrace = (New-Object -TypeName System.Xml.XmlDocument); #[xml](Get-Content -Path $TraceFilePath);

$ReportTrace.LoadXml($ReportTraceString);

[xml]$HardwareHashDecode = [xml](Get-Content -Path $DecodeFilePath);

[xml]$ProductKeyInfo = [xml](Get-Content -Path $ReportFilePath);

#$SMB = Get-WmiObject MSSmBios_RawSMBiosTables -Namespace Root\Wmi;
#$Monitors = Get-WmiObject -Namespace Root\Wmi -Clas WmiMonitorId;
#$SystemInfo = Get-CimInstance -ClassName Win32_ComputerSystem;
#$SKU = $SystemInfo.SystemSKUNumber; 
#$ProcessorInfo = Get-WmiObject -ClassName Win32_Processor;
#$ProcessorInfo | ConvertTo-Json;
 
$ProductKeyID = $ProductKeyInfo.Key.ProductKeyID; #$ReportTrace.HardwareVerificationReport.HardwareVerificationData.Environment.p.Where({$_.n -eq "ProductKeyID"})[0].'#text'#
$ExpectedOSType = "FullOS";
$ProcessorModel = $ReportTrace.HardwareVerificationData.Hardware.CPUID.p.Where({$_.name -eq "ProcessorModel"})[0].'#text'; #$env:PROCESSOR_IDENTIFIER;
#$ProcessorModel = $ReportTrace.HardwareVerificationReport.HardwareVerificationData.Hardware.SMBIOS.Processor.p.Where({$_.n -eq "Version"})[0].'#text'; #$env:PROCESSOR_IDENTIFIER;

$XsltArgs = New-Object -TypeName "System.Collections.Generic.Dictionary``2[System.String,System.Object]";

$XsltArgs.Add("transactionId", $TransactionID);
$XsltArgs.Add("productKeyId", $ProductKeyID);
$XsltArgs.Add("osType", $ExpectedOSType);
$XsltArgs.Add("processorModel", $ProcessorModel);

$XsltExtObjs = New-Object -TypeName "System.Collections.Generic.Dictionary``2[System.String,System.Object]";
$XsltExtObjs.Add("HHValidation.XsltExt", (New-Object -TypeName "HarwareHashValidation.XsltExtension"));

$XsltPath = $RootDir + "\Xslt\xslt-check-hh-xml.xslt";
$XsltHtmlPath = $RootDir + "\Xslt\xslt-check-hh-html.xslt";

$ResultXmlFilePath = $RootDir + "\Output\" + $TransactionID + "_" + $ProductKeyID + "_Result.xml";
$ResultJsonFilePath = $RootDir + "\Output\" + $TransactionID + "_" + $ProductKeyID + "_Result.json";
$ResultHtmlFilePath = $RootDir + "\Output\" + $TransactionID + "_" + $ProductKeyID + "_Result.html";

[xml]$ResultXml = Get-TransformedXml -XmlDocument $HardwareHashDecode -XsltPath $XsltPath -OutputEncoding "utf-8" -XsltArguments $XsltArgs -XsltExtendedObjects $XsltExtObjs;

$ResultXml.InnerXml | Out-File -Encoding utf8 -FilePath $ResultXmlFilePath -Force;

$ResultJson = Get-JsonFromXml -XmlString $ResultXml.InnerXml -Indent;
$ResultJson | Out-File -Encoding utf8 -FilePath $ResultJsonFilePath -Force;

#if(($ResultXml.TestItems.TotalPhysicalRAM.Result -eq "Failed") -or ($ResultXml.TestItems.PrimaryDiskTotalCapacity.Result -eq "Failed"))
#{
    $SMBIOSDumpPath = ($RootDir + "\Output\" + $TransactionID + "_" + $ProductKeyID + "_SMBIOS_Dump.txt");
    $Message = [System.String]::Format("Dumping SMBIOS data..., {0}", [System.DateTime]::Now);
	$Message;
	$Message | Out-File -FilePath ($LogPath + "\validation-log.log") -Append;
    Start-Process -FilePath "powershell" -ArgumentList @("-ExecutionPolicy ByPass", ("-File `"" + $RootDir + "\Module\Dump\SMBIOS-Dump.ps1`"")) -Wait -NoNewWindow -RedirectStandardOutput $SMBIOSDumpPath; 
#}

#if(($ResultXml.TestItems.DisplaySizePhysicalH.Result -eq "Failed") -or ($ResultXml.TestItems.DisplaySizePhysicalY.Result -eq "Failed"))
#{
    $MonitorDumpPath = ($RootDir + "\Output\" + $TransactionID + "_" + $ProductKeyID + "_MonitorSize_Dump.txt");
    $Message = [System.String]::Format("Dumping Monitor data..., {0}", [System.DateTime]::Now);
	$Message;
	$Message | Out-File -FilePath ($LogPath + "\validation-log.log") -Append;
    Start-Process -FilePath "powershell" -ArgumentList @("-ExecutionPolicy ByPass", ("-File `"" + $RootDir + "\Module\Dump\MonitorSize.ps1`"")) -Wait -NoNewWindow -RedirectStandardOutput $MonitorDumpPath; 
#}

#$ResultHtml = Do-XsltTransformation -XmlString $HardwareHashDecode.InnerXml -XsltPath $XsltHtmlPath -OutputEncoding "utf-8" -XsltArguments $XsltArgs;
$XsltArgs = New-Object -TypeName "System.Collections.Generic.Dictionary``2[System.String,System.Object]";
$XsltArgs.Add("transactionId", $TransactionID);
$XsltArgs.Add("productKeyId", $ProductKeyID);
$XsltArgs.Add("mode", "online");

$ResultHtml = Do-XsltTransformation -XmlString $ResultXml.InnerXml -XsltPath $XsltHtmlPath -OutputEncoding "utf-8" -XsltArguments $XsltArgs -XsltExtendedObjects $XsltExtObjs;
$ResultHtml | Out-File -Encoding utf8 -FilePath $ResultHtmlFilePath -Force;

$SMBInfo = Get-WmiObject MSSmBios_RawSMBiosTables -Namespace Root\Wmi;
$MonitorInfo = Get-WmiObject -Namespace Root\Wmi -Clas WmiMonitorId;
#$ProcessorInfo = Get-WmiObject -ClassName Win32_Processor;
$SystemInfo = Get-CimInstance -ClassName Win32_ComputerSystem;

$SMBInfoJson = ($SMBInfo | ConvertTo-Json);
$MonitorInfoJson = ($MonitorInfo | ConvertTo-Json);
#$ProcessorInfoJson = ($ProcessorInfo | ConvertTo-Json);
$SystemInfoJson =  ($SystemInfo | ConvertTo-Json);

$ReportTraceJson = (Get-JsonFromXml -XmlString $ReportTrace.InnerXml -Indent);
$HardwareHashDecodeJson = (Get-JsonFromXml -XmlString $HardwareHashDecode.InnerXml -Indent);
$ProductKeyInfoJson = (Get-JsonFromXml -XmlString $ProductKeyInfo.InnerXml -Indent);

$SMBInfoJson | Out-File -Encoding utf8 -FilePath ($RootDir + "\Output\" + $TransactionID + "_" + $ProductKeyID + "_SMBInfo.json") -Force;
$MonitorInfoJson | Out-File -Encoding utf8 -FilePath ($RootDir + "\Output\" + $TransactionID + "_" + $ProductKeyID + "_MonitorInfo.json") -Force;
#$ProcessorInfoJson | Out-File -Encoding utf8 -FilePath ($RootDir + "\Result\processorInfo_" + $TransactionID + "_" + $ProductKeyID + ".json") -Force;
$SystemInfoJson | Out-File -Encoding utf8 -FilePath ($RootDir + "\Output\" + $TransactionID + "_" + $ProductKeyID + "_SystemInfo.json") -Force;

$ReportTraceJson | Out-File -Encoding utf8 -FilePath ($RootDir + "\Output\" + $TransactionID + "_" + $ProductKeyID + "_Trace.json") -Force;
$HardwareHashDecodeJson | Out-File -Encoding utf8 -FilePath ($RootDir + "\Output\" + $TransactionID + "_" + $ProductKeyID + "_Decode.json") -Force;
$ProductKeyInfoJson | Out-File -Encoding utf8 -FilePath ($RootDir + "\Output\" + $TransactionID + "_" + $ProductKeyID + "_Report.json") -Force;

$TraceXmlOutputPath = ($RootDir + "\Output\" + $TransactionID + "_" + $ProductKeyID + "_Trace.xml");
$DecodeXmlOutputPath = ($RootDir + "\Output\" + $TransactionID + "_" + $ProductKeyID + "_Decode.xml");
$ReportXmlOutputPath = ($RootDir + "\Output\" + $TransactionID + "_" + $ProductKeyID + "_Report.xml");

Copy-Item -Path $TraceFilePath -Destination $TraceXmlOutputPath -Force;
Copy-Item -Path $DecodeFilePath -Destination $DecodeXmlOutputPath -Force;
Copy-Item -Path $ReportFilePath -Destination $ReportXmlOutputPath -Force;

$FilePathsForZip = @($TraceXmlOutputPath, $DecodeXmlOutputPath, $ReportXmlOutputPath, $ResultXmlFilePath, $ResultJsonFilePath, $ResultHtmlFilePath, $SMBIOSDumpPath, $MonitorDumpPath);
$ZippedFilePath = ($RootDir + "\Output\" + $TransactionID + "_" + $ProductKeyID + "_All.zip");

New-Zip -FilesToZip $FilePathsForZip -ZippedFilePath $ZippedFilePath -VirtualPathInZip ($ProductKeyID +"\"+ $TransactionID);

#$ProductKeyInfo.Save($RootDir + "\Input\" + $TransactionID + "_Report.xml");

#$IE.Navigate2([System.String]::Format("file:///{0}?TransactionID={1}&ProductKeyID={2}", $ResultHtmlFilePath, $TransactionID, $ProductKeyID));
#$IE.Visible=$true;

#Start-Process -FilePath ("microsoft-edge:file://" + $ResultHtmlFilePath);

Start-Process -FilePath ([System.String]::Format("file:///{0}?TransactionID={1}&ProductKeyID={2}", $ResultHtmlFilePath, $TransactionID, $ProductKeyID));

#$ServiceUrl = "http://127.0.0.1:3000/engineering/";

if([System.String]::IsNullOrEmpty($ServiceUrl) -eq $false)
{
	$BodyJsonString = '"uid": "",
		"transactionId": "{0}",
		"type": "",
		"value": "",
		"time": "{1}",
		"systemInfo": {2},
		"smbInfo": {3},
		"monitorInfo": {4},
		"oa3Report": {5},
		"oa3ReportTrace": {6},
		"oa3HwDecode": {7},
		"validationResult": {8}';

	$CurrDate = Get-Date;

	#$BodyJsonString = [System.String]::Format($BodyJsonString, $TransactionID, $CurrDate, $SystemInfoJson, $SMBInfoJson, $MonitorInfoJson, $ProductKeyInfoJson, $ReportTraceJson, $HardwareHashDecodeJson, $ResultJson);

	#$BodyJsonString = [System.String]::Format($BodyJsonString, $TransactionID, $CurrDate, $SystemInfoJson, "", "", "", "", "", "");

	$BodyJsonString = [System.String]::Format($BodyJsonString, $TransactionID, $CurrDate, $SystemInfoJson, "", $MonitorInfoJson, "", "", "", $ResultJson);

	$BodyJsonString = "{" + $BodyJsonString + "}";

	#$Body = ConvertFrom-Json -InputObject $BodyJsonString;

	Invoke-RestMethod -Method Post -Body $BodyJsonString -Uri $ServiceUrl;
}