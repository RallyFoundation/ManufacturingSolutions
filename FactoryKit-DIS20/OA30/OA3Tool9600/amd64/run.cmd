:rem== Creates a directory on Windows partition to hold outputs of running oa3tool.exe
mkdir C:\oa3

:rem== Invokes oa3tool.exe located on the mobile HD attached to assemble a key from the OA3.0 key provider server specified in the oa3tool configuration file
.\oa3tool.exe /Assemble /Configfile=.\oa3tool-serverbased.cfg

:rem== Copies and renames the output file containing key information to the directory just created
copy .\OA3_OUT.xml C:\oa3\OA3_OUT_Assemble.xml

:rem== Copies the output file containing key data binaries to the directory just created
copy .\OA3.bin C:\oa3\

:rem== Invokes DPK injection tool to flash the key data binary file to machine's firmware
:rem==[running DPK injection tool here]

:rem== Invokes oa3tool.exe located on the mobile HD attached to report the key just injected along with the hardware hash generated back to the OA3.0 key provider server specified in the oa3tool configuration file, and logs tracing information
.\oa3tool.exe /Report /Configfile=.\oa3tool-serverbased.cfg /LogTrace=C:\oa3\oa3log-report.xml

:rem== Copies and renames the output file containing key information to the directory just created
copy .\OA3_OUT.xml C:\oa3\OA3_OUT_Report.xml

:rem== Checks if the DPK injected matches the edition of the Windows deployed
.\oa3tool.exe /CheckEdition /online

:rem== Checks if the hardware hash generated during the key report process matches the one that's generated again this time
.\oa3tool.exe /CheckHwHash=C:\oa3\oa3log-report.xml /LogTrace=C:\oa3\oa3log-checkhwhash.xml

