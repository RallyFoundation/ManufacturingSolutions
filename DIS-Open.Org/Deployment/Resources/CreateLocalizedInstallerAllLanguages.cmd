set MsiName=%1
set TargetPath=%2
set ToolPath=%3

SET TargetPath=###%TargetPath%###
SET TargetPath=%TargetPath:"###=%
SET TargetPath=%TargetPath:###"=%
SET TargetPath=%TargetPath:###=%

SET ToolPath=###%ToolPath%###
SET ToolPath=%ToolPath:"###=%
SET ToolPath=%ToolPath:###"=%
SET ToolPath=%ToolPath:###=%

copy "%TargetPath%en-US\%MsiName%.msi" "%TargetPath%%MsiName%.msi" /Y

"%ToolPath%\MsiTran.exe" -g "%TargetPath%%MsiName%.msi" "%TargetPath%es-es\%MsiName%.msi" "%TargetPath%es-es.mst"
cscript "%ToolPath%\wisubstg.vbs" "%TargetPath%%MsiName%.msi" "%TargetPath%es-es.mst" 3082

"%ToolPath%\MsiTran.exe" -g "%TargetPath%%MsiName%.msi" "%TargetPath%zh-cn\%MsiName%.msi" "%TargetPath%zh-cn.mst"
cscript "%ToolPath%\wisubstg.vbs" "%TargetPath%%MsiName%.msi" "%TargetPath%zh-cn.mst" 2052

"%ToolPath%\MsiTran.exe" -g "%TargetPath%%MsiName%.msi" "%TargetPath%pt-br\%MsiName%.msi" "%TargetPath%pt-br.mst"
cscript "%ToolPath%\wisubstg.vbs" "%TargetPath%%MsiName%.msi" "%TargetPath%pt-br.mst" 1046

"%ToolPath%\MsiTran.exe" -g "%TargetPath%%MsiName%.msi" "%TargetPath%ja-jp\%MsiName%.msi" "%TargetPath%ja-jp.mst"
cscript "%ToolPath%\wisubstg.vbs" "%TargetPath%%MsiName%.msi" "%TargetPath%ja-jp.mst" 1041

"%ToolPath%\MsiTran.exe" -g "%TargetPath%%MsiName%.msi" "%TargetPath%zh-tw\%MsiName%.msi" "%TargetPath%zh-tw.mst"
cscript "%ToolPath%\wisubstg.vbs" "%TargetPath%%MsiName%.msi" "%TargetPath%zh-tw.mst" 1028

cscript "%ToolPath%\WiLangId.vbs" "%TargetPath%%MsiName%.msi" Package 1033,2052,1046,1041,1028,3082


Copy "%ToolPath%\setup.exe" "%TargetPath%Setup.exe" /Y
"%ToolPath%\mt.exe" -manifest "%ToolPath%\Vista.manifest" -outputresource:"%TargetPath%Setup.exe";#1
"%ToolPath%\setupbld.exe" -out "%TargetPath%%MsiName%.exe" -msu "%TargetPath%%MsiName%.msi" -setup "%TargetPath%Setup.exe"

del "%TargetPath%Setup.exe"
del "%TargetPath%*.mst"
:rmdir "%TargetPath%es-es" /s /q
:rmdir "%TargetPath%en-US" /s /q
:rmdir "%TargetPath%zh-CN" /s /q
:rmdir "%TargetPath%zh-TW" /s /q
:rmdir "%TargetPath%ja-JP" /s /q
:rmdir "%TargetPath%pt-BR" /s /q
