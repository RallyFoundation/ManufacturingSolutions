@echo off
setlocal

if /i not "%2"=="APP_ROOT_NULL" goto withapproot 
if /i "%2"=="APP_ROOT_NULL" goto withoutapproot

:withapproot
rem echo "with app root"
set rootdir=%2
set transid=%1
set logpath= %rootdir%\Log\%transid%.log
goto startprocessing

:withoutapproot
rem echo "without app root"
set rootdir=%~f0
set rootdir=%rootdir:\Script\online.cmd=%
set transid=%1
set logpath= %rootdir%\Log\%transid%.log
goto startprocessing

rem if /i "%transid%"=="" for /f %a in ('PowerShell -Command "& {Write-Host([System.GUID]::NewGUID())}"') do (set transid=%a)

:startprocessing
echo %rootdir%
echo %transid%
echo %logpath%

echo "Processing..."
call PowerShell -ExecutionPolicy ByPass -File "%rootdir%\Script\validate-online.ps1" -TransactionID %transid% > %logpath%
echo "Done!"
endlocal & exit /b %EXITCODE%