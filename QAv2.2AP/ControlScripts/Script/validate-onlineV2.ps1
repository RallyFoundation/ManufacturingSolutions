param([System.String]$TraceFilePath, [System.String]$DecodeFilePath, [System.String]$ReportFilePath, [System.String]$TransactionID, [System.String]$RootDir, [System.String]$ServiceUrl, [System.Boolean]$ByPassUI = 0, [System.Boolean]$StayInHost = 0, [ref]$OutResult);

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
if([System.IO.Directory]::Exists($LogPath) -eq $false)
{
    [System.IO.Directory]::CreateDirectory($LogPath);
	Start-Sleep -Milliseconds 1000;
}

$OutputPath = $RootDir +  "\Output";
if([System.IO.Directory]::Exists($OutputPath) -eq $false)
{
    [System.IO.Directory]::CreateDirectory($OutputPath);
    Start-Sleep -Milliseconds 1000;
}

$InputPath = $RootDir +  "\Input";
if([System.IO.Directory]::Exists($InputPath) -eq $false)
{
    [System.IO.Directory]::CreateDirectory($InputPath);
	Start-Sleep -Milliseconds 1000;
}

$ValidationModulePath = $RootDir + "\Module\Validation\QA.PowerShell.Validation.dll";

$DataProcessingModulePath = $RootDir + "\Module\DataProcessing\QA.PowerShell.DataProcessing.dll";

$XsltExtensionObjectPath = $RootDir + "\Module\Extension\QA.Extension.Business.dll";#$RootDir + "\Module\XsltExtension\XsltExtension.cs";

$QAModelObjectPath = $RootDir + "\Module\Validation\QA.Model.dll";

$LoadingHtmlPath = $RootDir + "\Module\UI\Loading.html";

$OA3ReportXmlSchemaPath = $RootDir + "\Data\SchemaOA3ToolReportKey.xsd";

[System.String]$Message = [System.String]::Format("Transaction Started: {0}..., {1}", $TransactionID, [System.DateTime]::Now);
$Message;
$Message | Out-File -FilePath ($LogPath + "\validation-log.log") -Append;

Add-Type -Path $XsltExtensionObjectPath;

Import-Module ($DataProcessingModulePath);

Add-Type -Path $QAModelObjectPath;

Import-Module ($ValidationModulePath);

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

	[System.String]$OSArchitecture = $OSInfo.CimInstanceProperties.Item("OSArchitecture").Value;

	$OSArchitecture;

	$OSSKU = $OSInfo.CimInstanceProperties.Item("OperatingSystemSKU").Value;

	$OSSKU;

	if(($OSArchitecture -eq "64-bit") -or ($OSArchitecture.Contains("64")))
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

	$ShouldByPassDPKChecking = $false;

	#Invokes OA3Tool.exe /Validate to test if there is already a DPK injected
	try
	{
		$Message = [System.String]::Format("Validating ACPI MSDM table..., {0}", [System.DateTime]::Now);
		$Message;

		$Message | Out-File -FilePath ($LogPath + "\validation-log.log") -Append;
       
		$Message = & ($OA3ToolPath) @("/Validate");
		$Message | Out-File -FilePath ($LogPath + "\validation-log.log") -Append;

		$Message | Out-File -FilePath ($LogPath + "\" + $TransactionID + "-oa3tool-validate.log") -Force;

		$Message = Get-Content -Path ($LogPath + "\" + $TransactionID + "-oa3tool-validate.log");

		if(($Message.Contains("0x80070490") -eq $true) -or ($Message.Contains("Error: OEM Activation Tool failed to find the ACPI MSDM table") -eq $true)) #if($Message.Contains("The operation completed successfully.") -eq $false)
		{
		   #$Host.UI.RawUI.BackgroundColor = "Red";
		   $Host.UI.RawUI.ForegroundColor = "Yellow";
		   #Write-Host -Object "The board has NOT got a DPK injected, and the ACPI MSDM table is empty!";
		   $Message = "The board has NOT got a DPK injected, and the ACPI MSDM table is empty! No Key Check will be taken.";

		   $Message | Out-File -FilePath ($LogPath + "\validation-log.log") -Append;

		   Read-Host -Prompt "The board has NOT got a DPK injected, and the ACPI MSDM table is empty! `nPress any key to continue without DPK checking...";

		   $ShouldByPassDPKChecking = $true;
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

		"Error(s) occurred during ACPI MSDM table validation!" | Out-File -FilePath ($LogPath + "\validation-log.log") -Append;
		$Message | Out-File -FilePath ($LogPath + "\validation-log.log") -Append;

		$Host.UI.RawUI.BackgroundColor = "Red";
		$Host.UI.RawUI.ForegroundColor = "Yellow";
		#Write-Host -Object "Error(s) occurred during ACPI MSDM table validation!";
		Read-Host -Prompt "Error(s) occurred during ACPI MSDM table validation!`nPress any key to exit...";
		exit;
		
	}
	finally
	{
	    if(($Error.Count -gt 0) -and ($StayInHost -eq $false))
		{
			$Host.SetShouldExit(1);
		}
	}

	#Invokes OA3Tool.exe /Report to generate output DPK info xml file
	try
	{
		$Message = [System.String]::Format("Reporting DPK..., {0}", [System.DateTime]::Now);
		$Message;
		$Message | Out-File -FilePath ($LogPath + "\validation-log.log") -Append;

		if($ShouldByPassDPKChecking -eq $false)
		{
		    Start-Process -FilePath $OA3ToolPath -ArgumentList @("/Report",  ("/ConfigFile=`"" + $OA3ToolConfigurationFilePath + "`""), ("/LogTrace=`"" + $TraceFilePath + "`"")) -Wait -NoNewWindow -RedirectStandardOutput ($LogPath + "\" + $TransactionID + "-oa3tool-report.log");
		}
		else
		{
		    Start-Process -FilePath $OA3ToolPath -ArgumentList @("/Report", "/NoKeyCheck",  ("/ConfigFile=`"" + $OA3ToolConfigurationFilePath + "`""), ("/LogTrace=`"" + $TraceFilePath + "`"")) -Wait -NoNewWindow -RedirectStandardOutput ($LogPath + "\" + $TransactionID + "-oa3tool-report.log");
		}
	}
	catch [System.Exception]
	{
		$Message = $Error[0].Exception;
		$Message;
		$Message | Out-File -FilePath ($LogPath + "\validation-log.log") -Append;
  
		$Host.UI.RawUI.BackgroundColor = "Red";
		$Host.UI.RawUI.ForegroundColor = "Yellow";
		Write-Host -Object "Errors occurred!";
		Read-Host -Prompt "Errors occurred!`nPress any key to exit...";
		exit;
	}
    finally
	{
	   if(($Error.Count -gt 0) -and ($StayInHost -eq $false))
	   {
		  $Host.SetShouldExit(1);
	   }
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
}

#Validate OA3Tool /Report file against schema:
try
{
    [xml]$ReportXml = Get-Content -Path $ReportFilePath -Encoding UTF8;
    [xml]$SchemaXml = Get-Content -Path $OA3ReportXmlSchemaPath -Encoding UTF8;

    [xml]$SchemaValidationResult = New-XmlSchemaValidation -XmlString $ReportXml.InnerXml -XmlSchemaString $SchemaXml.InnerXml -OutputFormat "xml";

    $SchemaValidationResult.validationResult.errorCount;
    $SchemaValidationResult.validationResult.errorMessage;

    [int]$schemaErrorCount = $SchemaValidationResult.validationResult.errorCount;

    if($schemaErrorCount -gt 0)
    {
        $Message = $SchemaValidationResult.validationResult.errorMessage;

        $Host.UI.RawUI.BackgroundColor = "Red";
	    $Host.UI.RawUI.ForegroundColor = "Yellow";
	    Write-Host -Object "The file provided for the OA3Tool report result file failed to pass the XML schema validation!"; 
        Write-Host $Message;
	    Read-Host -Prompt ($Message + "`nThe file provided for the OA3Tool report result file failed to pass the XML schema validation! `nPress any key to exit...");
        $Error.Add("Invalid OA3Tool /Report XML File.");
		exit;
		#$Host.SetShouldExit(1);
    }
}
catch [System.Exception]
{
    $Message = $Error[0].Exception;

	"Error(s) occurred during xml schema validation!" | Out-File -FilePath ($LogPath + "\validation-log.log") -Append;
	$Message | Out-File -FilePath ($LogPath + "\validation-log.log") -Append;

	$Host.UI.RawUI.BackgroundColor = "Red";
	$Host.UI.RawUI.ForegroundColor = "Yellow";
	#Write-Host -Object "Error(s) occurred during xml schema validation!";
    Write-Host $Message;
	Read-Host -Prompt ($Message + "`nError(s) occurred during xml schema validation! `nPress any key to exit...");
    exit;
}
finally
{
    if(($Error.Count -gt 0) -and ($StayInHost -eq $false))
	{
		$Host.SetShouldExit(1);
	}
}

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

	"Errors occurred!" | Out-File -FilePath ($LogPath + "\validation-log.log") -Append;
	$Message | Out-File -FilePath ($LogPath + "\validation-log.log") -Append;
  
	$Host.UI.RawUI.BackgroundColor = "Red";
	$Host.UI.RawUI.ForegroundColor = "Yellow";
	#Write-Host -Object "Errors occurred!";
	Read-Host -Prompt "Errors occurred!`nPress any key to exit...";
	exit;
}
finally
{
    if(($Error.Count -gt 0) -and ($StayInHost -eq $false))
    {
	    $Host.SetShouldExit(1);
    }
}

if([System.IO.File]::Exists($DecodeFilePath) -eq $false)
{
   $Host.UI.RawUI.BackgroundColor = "Red";
   $Host.UI.RawUI.ForegroundColor = "Yellow";
   Write-Host -Object ("Errors occurred decoding hardware hash. Please see the log file for more details.");
   Read-Host -Prompt ([System.String]::Format("Errors occurred decoding hardware hash! `nPlease see the log file (`"{0}`") for more details...`nPress any key to exit...", ($LogPath + "\" + $TransactionID + ".log")));
   exit;

   if($StayInHost -eq $false)
   {
      $Host.SetShouldExit(1);
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


if($ShouldByPassDPKChecking -eq $true)
{
   $ProductKeyID = "NO_KEY_CHECK";
}
else
{
   $ProductKeyID = $ProductKeyInfo.Key.ProductKeyID; #$ReportTrace.HardwareVerificationReport.HardwareVerificationData.Environment.p.Where({$_.n -eq "ProductKeyID"})[0].'#text'#
} 

$ExpectedOSType = "FullOS";
$ProcessorModel = $ReportTrace.HardwareVerificationData.Hardware.CPUID.p.Where({$_.name -eq "ProcessorModel"})[0].'#text'; #$env:PROCESSOR_IDENTIFIER;
#$ProcessorModel = $ReportTrace.HardwareVerificationReport.HardwareVerificationData.Hardware.SMBIOS.Processor.p.Where({$_.n -eq "Version"})[0].'#text'; #$env:PROCESSOR_IDENTIFIER;

$SerialNumber = $ReportTrace.HardwareVerificationData.Hardware.SMBIOS.System.p.Where({$_.name -eq "SerialNumber"})[0].'#text'; 

$XsltArgs = New-Object -TypeName "System.Collections.Generic.Dictionary``2[System.String,System.Object]";

$XsltArgs.Add("transactionId", $TransactionID);
$XsltArgs.Add("productKeyId", $ProductKeyID);
$XsltArgs.Add("osType", $ExpectedOSType);
$XsltArgs.Add("processorModel", $ProcessorModel);

$XsltExtObjs = New-Object -TypeName "System.Collections.Generic.Dictionary``2[System.String,System.Object]";
$XsltExtObjs.Add("HHValidation.XsltExt", (New-Object -TypeName "QA.Extension.Business.XsltExtension"));

#$XsltPath = $RootDir + "\Xslt\xslt-check-hh-xml.xslt";
$XsltHtmlPath = $RootDir + "\Xslt\xslt-check-hh-html.xslt";

$ResultXmlFilePath = $RootDir + "\Output\" + $TransactionID + "_" + $ProductKeyID + "_Result.xml";
$ResultJsonFilePath = $RootDir + "\Output\" + $TransactionID + "_" + $ProductKeyID + "_Result.json";
$ResultHtmlFilePath = $RootDir + "\Output\" + $TransactionID + "_" + $ProductKeyID + "_Result.html";

#[xml]$ResultXml = Get-TransformedXml -XmlDocument $HardwareHashDecode -XsltPath $XsltPath -OutputEncoding "utf-8" -XsltArguments $XsltArgs -XsltExtendedObjects $XsltExtObjs;

#$RulesObj = Initialize-Rule -Path ($RootDir + "\Config\rule.json");

$RulesObj = Initialize-Rule -DefaultRulePath ($RootDir + "\Config\rule.json") -UserRulePath ($RootDir + "\Config\user-rule.json");

$DataObj = Initialize-Data -Path $DecodeFilePath;

$RuleItemProductKeyID = New-Object -TypeName "QA.Model.ValidationRuleItem";

$RuleItemProductKeyID.FieldName = "ProductKeyID";
$RuleItemProductKeyID.FieldValue = $ProductKeyID;
$RuleItemProductKeyID.GroupName = "DEFAULT";
$RuleItemProductKeyID.RuleType = ([QA.Model.RuleType]::EqualTo);

$RuleItemProductKeyID;

Add-Rule -RuleItem $RuleItemProductKeyID;

$RuleItemProcessorModel = New-Object -TypeName "QA.Model.ValidationRuleItem";

$RuleItemProcessorModel.FieldName = "ProcessorModel";
$RuleItemProcessorModel.FieldValue = $ProcessorModel;
$RuleItemProcessorModel.GroupName = "DEFAULT";
$RuleItemProcessorModel.RuleType = ([QA.Model.RuleType]::EqualTo);

$RuleItemProcessorModel;

Add-Rule -RuleItem $RuleItemProcessorModel;

#$ResultJson = Get-Result | ConvertTo-Json;

if(($ShouldByPassDPKChecking -eq $true) -and ($ProductKeyID -eq "NO_KEY_CHECK"))
{
	Set-Data -Key "ProductKeyID" -Value "NO_KEY_CHECK";
}

[xml]$ResultXml = Get-Result;

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

$ResultHtml = New-XsltTransformation -XmlString $ResultXml.InnerXml -XsltPath $XsltHtmlPath -OutputEncoding "utf-8" -XsltArguments $XsltArgs -XsltExtendedObjects $XsltExtObjs;
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
$AutopilotCSVFilePath = ($RootDir + "\Output\" + $TransactionID + "_" + $ProductKeyID + "_Autopilot.csv");

Copy-Item -Path $TraceFilePath -Destination $TraceXmlOutputPath -Force;
Copy-Item -Path $DecodeFilePath -Destination $DecodeXmlOutputPath -Force;
Copy-Item -Path $ReportFilePath -Destination $ReportXmlOutputPath -Force;

#("Device Serial Number,Windows Product ID,Hardware Hash,Group Tag") | Out-File -FilePath $AutopilotCSVFilePath -Encoding utf8; #In reaction to the May 20, 2019 Autopilot CSV format change -- Rally, May 30, 2019
#($SerialNumber + "," + $ProductKeyID + "," + $ProductKeyInfo.Key.HardwareHash) | Out-File -FilePath $AutopilotCSVFilePath -Encoding utf8 -Append;

#$FilePathsForZip = @($TraceXmlOutputPath, $DecodeXmlOutputPath, $ReportXmlOutputPath, $ResultXmlFilePath, $ResultJsonFilePath, $ResultHtmlFilePath, $SMBIOSDumpPath, $MonitorDumpPath, ($LogPath + "\" + $TransactionID + ".log"));

#$ZippedFilePath = ($RootDir + "\Output\" + $TransactionID + "_" + $ProductKeyID + "_All.zip");
#New-Zip -FilesToZip $FilePathsForZip -ZippedFilePath $ZippedFilePath -VirtualPathInZip ($ProductKeyID +"\"+ $TransactionID);

#$ProductKeyInfo.Save($RootDir + "\Input\" + $TransactionID + "_Report.xml");

#$IE.Navigate2([System.String]::Format("file:///{0}?TransactionID={1}&ProductKeyID={2}", $ResultHtmlFilePath, $TransactionID, $ProductKeyID));
#$IE.Visible=$true;

#Start-Process -FilePath ("microsoft-edge:file://" + $ResultHtmlFilePath);

#Start-Process -FilePath ([System.String]::Format("file:///{0}?TransactionID={1}&ProductKeyID={2}", $ResultHtmlFilePath, $TransactionID, $ProductKeyID));

$FailedItemNodes = $ResultXml.SelectNodes("//Result[text() = 'Failed']");

$TotalResult = "Passed";

if($FailedItemNodes.Count -gt 0)
{
   $TotalResult = "Failed";
   $Host.UI.RawUI.ForegroundColor = "Red";
}
else
{
   $Host.UI.RawUI.ForegroundColor = "Green";
}

if($OutResult -ne $null)
{
   $OutResult.Value = $TotalResult;
}

if($TotalResult -eq "Passed")
{
	("Device Serial Number,Windows Product ID,Hardware Hash,Group Tag") | Out-File -FilePath $AutopilotCSVFilePath -Encoding utf8; #In reaction to the May 20, 2019 Autopilot CSV format change -- Rally, May 30, 2019
	
	if(($ProductKeyID -ne "NO_KEY_CHECK") -and ([System.String]::IsNullOrEmpty($ProductKeyID) -eq $false))
	{
		($SerialNumber + "," + $ProductKeyID + "," + $ProductKeyInfo.Key.HardwareHash) | Out-File -FilePath $AutopilotCSVFilePath -Encoding utf8 -Append;
	}
	else
	{
		($SerialNumber + ",," + $ProductKeyInfo.Key.HardwareHash) | Out-File -FilePath $AutopilotCSVFilePath -Encoding utf8 -Append;
	}
    
    $FilePathsForZip = @($TraceXmlOutputPath, $DecodeXmlOutputPath, $ReportXmlOutputPath, $ResultXmlFilePath, $ResultJsonFilePath, $ResultHtmlFilePath, $SMBIOSDumpPath, $MonitorDumpPath, ($LogPath + "\" + $TransactionID + ".log"), $AutopilotCSVFilePath);
}
else
{
	 $FilePathsForZip = @($TraceXmlOutputPath, $DecodeXmlOutputPath, $ReportXmlOutputPath, $ResultXmlFilePath, $ResultJsonFilePath, $ResultHtmlFilePath, $SMBIOSDumpPath, $MonitorDumpPath, ($LogPath + "\" + $TransactionID + ".log"));
}

$ZippedFilePath = ($RootDir + "\Output\" + $TransactionID + "_" + $ProductKeyID + "_All.zip");
New-Zip -FilesToZip $FilePathsForZip -ZippedFilePath $ZippedFilePath -VirtualPathInZip ($ProductKeyID +"\"+ $TransactionID);


#$ServiceUrl = "http://127.0.0.1:3000/engineering/";
#Reading out Factory.js settings, and set Service Url for result uploading (if configured as enabled)
try
{
	[xml]$FactoryJSAPISettings = Get-Content -Path ($RootDir + "\Config\api-config.xml") -Encoding UTF8;

	$FactoryJSAPISettings.settings;

	if(($FactoryJSAPISettings.settings.enabled -eq "true") -or ($FactoryJSAPISettings.settings.enabled -eq "1"))
	{
		$ServiceUrl = $FactoryJSAPISettings.settings.servicePoint;
	}
}
catch [System.Exception]
{
	$Message = $Error[0].Exception;
    $Message;
    $Message | Out-File -FilePath $LogPath -Append;
}

#Uploading result files to Factory.js, if service url is specified and available
$ServiceUrl;
if([System.String]::IsNullOrEmpty($ServiceUrl) -eq $false)
{
	#$BodyJsonString = '"uid": "",
	#	"transactionId": "{0}",
	#	"type": "",
	#	"value": "",
	#	"time": "{1}",
	#	"systemInfo": {2},
	#	"smbInfo": {3},
	#	"monitorInfo": {4},
	#	"oa3Report": {5},
	#	"oa3ReportTrace": {6},
	#	"oa3HwDecode": {7},
	#	"validationResult": {8}';

	#$CurrDate = Get-Date;

	##$BodyJsonString = [System.String]::Format($BodyJsonString, $TransactionID, $CurrDate, $SystemInfoJson, $SMBInfoJson, $MonitorInfoJson, $ProductKeyInfoJson, $ReportTraceJson, $HardwareHashDecodeJson, $ResultJson);

	##$BodyJsonString = [System.String]::Format($BodyJsonString, $TransactionID, $CurrDate, $SystemInfoJson, "", "", "", "", "", "");

	#$BodyJsonString = [System.String]::Format($BodyJsonString, $TransactionID, $CurrDate, $SystemInfoJson, "", $MonitorInfoJson, "", "", "", $ResultJson);

	#$BodyJsonString = "{" + $BodyJsonString + "}";

	##$Body = ConvertFrom-Json -InputObject $BodyJsonString;

	#Invoke-RestMethod -Method Post -Body $BodyJsonString -Uri $ServiceUrl;

	$FactoryJSAPIServicePoint = $ServiceUrl;

	if($FactoryJSAPIServicePoint.EndsWith("/") -eq $false)
	{
	   $FactoryJSAPIServicePoint += "/";
	}

	$FactoryJSAPIServicePoint;

	[System.String]$HttpResult = Invoke-RestMethod -Method Get -Uri $FactoryJSAPIServicePoint;

	$HttpResult;

	if($HttpResult.Contains("Welcome to Factory.js!") -eq $true)
	{
		[System.Net.WebClient]$webClient = New-Object -TypeName System.Net.WebClient;

		$FactoryJSAPIHQAOutputUrl = ($FactoryJSAPIServicePoint + "hqa/output/");

		$FactoryJSAPIHQAOutputUrl;

		$webClient.UploadFile($FactoryJSAPIHQAOutputUrl, $ZippedFilePath);
	}
}

Write-Host -Object ("Validation Result: {0}." -f $TotalResult);

if($ByPassUI -eq $false)
{
    $AppDataResultJson = ("AppData=" + $ResultJson);
	$AppDataResultJson | Out-File -Encoding utf8 -FilePath ($RootDir + "\Module\UI\Views\data.json") -Force;

	$AppSettings = New-Object -TypeName "System.Collections.Generic.Dictionary``2[System.String,System.Object]";
	$AppSettings.Add("Mode", "Online");
	$AppSettings.Add("TransactionID", $TransactionID);
	$AppSettings.Add("ProductKeyID", $ProductKeyID);
	$AppSettings.Add("RootDir", $RootDir);
	$AppSettings.Add("ZipFilePath", $ZippedFilePath);
	$AppSettings.Add("LogFilePath", ($LogPath + "\" + $TransactionID + ".log"));
	$AppSettings.Add("SMBIOSDumpPath", $SMBIOSDumpPath);
	$AppSettings.Add("MonitorDumpPath", $MonitorDumpPath);
	$AppSettings.Add("TraceXmlOutputPath", $TraceXmlOutputPath);

	$AppSettingsJson = ConvertTo-Json -InputObject $AppSettings -Compress;
	$AppSettingsJson = "Settings={`"Data`":" + $AppSettingsJson + "}";
	$AppSettingsJson | Out-File -Encoding utf8 -FilePath ($RootDir + "\Module\UI\Views\config.json") -Force;

	$Message = [System.String]::Format("Launching Report..., {0}", [System.DateTime]::Now);
	$Message;
	$Message | Out-File -FilePath ($LogPath + "\validation-log.log") -Append;    

	#$Choice = Read-Host -Prompt ("Validation Result: {0}.`nView the validation summary report for more details? (Y: `"Yes`"; N: `"No`" (Default).)" -f $TotalResult);

	#if(($Choice -eq "Y") -or ($Choice -eq "Yes"))
	#{
	#   #Show-Report -Uri ([System.String]::Format("file:///{0}?TransactionID={1}&ProductKeyID={2}", $ResultHtmlFilePath, $TransactionID, $ProductKeyID));

	#   #Start-Process -FilePath ($RootDir + "\Module\UI\WebViewPlus.exe") -ArgumentList @($RootDir + "\Module\UI\Default.html") -Wait -NoNewWindow -RedirectStandardOutput ($LogPath + "\" + $TransactionID + ".log");

 #      Start-Process -FilePath ($RootDir + "\Module\UI\WebViewPlus.exe");
	#}
	#else
	#{
	#   exit;
	#}

	$PromptTitle = "Show Validation Report"
	$PromptMessage = "Do you want to view report for more details?"
	$OptionYes = New-Object System.Management.Automation.Host.ChoiceDescription "&Yes", "Show the report and view it."
	$OptionNo = New-Object System.Management.Automation.Host.ChoiceDescription "&No", "Skip the report and exit."
	$PromptOptions = [System.Management.Automation.Host.ChoiceDescription[]]($OptionNo, $OptionYes)

	$Choice = $Host.UI.PromptForChoice($PromptTitle, $PromptMessage, $PromptOptions, 0) 

	switch ($Choice)
	{
	    0 {
			#$Host.SetShouldExit(1);
			#exit;
		}
		1 {
			Start-Process -FilePath ($RootDir + "\Module\UI\WebViewPlus.exe") -Wait -NoNewWindow;
		}
	}
}
#else
#{
#   #exit;
#   #$Host.SetShouldExit(1);
#}

if($StayInHost -eq $false)
{
   $Host.SetShouldExit(1);
}

