Import-Module "E:\D\home\v-rawang\Documents\Visual Studio 2013\Projects\OA3DPKIDSNManager\PowerShellOA3DPKSNBinder\bin\Debug\PowerShellOA3DPKSNBinder.dll"

Add-DPKIDSNBinding -ProductKeyID 9079000209548 -SerialNumber ABCD1234 -PersistencyMode RDBMSSQLServer -DBConnectionString "Data Source=.\ADK;Initial Catalog=OA3DPKSN;Integrated Security=True" 

Add-DPKIDSNBinding -ProductKeyID 9079000209548 -SerialNumber ABCD1234 -PersistencyMode FileSystemXML -FilePath E:\dpksn.xml 