@rem echo off
rem cd   /d %~dp0

@echo ==01== change to current dir
set toolDir=%cd%
echo %toolDir%
cd /d  %toolDir%


@echo ==02== Copy .xml and .cfg file to make key dir(by overwrite them)
copy /y .\oa3tool.cfg .\MakeKey_Windows\
rem copy /y .\oa3tool.xml .\MakeKey_Windows\
rem copy /y \oa3tool.cfg .\MakeKey_Windows\
rem copy /y \oa3tool.xml .\MakeKey_Windows\

@echo ==03== make key  and copy it to the main dir
cd .\MakeKey_windows\
call 01_makeKey.bat
cd ..


@echo ==04== Flash key
cd .\Flash_afuwin\
call 01_Flash.bat
cd ..

@echo ==05== Read key
cd .\MakeKey_Windows\
02_ReadKey.bat

rem cd 
rem @echo ===============Press anykey to reset the computer ================
rem @echo ===============Honghui.Li@emdoor.com   2014-03-10 ================
rem @pause

rem @c:\windows\system32\shutdown.exe -r -t 0
