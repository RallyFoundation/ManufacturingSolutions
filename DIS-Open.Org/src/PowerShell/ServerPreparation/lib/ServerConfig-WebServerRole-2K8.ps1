#Import-Module ServerManager

##Adding IIS 7.x Web Server Role
Add-WindowsFeature Web-Server
Add-WindowsFeature Web-WebServer
Add-WindowsFeature Web-Common-Http
Add-WindowsFeature Web-Static-Content
Add-WindowsFeature Web-Default-Doc
Add-WindowsFeature Web-Dir-Browsing
Add-WindowsFeature Web-Http-Errors
Add-WindowsFeature Web-Http-Redirect
Add-WindowsFeature Web-DAV-Publishing
Add-WindowsFeature Web-App-Dev
Add-WindowsFeature Web-Asp-Net
Add-WindowsFeature Web-Net-Ext
Add-WindowsFeature Web-ASP
Add-WindowsFeature Web-CGI
Add-WindowsFeature Web-ISAPI-Ext
Add-WindowsFeature Web-ISAPI-Filter
Add-WindowsFeature Web-Includes
Add-WindowsFeature Web-Health
Add-WindowsFeature Web-Http-Logging
Add-WindowsFeature Web-Log-Libraries
Add-WindowsFeature Web-Request-Monitor
Add-WindowsFeature Web-Http-Tracing
Add-WindowsFeature Web-Custom-Logging
Add-WindowsFeature Web-ODBC-Logging
Add-WindowsFeature Web-Security
Add-WindowsFeature Web-Basic-Auth
Add-WindowsFeature Web-Windows-Auth
Add-WindowsFeature Web-Digest-Auth
Add-WindowsFeature Web-Client-Auth
Add-WindowsFeature Web-Cert-Auth
Add-WindowsFeature Web-Url-Auth
Add-WindowsFeature Web-Filtering
Add-WindowsFeature Web-IP-Security
Add-WindowsFeature Web-Performance
Add-WindowsFeature Web-Stat-Compression
Add-WindowsFeature Web-Dyn-Compression
Add-WindowsFeature Web-Mgmt-Tools
Add-WindowsFeature Web-Mgmt-Console
Add-WindowsFeature Web-Scripting-Tools
Add-WindowsFeature Web-Mgmt-Service
Add-WindowsFeature Web-Mgmt-Compat
Add-WindowsFeature Web-Metabase
Add-WindowsFeature Web-WMI
Add-WindowsFeature Web-Lgcy-Scripting
Add-WindowsFeature Web-Lgcy-Mgmt-Console
Add-WindowsFeature Web-WHC