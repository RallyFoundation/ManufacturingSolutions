wpeinit
:rem==diskpart /s %WINDIR%\System32\diskpartcmd.txt
:rem==%WINDIR%\System32\Wdsmcast\wdsmcast.exe /progress /verbose /trace:wds_trace.etl /Apply-Image /Server:192.168.0.215 /Namespace:WDS:Group-Windows8/Win8-Windows.wim/1 /Username:WIN-Server-02\Administrator /Password:P@ssword! /SourceFile:Win8-Windows.wim /Index:1 /DestinationPath:W:\
:rem==%WINDIR%\System32\Wdsmcast\wdsmcast.exe /progress /verbose /trace:wds_trace.etl /Transfer-File /Server:192.168.0.215 /Namespace:WDS:Group-Windows8/Win8-Windows.wim/1 /Username:WIN-Server-02\Administrator /Password:P@ssword! /SourceFile:Win8-Windows.wim /DestinationFile:R:\install.wim
:rem==DISM /Apply-Image /ImageFile:R:\install.wim /ApplyDir:W:\ /Index:1  /ScratchDir:R:\TEMP

call powercfg /list
call powercfg /s 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c

PowerShell -ExecutionPolicy ByPass -File .\run.ps1

:rem==BCDBoot W:\Windows /s S:\ /f ALL
:rem==wpeutil reboot