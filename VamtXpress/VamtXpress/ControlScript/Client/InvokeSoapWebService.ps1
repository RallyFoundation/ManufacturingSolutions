$clientHostName = hostname

#If you want to use the client machine's IPv4 Address instead, just uncomment the following 2 lines, replacing the value of -InterfaceAlias corrosponding to your NIC configuration:
#$clientIPv4Config = Get-NetIPAddress -AddressFamily IPv4 -InterfaceAlias "Wi-Fi"
#$clientHostName = $clientIPv4Config.IPAddress.ToString()

$clientUserName = "Administrator"
$clientPassword = "ClientPassword"
$clientNetBiosDomainUserName = $clientHostName + "\" + $clientUserName


[System.String]$body = '<?xml version="1.0" encoding="utf-8"?>
<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
  <soap:Body>
    <ActivateWindows xmlns="http://tempuri.org/">
      <clientHostName>{0}</clientHostName>
      <clientUserName>{1}</clientUserName>
      <clientPassword>{2}</clientPassword>
    </ActivateWindows>
  </soap:Body>
</soap:Envelope>';

$body = [System.String]::Format($body, $clientHostName, $clientNetBiosDomainUserName, $clientPassword);

$headers = @{};

#If you would like to declare a dictionary in a .NET programming way, just uncomment the following 1 line: 
#$headers = New-Object 'System.Collections.Generic.Dictionary[System.String, System.String]';

$headers.Add("SOAPAction", "http://tempuri.org/ActivateWindows");

#Remember to replace the value of -Uri corrosponding to your production environment
[System.Net.HttpWebResponse]$response = Invoke-WebRequest -Uri http://192.168.0.211:8080/Services/VamtService.asmx -Body $body -Method Post -ContentType "text/xml; charset=utf-8"  -Headers $headers;

[System.IO.Stream]$stream = $response.GetResponseStream();

[System.IO.StreamReader]$reader = New-Object System.IO.StreamReader($stream);

[System.String]$resultValue = $reader.ReadToEnd();

$reader.Close();
$stream.Close();

$resultValue;