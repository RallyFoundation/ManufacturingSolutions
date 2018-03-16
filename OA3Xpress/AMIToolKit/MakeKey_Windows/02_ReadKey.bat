set tooldir=%cd%
cd %tooldir%
oa3tool.exe /Report /Configfile=oa3tool.cfg /LogTrace=OA3.Trace.xml
oa3tool.exe /DecodeHwHash=OA3.xml /LogTrace=OA3.HWDecode.xml
rem OA3.xml
copy /y OA3.xml ..\OA3.Report.xml
copy /y OA3.xml ..\Flash_afuwin\OA3.Report.xml
copy /y OA3.Trace.xml ..\OA3.Trace.xml
copy /y OA3.Trace.xml ..\Flash_afuwin\OA3.Trace.xml
copy /y OA3.HWDecode.xml ..\OA3.HWDecode.xml
copy /y OA3.HWDecode.xml ..\Flash_afuwin\OA3.HWDecode.xml
rem pause