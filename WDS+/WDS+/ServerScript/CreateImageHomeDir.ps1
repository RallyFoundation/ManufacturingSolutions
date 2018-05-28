mkdir C:\WDS-Images\Install\
mkdir C:\WDS-Images\Boot\
mkdir C:\WDS-Images\FFU\
mkdir C:\WDS-Images\Test\
mkdir C:\WDS-Images\Log\


$ImagePath = "C:\WDS-Images";
$ShareName = "WDS-Images";
$ShareType = 0;
$WMIObj = [wmiClass] 'Win32_share';
$WMIObj.Create($ImagePath, $ShareName, $ShareType);