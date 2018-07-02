@echo off

set transid=%1
set inputfile=%2

rem if /i "%transid%"=="" for /f %a in ('PowerShell -Command "& {Write-Host([System.GUID]::NewGUID())}"') do (set transid=%a)

echo %transid%
echo %inputfile%

set logpath=.\Log\log_offline_%transid%.log

echo "Processing..."
call PowerShell -ExecutionPolicy ByPass -File .\Script\validate-online.ps1 -TransactionID %transid% -ReportFilePath %inputfile% > %logpath%
echo "Done!"