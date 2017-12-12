$RootDir = Split-Path -parent $MyInvocation.MyCommand.Definition;

if($RootDir.EndsWith("\") -eq $true)
{
   $RootDir = $RootDir.Substring(0, ($RootDir.Length -1));
}

$ErrorActionPreference = "Stop";

[System.DateTime]$StartTime = [System.DateTime]::Now;

[xml]$ConfigXml = Get-Content -Path ($RootDir + "\config.xml") -Encoding UTF8;

$ConfigXml.InnerXml;

$ImageServerAddress = $ConfigXml.configurationItems.imageServerAddress; #"minint-et2evvt";
$ImageServerUserName= $ConfigXml.configurationItems.imageServerUserName; #"Administrator";
$ImageServerPassword = $ConfigXml.configurationItems.imageServerPassword; #"W@lcome!";
$NICName = $ConfigXml.configurationItems.nicName;
[int]$ClientIdentifierType = $ConfigXml.configurationItems.clientIdentifierType;
[int]$ImageIdentifierType = $ConfigXml.configurationItems.imageIdentifierType;

$ImageDestination = $ConfigXml.configurationItems.imageDestination;

[System.String]$WDSApiServicePoint = $ConfigXml.configurationItems.wdsApiServicePoint; #"http://minint-et2evvt:8089";

if($WDSApiServicePoint.EndsWith("/") -eq $false)
{
   $WDSApiServicePoint += "/";
}

$ImageServerAddress;
$ImageServerUserName;
$WDSApiServicePoint;

$TransactionID = [System.Guid]::NewGuid().ToString();

$ClientID = "";
$ImageID = "";

if($ClientIdentifierType -eq 0)
{
	$ComputerBIOS = Get-CimInstance CIM_BIOSElement;
	$SerialNumber = $ComputerBIOS.SerialNumber;
	$ClientID = $SerialNumber;
}

if($ClientIdentifierType -eq 1)
{
	$MacObject = Get-WmiObject Win32_NetworkAdapter | Where-Object { $_.MacAddress -and $_.Name -eq $NICName} | Select-Object Name, MacAddress; 
	$MacAddress = $MacObject.MacAddress;
	$ClientID = $MacAddress;
}

if([System.String]::IsNullOrEmpty($ClientID))
{
	$ClientID = $ConfigXml.configurationItems.clientIdentifierValue;
}

$ClientID;

$Body = ConvertFrom-Json -InputObject "{`"Key`":`"`", `"Value`":`"`",  `"Data`":`"`", `"TransID`":`"`", `"Time`":`"`"}";
$Body.TransID = $TransactionID;
$Body.Key = $ClientID;

[System.String]$Url = "wds/lookup/";
[System.String]$UrlProgress = "wds/terminal/status/";
[System.String]$UrlLogFile = ("wds/logfile/{0}" -f $TransactionID);

$Body.Value = "GettingSKUFromBIOS";
$Body.Time = [System.DateTime]::Now;
$BodyJson = ConvertTo-Json -InputObject $Body;
Invoke-RestMethod -Method Post -Uri ($WDSApiServicePoint + $UrlProgress) -Body $BodyJson -ContentType "application/json";

if($ImageIdentifierType -eq 0)
{
	$SystemInfo = Get-CimInstance -ClassName Win32_ComputerSystem;
	$SKU = $SystemInfo.SystemSKUNumber;
	$ImageID = $SKU;
}

if($ImageIdentifierType -eq 1)
{
   $SystemInfo = Get-CimInstance -ClassName Win32_ComputerSystem;
   $Model = $SystemInfo.Model;
   $ImageID = $Model;
}

if($ImageIdentifierType -eq 2)
{
   $SystemInfo = Get-CimInstance -ClassName Win32_ComputerSystem;
   $ImageID = $SystemInfo.SystemFamily;
}

if($ImageIdentifierType -eq 3)
{
   $SystemInfo = Get-CimInstance -ClassName Win32_ComputerSystem;
   $ImageID = $SystemInfo.Manufacturer;
}

if([System.String]::IsNullOrEmpty($ImageID))
{
	$ImageID = $ConfigXml.configurationItems.imageIdentifierValue;
}

$ImageID;

if([System.String]::IsNullOrEmpty($ImageID))
{
    $Host.UI.RawUI.BackgroundColor = "Red";
    $Host.UI.RawUI.ForegroundColor = "Yellow";
    Write-Host -Object "The specified image identifier field in SMBIOS is empty!";
	Read-Host -Prompt "The specified image identifier field in SMBIOS is empty! `nPress any key to exit...";
    exit;
}

$Body.Value = "GettingImageConfiguration";
$Body.Time = [System.DateTime]::Now;
$BodyJson = ConvertTo-Json -InputObject $Body;
Invoke-RestMethod -Method Post -Uri ($WDSApiServicePoint + $UrlProgress) -Body $BodyJson -ContentType "application/json";

$Uri = $WDSApiServicePoint + $Url + $ImageID;
$Uri;

#$ImageUrl = Invoke-RestMethod -Method Get -Uri $Uri;
#$ImageUrl;

$ImageConfig = Invoke-RestMethod -Method Get -Uri $Uri;
$ImageConfig;

if([System.String]::IsNullOrEmpty($ImageConfig.ImageServerAddress) -eq $false)
{
	 $ImageServerAddress = $ImageConfig.ImageServerAddress;
}

if([System.String]::IsNullOrEmpty($ImageConfig.ImageServerUsername) -eq $false)
{
	 $ImageServerUserName = $ImageConfig.ImageServerUsername;
}

if([System.String]::IsNullOrEmpty($ImageConfig.ImageServerPassword) -eq $false)
{
	 $ImageServerPassword = $ImageConfig.ImageServerPassword;
}

if([System.String]::IsNullOrEmpty($ImageConfig.ImageDestination) -eq $false)
{
	 $ImageDestination = $ImageConfig.ImageDestination;
}

if([System.String]::IsNullOrEmpty($ImageConfig.DiskIdentifier) -eq $false)
{
	 $DiskIdentifier = $ImageConfig.DiskIdentifier;
}

$ImageUrl = $ImageConfig.ImageSource;

$Body.Value = "CreatingDiskPartition";
$Body.Time = [System.DateTime]::Now;
$BodyJson = ConvertTo-Json -InputObject $Body;
Invoke-RestMethod -Method Post -Uri ($WDSApiServicePoint + $UrlProgress) -Body $BodyJson -ContentType "application/json";

if($DiskIdentifier -ne $ConfigXml.configurationItems.diskIdentifierValue)
{
   $DiskPartCmdTemplate = Get-Content -Path ("diskpartcmd-template.txt") -Encoding Ascii;

   if([System.IO.File]::Exists("diskpartcmd.txt"))
   {
      [System.IO.File]::Delete("diskpartcmd.txt");
   }

   foreach($line in  $DiskPartCmdTemplate)
   {
      if($line.StartsWith("select disk {0}"))
      {
         $line = ($line -f $DiskIdentifier);      
      }

      $line | Out-File -FilePath ("diskpartcmd.txt") -Encoding ascii -Append -Force;
   }
}

Start-Process -FilePath "diskpart" -ArgumentList  @("/s diskpartcmd.txt") -Wait -NoNewWindow;

$Body.Data = $ImageUrl;
$Body.Value = "ApplyingImage";
$Body.Time = [System.DateTime]::Now;
$BodyJson = ConvertTo-Json -InputObject $Body;
Invoke-RestMethod -Method Post -Uri ($WDSApiServicePoint + $UrlProgress) -Body $BodyJson -ContentType "application/json";

$WDSImageNameSpace = $ImageUrl;

$WDSImageSource = $ImageUrl.Substring(($ImageUrl.IndexOf("/") + 1));
$WDSImageSource = $WDSImageSource.Substring(0, $WDSImageSource.LastIndexOf("/"));

$WDSImageSource; 


#%WINDIR%\System32\Wdsmcast\wdsmcast.exe /progress /verbose /trace:wds_trace.etl /Transfer-File /Server:192.168.0.215 /Namespace:WDS:Group-Windows8/Win8-Windows.wim/1 /Username:WIN-Server-02\Administrator /Password:P@ssword! /SourceFile:Win8-Windows.wim /DestinationFile:R:\install.wim
#& (wdsmcast.exe) @("/progress", "/verbose", "/trace:wds_trace.etl", "/Transfer-File", ("/Server:" + $ImageServerAddress), ("/Namespace:" + $WDSImageNameSpace),  ("/Username:" + $ImageServerAddress + "\" + $ImageServerUserName), ("/Password:" + $ImageServerPassword),  ("/SourceFile:" + $WDSImageSource), "/DestinationFile:R:\install.wim");
#Start-Process -FilePath "wdsmcast.exe" -ArgumentList @("/progress", "/verbose", "/trace:wds_trace.etl", "/Transfer-File", ("/Server:" + $ImageServerAddress), ("/Namespace:" + $WDSImageNameSpace), ("/Username:" + $ImageServerAddress + "\" + $ImageServerUserName), ("/Password:" + $ImageServerPassword), ("/SourceFile:" + $WDSImageSource), "/DestinationFile:R:\install.wim") -Wait -NoNewWindow;

#%WINDIR%\System32\Wdsmcast\wdsmcast.exe /progress /verbose /trace:wds_trace.etl /Apply-Image /Server:192.168.0.215 /Namespace:WDS:Group-Windows8/Win8-Windows.wim/1 /Username:WIN-Server-02\Administrator /Password:P@ssword! /SourceFile:Win8-Windows.wim /Index:1 /DestinationPath:W:\
#Start-Process -FilePath "wdsmcast.exe" -ArgumentList @("/progress", "/verbose", "/trace:wds_trace.etl", "/Apply-Image", ("/Server:" + $ImageServerAddress), ("/Namespace:" + $WDSImageNameSpace), ("/Username:" + $ImageServerAddress + "\" + $ImageServerUserName), ("/Password:" + $ImageServerPassword), ("/SourceFile:" + $WDSImageSource), "/Index:1", "/DestinationPath:W:\") -Wait -NoNewWindow;

if([System.String]::IsNullOrEmpty($ImageDestination) -eq $true)
{
   $ImageDestination = "W:\";
}

Start-Process -FilePath "wdsmcast.exe" -ArgumentList @("/progress", "/verbose", "/trace:wds_trace.etl", "/Apply-Image", ("/Server:" + $ImageServerAddress), ("/Namespace:" + $WDSImageNameSpace), ("/Username:" + $ImageServerAddress + "\" + $ImageServerUserName), ("/Password:" + $ImageServerPassword), ("/SourceFile:" + $WDSImageSource), "/Index:1", "/DestinationPath:" + $ImageDestination) -Wait -NoNewWindow;

#DISM /Apply-Image /ImageFile:R:\install.wim /ApplyDir:W:\ /Index:1  /ScratchDir:R:\TEMP

#mkdir R:\TEMP;

#Expand-WindowsImage -ImagePath "R:\install.wim" -ApplyPath "W:\" -Index 1 -ScratchDirectory "R:\TEMP";

$Body.Value = "ImageApplied";
$Body.Time = [System.DateTime]::Now;
$BodyJson = ConvertTo-Json -InputObject $Body;
Invoke-RestMethod -Method Post -Uri ($WDSApiServicePoint + $UrlProgress) -Body $BodyJson -ContentType "application/json";

#Copy-Item -Path X:\Windows\Logs\DISM\dism.log -Destination ("D:\dismlog_{0}_{1}.log" -f $ImageID, $TransactionID) -Force;

[System.Net.WebClient]$WebClient = [System.Net.WebClient]::new();
$WebClient.UploadFileAsync(($WDSApiServicePoint + $UrlLogfile), "X:\Windows\Logs\DISM\dism.log");


$Body.Value = "ConfiguringBootOptions";
$Body.Time = [System.DateTime]::Now;
$BodyJson = ConvertTo-Json -InputObject $Body;
Invoke-RestMethod -Method Post -Uri ($WDSApiServicePoint + $UrlProgress) -Body $BodyJson -ContentType "application/json";

Start-Process -FilePath "BCDBoot" -ArgumentList  @(($ImageDestination + "Windows"), "/s S:\", "/f ALL") -Wait -NoNewWindow;

[System.DateTime]$EndTime = [System.DateTime]::Now;
[System.TimeSpan]$TimeSpan = $EndTime.Subtract($StartTime);
$Message = ("Total time spent: {0} seconds ({1} minutes)." -f $TimeSpan.TotalSeconds, $TimeSpan.TotalMinutes);
$Host.UI.RawUI.BackgroundColor = "Yellow";
$Host.UI.RawUI.ForegroundColor = "Green";
Write-Host -Object $Message;