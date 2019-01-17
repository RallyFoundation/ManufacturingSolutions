mkdir C:\VAMT-API\Log\

$ImagePath = "C:\VAMT-API\Log\";
$ShareName = "VAMT-Data";
$ShareType = 0;
$WMIObj = [wmiClass] 'Win32_share';
$WMIObj.Create($ImagePath, $ShareName, $ShareType);