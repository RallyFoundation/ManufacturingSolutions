﻿$RootDir = Split-Path -parent $MyInvocation.MyCommand.Definition;

if($RootDir.EndsWith("\") -eq $true)
{
   $RootDir = $RootDir.Substring(0, ($RootDir.Length -1));
}

[xml]$ConfigXml = Get-Content -Path ($RootDir + "\config.xml") -Encoding UTF8;

$ConfigXml.InnerXml;

$ImageServerAddress = $ConfigXml.configurationItems.imageServerAddress; #"minint-et2evvt";
$ImageServerUserName= $ConfigXml.configurationItems.imageServerUserName; #"Administrator";
$ImageServerPassword = $ConfigXml.configurationItems.imageServerPassword; #"W@lcome!";
$NICName = $ConfigXml.configurationItems.nicName;
[int]$ClientIdentifierType = $ConfigXml.configurationItems.clientIdentifierType;
[int]$ImageIdentifierType = $ConfigXml.configurationItems.imageIdentifierType;
 

[System.String]$WDSApiServicePoint = $ConfigXml.configurationItems.wdsApiServicePoint; #"http://minint-et2evvt:8089";

if($WDSApiServicePoint.EndsWith("/") -eq $false)
{
   $WDSApiServicePoint += "/";
}

$ImageServerAddress;
$ImageServerUserName;
$WDSApiServicePoint;

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

$Body = ConvertFrom-Json -InputObject "{`"Key`":`"`", `"Value`":`"`"}";
$Body.Key = $ClientID;

[System.String]$Url = "wds/lookup/";
[System.String]$UrlProgress = "wds/terminal/status/";

$Body.Value = "GettingSKUFromBIOS";
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

$ImageID;

$Body.Value = "GettingImageUrl";
$BodyJson = ConvertTo-Json -InputObject $Body;
Invoke-RestMethod -Method Post -Uri ($WDSApiServicePoint + $UrlProgress) -Body $BodyJson -ContentType "application/json";

$Uri = $WDSApiServicePoint + $Url + $ImageID;

$Uri;

$ImageUrl = Invoke-RestMethod -Method Get -Uri $Uri;

$ImageUrl;

$Body.Value = "DownloadingImage";
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
Start-Process -FilePath "wdsmcast.exe" -ArgumentList @("/progress", "/verbose", "/trace:wds_trace.etl", "/Apply-Image", ("/Server:" + $ImageServerAddress), ("/Namespace:" + $WDSImageNameSpace), ("/Username:" + $ImageServerAddress + "\" + $ImageServerUserName), ("/Password:" + $ImageServerPassword), ("/SourceFile:" + $WDSImageSource), "/Index:1", "/DestinationPath:W:\") -Wait -NoNewWindow;

#DISM /Apply-Image /ImageFile:R:\install.wim /ApplyDir:W:\ /Index:1  /ScratchDir:R:\TEMP

#mkdir R:\TEMP;

#Expand-WindowsImage -ImagePath "R:\install.wim" -ApplyPath "W:\" -Index 1 -ScratchDirectory "R:\TEMP";

$Body.Value = "ImageApplied";
$BodyJson = ConvertTo-Json -InputObject $Body;
Invoke-RestMethod -Method Post -Uri ($WDSApiServicePoint + $UrlProgress) -Body $BodyJson -ContentType "application/json";