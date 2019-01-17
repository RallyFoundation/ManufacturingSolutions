Start-Process "node" -ArgumentList @(".\Bin\Vamt.js\vamt.js");
#Start-Process "node" -ArgumentList @(".\Bin\OData\server-odata.js");

#cd .\Bin\Ftp\
#Start-Process "..\..\Lib\python\python.exe" -ArgumentList @(".\PythonFTP.py");

#cd ..\..\Bin\Frontend\;
#Start-Process "..\..\Lib\netcore\dotnet.exe" -ArgumentList @(".\WindowsManufacturingCloud.dll");