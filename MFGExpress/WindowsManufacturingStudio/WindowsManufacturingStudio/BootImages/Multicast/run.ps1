$RootDir = Split-Path -parent $MyInvocation.MyCommand.Definition;

if($RootDir.EndsWith("\") -eq $true)
{
   $RootDir = $RootDir.Substring(0, ($RootDir.Length -1));
}

[xml]$ConfigXml = Get-Content -Path ($RootDir + "\config.xml") -Encoding UTF8;

$ConfigXml.InnerXml;

$ImageServerAddress = $ConfigXml.configurationItems.imageServerAddress; #"minint-et2evvt";
$ImageServerUserName= $ConfigXml.configurationItems.imageServerUserName; #"Administrator";
$ImageServerPassword = $ConfigXml.configurationItems.imageServerPassword; #"W@lcome!";

[System.String]$WDSApiServicePoint = $ConfigXml.configurationItems.wdsApiServicePoint; #"http://minint-et2evvt:8089";

if($WDSApiServicePoint.EndsWith("/") -eq $false)
{
   $WDSApiServicePoint += "/";
}

$ImageServerAddress;
$ImageServerUserName;
$WDSApiServicePoint;

$ClientID = "";

[System.String]$Url = "wds/lookup/";
[System.String]$UrlProgress = "wds/terminal/status/";

Invoke-RestMethod -Method Post -Uri ($WDSApiServicePoint + $UrlProgress) -Body ([System.String]::Format("{`"Key`":`"{0}`", `"Value`":`"{1}`"}", $ClientID, "GettingSKUFromBIOS")) -ContentType "application/json";

$SystemInfo = Get-CimInstance -ClassName Win32_ComputerSystem;
$SKU = $SystemInfo.SystemSKUNumber;

$SKU;

Invoke-RestMethod -Method Post -Uri ($WDSApiServicePoint + $UrlProgress) -Body ([System.String]::Format("{`"Key`":`"{0}`", `"Value`":`"{1}`"}", $ClientID, "GettingImageUrl")) -ContentType "application/json";

$Uri = $WDSApiServicePoint + $Url + $SKU;

$Uri;

$ImageUrl = Invoke-RestMethod -Method Get -Uri $Uri;

$ImageUrl;

Invoke-RestMethod -Method Post -Uri ($WDSApiServicePoint + $UrlProgress) -Body ([System.String]::Format("{`"Key`":`"{0}`", `"Value`":`"{1}`"}", $ClientID, "DownloadingImage")) -ContentType "application/json";

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

Invoke-RestMethod -Method Post -Uri ($WDSApiServicePoint + $UrlProgress) -Body ([System.String]::Format("{`"Key`":`"{0}`", `"Value`":`"{1}`"}", $ClientID, "ImageApplied")) -ContentType "application/json";