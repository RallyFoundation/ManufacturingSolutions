using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Utility
{
    public class HttpUtility
    {
        public static object Get(string uri, Authentication authentication, Dictionary<string, string> headers)
        {
            return createSyncRequest(uri, null, "GET", authentication, headers);
        }

        public static object Post(string uri, string data, Authentication authentication, Dictionary<string, string> headers)
        {
            return createSyncRequest(uri, data, "POST", authentication, headers);
        }

        private static bool remoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain certificateChain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private static X509Certificate getCertificate(string certificatePath, string password)
        {
            X509Certificate returnValue = new X509Certificate();

            returnValue.Import(certificatePath, password, X509KeyStorageFlags.DefaultKeySet);

            return returnValue;
        }

        private static X509Certificate getCertificate(byte[] certificateData, string password)
        {
            X509Certificate returnValue = new X509Certificate();

            returnValue.Import(certificateData, password, X509KeyStorageFlags.DefaultKeySet);

            return returnValue;
        }

        private static string createSyncRequest(string uri, string message, string method, Authentication authentication, Dictionary<string, string> headers)
        {
            string returnValue = String.Empty;

            #region Prepares request uri

            Uri uriAddress = new Uri(uri);

            #endregion

            #region Prepares request

            HttpWebRequest request = WebRequest.Create(uriAddress) as HttpWebRequest;
            request.Method = method;//Sets http request method(GET or POST)
            request.Accept = "application/xml";
            request.ContentType = "application/xml;charset=utf-8";
            request.CookieContainer = new CookieContainer();

            switch (authentication.Type)
            {
                case AuthenticationType.PlainText:
                    //Attaches User name and password
                    string servicePoint = request.Address.Host;
                    request.Credentials = new NetworkCredential(authentication.Identifier, authentication.Password, servicePoint);
                    break;
                case AuthenticationType.X509Certificate:
                    //Attaches X.509 certificate
                    request.ClientCertificates.Add(getCertificate(authentication.Identifier, authentication.Password));
                    break;
                case AuthenticationType.Kerberos:
                    break;
                case AuthenticationType.NTLM:
                    break;
                case AuthenticationType.Negociate:
                    break;
                default:
                    break;
            }

            ////Attaches X.509 certificate
            //request.ClientCertificates.Add(this.getCertificate(certificateLocation, certificatePassword));
            ////Attaches User name and password
            //request.Credentials = new NetworkCredential(userName, password, servicePoint);

            if ((headers != null) && (headers.Count > 0))
            {
                foreach (string key in headers.Keys)
                {
                    if (key.ToLower() == "content-type")
                    {
                        request.ContentType = headers[key];
                        continue;
                    }

                    if (key.ToLower() == "accept")
                    {
                        request.Accept = headers[key];
                        continue;
                    }

                    request.Headers.Add(key, headers[key]);
                }
            }

            Stream stream = null;

            //Writes XML message to request stream
            if (!String.IsNullOrEmpty(message))
            {
                stream = request.GetRequestStream();

                StreamWriter writer = new StreamWriter(stream);

                writer.Write(message);

                writer.Flush();
                writer.Close();
            }

            #endregion

            #region Processes response

            WebResponse response = null;

            try
            {
                response = request.GetResponse();

                //this.trace(new object[] { "Request information:", request.Headers, request.UserAgent, request.RequestUri, message, request.Method, request.Host, request.Date });
            }
            catch (Exception ex)
            {
                //this.trace(new object[] { "Exception information", ex.ToString(), message, uri, method });

                return ex.ToString();
            }

            if (stream != null)
            {
                stream.Close();
            }

            //Extracts response XML message from response stream
            if (response != null)
            {
                stream = response.GetResponseStream();

                if (stream != null)
                {
                    //if (request.Accept.ToLower().StartsWith("image/") || request.Accept.ToLower().StartsWith("audio/") || request.Accept.ToLower().StartsWith("video/"))
                    //{
                    //    //byte[] buffer = new byte[stream.Length];
                    //    //stream.Seek(0, SeekOrigin.Begin);
                    //    //stream.Read(buffer, 0, buffer.Length);

                    //    byte[] buffer = new byte[1024];
                    //    int bytesRead = stream.Read(buffer, 0, 1024);
                    //    int currentBytes = bytesRead;

                    //    MemoryStream memoryStream = new MemoryStream();
                    //    memoryStream.Write(buffer, 0, buffer.Length);

                    //    while (bytesRead == 1024)
                    //    {
                    //        buffer = new byte[1024];
                    //        bytesRead = stream.Read(buffer, 0, 1024);
                    //        currentBytes += bytesRead;

                    //        memoryStream.Write(buffer, 0, buffer.Length);
                    //    }

                    //    buffer = new byte[memoryStream.Length];
                    //    memoryStream.Seek(0, SeekOrigin.Begin);
                    //    memoryStream.Read(buffer, 0, buffer.Length);

                    //    returnValue = Convert.ToBase64String(buffer);
                    //}
                    //else
                    {
                        StreamReader reader = new StreamReader(stream);
                        returnValue = reader.ReadToEnd();
                    }

                    stream.Close();
                }

                //this.trace(new object[] { "Response information:", response.Headers, response.ResponseUri, returnValue });
            }

            #endregion

            return returnValue;
        }
    }

    public enum AuthenticationType
    {
        PlainText = 0,
        X509Certificate = 1,
        Kerberos = 2,
        NTLM = 3,
        Negociate = 4,
        Custom = 5
    }

    public class Authentication
    {
        public virtual AuthenticationType Type
        {
            get;
            set;
        }

        public virtual string Identifier
        {
            get;
            set;
        }

        public virtual string Password
        {
            get;
            set;
        }

    }
}
