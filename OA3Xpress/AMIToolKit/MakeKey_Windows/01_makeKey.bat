@echo MakeKey...
set tooldir=%cd%
cd %tooldir%
oa3tool.exe /Assemble /Configfile=oa3tool.cfg
copy /y OA3.bin ..\
copy /y OA3.bin ..\Flash_afuwin\
copy /y OA3.xml ..\OA3.Assemble.xml
copy /y OA3.xml ..\Flash_afuwin\OA3.Assemble.xml

rem pause