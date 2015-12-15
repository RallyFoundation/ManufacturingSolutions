-- =============================================
-- Script Template
-- =============================================

IF NOT EXISTS(SELECT TOP 1 1 FROM Configuration)
BEGIN
    INSERT Configuration(Name,Value,[Type]) VALUES(N'IsAutoDiagnostic', '<boolean>false</boolean>', N'System.Boolean')
    insert Configuration(Name,Value,[Type]) values(N'FulfillmentInterval', '<int>600000</int>', N'System.Int32')
    insert Configuration(Name,Value,[Type]) values(N'ReportInterval', '<int>600000</int>', N'System.Int32')
    insert Configuration(Name,Value,[Type]) values(N'IsAutoFulfillmentOn', '<boolean>true</boolean>', N'System.Boolean')
    insert Configuration(Name,Value,[Type]) values(N'IsAutoReportOn', '<boolean>true</boolean>', N'System.Boolean')
    insert Configuration(Name,Value,[Type]) values(N'IsRequireOHRData', '<boolean>false</boolean>', N'System.Boolean')
	insert Configuration(Name,Value,[Type]) values(N'OldTimeline', '<int>180</int>', N'System.Int32')
    insert Configuration(Name,Value,[Type]) values(N'IsEncryptExportedFile', '<boolean>true</boolean>', N'System.Boolean')
     insert Configuration(Name,Value,[Type]) values(N'CertificateSubject', 
	'<DisCert xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
    <Subject></Subject>
    <ThumbPrint></ThumbPrint>
    </DisCert>',
    N'DIS.Data.DataContract.DisCert')
    insert Configuration(Name,Value,[Type]) values(N'InternalServiceConfig', '<ServiceConfig xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <UserName></UserName>
  <UserKey></UserKey>
  <ServiceHostUrl>http://localhost:81/KeyBinding.svc</ServiceHostUrl>
</ServiceConfig>', N'DIS.Data.DataContract.ServiceConfig')
    insert Configuration(Name,Value,[Type]) values(N'MsServiceConfig', '<ServiceConfig xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <UserName>1234560000</UserName>
  <UserKey>40bd001563085fc35165329ea1ff5c5ecbdbbeef</UserKey>
  <ServiceHostUrl>https://api.microsoftoem.com</ServiceHostUrl>
</ServiceConfig>', N'DIS.Data.DataContract.ServiceConfig')
    insert Configuration(Name,Value,[Type]) values(N'ProxySetting', '<ProxySetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <ProxyType>Default</ProxyType>
  <BypassProxyOnLocal>false</BypassProxyOnLocal>
  <ServiceConfig>
    <ServiceHostUrl>http://proxyUrl</ServiceHostUrl>
    <UserName>username</UserName>
    <UserKey>password</UserKey>
  </ServiceConfig>
</ProxySetting>', N'DIS.Data.DataContract.ProxySetting')
    insert Configuration(Name,Value,[Type]) values(N'IsMsServiceEnabled', '<boolean>false</boolean>', N'System.Boolean')
END
