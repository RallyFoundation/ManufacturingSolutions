rem @echo off

set toolDir=%cd%
echo %toolDir%
cd /d  %toolDir%

rem runas /user:administrator  "afuwin.exe   /OAD"
rem runas /user:administrator  "afuwin.exe   /Aoa3.bin"

rem afuwin.exe   /OAD
rem afuwin.exe   /Aoa3.bin


call 0DelKey.bat
call 0FlashKey.bat

