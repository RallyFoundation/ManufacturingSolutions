set MsiName=%1
set lang=%2
set langcode=%3
set TargetPath=%4
set ToolPath=%5

copy "%TargetPath%\en-US\%MsiName%.msi" "%TargetPath%\%MsiName%.msi"
"%ToolPath%\MsiTran.exe" -g "%TargetPath%\%MsiName%.msi" "%TargetPath%\%lang%\%MsiName%.msi" "%TargetPath%\%lang%.mst"
cscript "%ToolPath%\wisubstg.vbs" "%TargetPath%\%MsiName%.msi" "%TargetPath%\%lang%.mst" %langcode%