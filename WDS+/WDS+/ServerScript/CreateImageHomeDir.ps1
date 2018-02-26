mkdir D:\WDS-Images\Install\
mkdir D:\WDS-Images\Boot\
mkdir D:\WDS-Images\FFU\
mkdir D:\WDS-Images\Test\
mkdir D:\WDS-Images\Log\


$ImagePath = "D:\WDS-Images";
$ShareName = "WDS-Images";
$ShareType = 0;
$WMIObj = [wmiClass] 'Win32_share';
$WMIObj.Create($ImagePath, $ShareName, $ShareType);