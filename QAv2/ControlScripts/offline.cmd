@echo off
setlocal

if /i not "%2"=="APP_ROOT_NULL" goto withapproot 
if /i "%2"=="APP_ROOT_NULL" goto withoutapproot

:withapproot
rem echo "with app root"
set rootdir=%2
set transid=%1
rem set inputfile=%3
set inputfile=%~s3
rem set inputfile=%~3
set logpath= %rootdir%\Log\%transid%.log
goto route

:withoutapproot
rem echo "without app root"
set rootdir=%~f0
set rootdir=%rootdir:\Script\offline.cmd=%
set transid=%1
rem set inputfile=%3
set inputfile=%~s3
rem set inputfile=%~3
set logpath= %rootdir%\Log\%transid%.log
goto route

rem if /i "%transid%"=="" for /f %a in ('PowerShell -Command "& {Write-Host([System.GUID]::NewGUID())}"') do (set transid=%a)
rem if /i "%inputfile%"=="" goto loop1

:route
echo %rootdir%
echo %transid%
echo %inputfile%
echo %logpath%

rem subst Y: "%rootdir%"
rem set logpath = Y:\Log\%transid%.log
rem echo %logpath%

if /i not "%inputfile%"=="" goto withxml
if /i "%inputfile%"=="" goto withoutxml

:withxml
echo "Processing..."
call PowerShell -ExecutionPolicy ByPass -File "%rootdir%\Script\validate-offline.ps1" -TransactionID %transid% -ReportFilePath "%inputfile%" > %logpath%
echo "Done!"
goto cleanup

:withoutxml
echo "Processing..."
call PowerShell -ExecutionPolicy ByPass -File "%rootdir%\Script\validate-offline.ps1" -TransactionID %transid% > %logpath%
echo "Done!"
goto cleanup

:cleanup
endlocal & exit /b %EXITCODE%

rem :loop1
rem set /p inputfile=Please specify the full path to the oa3.0 /report result file:
rem if /i "%inputfile%"=="" goto loop1