@echo off
setlocal

rem Get the work directory of the current .cmd
set rootpath=%~f0
set rootpath=%rootpath:\InvokeOffline.cmd=%

rem Convert input file path to short path name, and remove any surrounding quotation marks (""): 
set inputpath=%~s1

if "%inputpath%"=="" goto askforpath else goto startexe

:startexe
echo %rootpath%
echo %inputpath%
call "%rootpath%\Offline.exe" %inputpath%
goto finish

:askforpath
echo "The full path to the oa3.0 /report result file is required!"
goto finish

:finish
endlocal & exit /b %EXITCODE%