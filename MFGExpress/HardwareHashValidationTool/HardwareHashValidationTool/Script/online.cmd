@echo off

set transid=%1

rem if /i "%transid%"=="" for /f %a in ('PowerShell -Command "& {Write-Host([System.GUID]::NewGUID())}"') do (set transid=%a)

echo %transid%

set logpath=.\Log\log_online_%transid%.log

echo "Processing..."
call PowerShell -ExecutionPolicy ByPass -File .\Script\validate-online.ps1 -TransactionID %transid% > %logpath%
echo "Done!"