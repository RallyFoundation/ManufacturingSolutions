using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.Protocols;
using System.Xml;
using System.Xml.Serialization;
using DataIntegrator.Descriptions.LDAP.DSML;
using DataIntegrator.Helpers.Tracing;

namespace DataIntegrator.Helpers.LDAP
{
    class LDAPHelper
    {
        public LDAPHelper() 
        {
        }

        public LDAPHelper(bool enableTracing, string traceSourceName) 
        {
            this.EnableTracing = enableTracing;
            this.TraceSourceName = traceSourceName;
        }

        public bool EnableTracing { get; set; }

        public string TraceSourceName { get; set; }

        public object Search(string serverAddresses, Authentication authentication, bool isAutoBind, int timeout, string distinguishedName, string filter, string scope) 
        {
            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { serverAddresses, authentication, isAutoBind, timeout, distinguishedName, filter, scope}, this.TraceSourceName);
            }

            object returnValue = null;

            LdapConnection connection = this.getLdapConnection(serverAddresses, authentication, isAutoBind, timeout);

            if (connection != null) 
            {
                System.DirectoryServices.Protocols.SearchRequest request = new System.DirectoryServices.Protocols.SearchRequest();
                request.DistinguishedName = distinguishedName;
                request.Scope = this.getSearchScope(scope);
                request.Filter = filter;

                if (this.EnableTracing)
                {
                    TracingHelper.Trace(new object[] {connection, request}, this.TraceSourceName);
                }

                returnValue = (connection as System.DirectoryServices.Protocols.LdapConnection).SendRequest(request);

                if (this.EnableTracing)
                {
                    TracingHelper.Trace(new object[] { returnValue }, this.TraceSourceName);
                }

                if ((returnValue != null) && (returnValue is System.DirectoryServices.Protocols.SearchResponse))
                {
                    string responseXml = this.getSearchResponseDSML(returnValue as System.DirectoryServices.Protocols.SearchResponse);

                    if (this.EnableTracing)
                    {
                        TracingHelper.Trace(new object[] { responseXml }, this.TraceSourceName);
                    }

                    if (!String.IsNullOrEmpty(responseXml))
                    {
                        returnValue = new XmlDocument();
                        (returnValue as XmlDocument).LoadXml(responseXml);
                    }
                }
            }

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { returnValue }, this.TraceSourceName);
            }

            return returnValue;
        }

        //private void trace(object[] data)
        //{
        //    try
        //    {
        //        System.Diagnostics.TraceSource trace = new System.Diagnostics.TraceSource("DataIntegratorTraceSource");

        //        trace.TraceData(System.Diagnostics.TraceEventType.Information, new Random().Next(), data);

        //        trace.Flush();
        //    }
        //    catch (Exception)
        //    {
        //        //If you want to handle this exception, add your exception handling code here, else you may uncomment the following line to throw this exception out.
        //        throw;
        //    }
        //}

        private System.DirectoryServices.Protocols.LdapConnection getLdapConnection(string serverAddresses, Authentication authentication, bool isAutoBind, int timeout)
        {
            System.DirectoryServices.Protocols.LdapConnection returnValue = null;

            if ((!String.IsNullOrEmpty(serverAddresses)) && (authentication != null))
            {
                string[] servers = serverAddresses.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                System.DirectoryServices.Protocols.LdapDirectoryIdentifier identifier = new System.DirectoryServices.Protocols.LdapDirectoryIdentifier(servers, false, false);

                NetworkCredential credential = new NetworkCredential(authentication.Identifier, authentication.Password);

                returnValue = new System.DirectoryServices.Protocols.LdapConnection(identifier, credential);

                returnValue.AutoBind = isAutoBind;

                returnValue.AuthType = this.getAuthType(authentication.Type);

                if (timeout > 0)
                {
                    returnValue.Timeout = TimeSpan.FromSeconds(timeout);
                }
            }

            return returnValue;
        }

        private System.DirectoryServices.Protocols.AuthType getAuthType(AuthenticationType authType)
        {
            System.DirectoryServices.Protocols.AuthType returnValue = System.DirectoryServices.Protocols.AuthType.Ntlm;

            switch (authType)
            {
                case AuthenticationType.PlainText:
                    returnValue = System.DirectoryServices.Protocols.AuthType.Basic;
                    break;
                case AuthenticationType.X509Certificate:
                    returnValue = System.DirectoryServices.Protocols.AuthType.Basic;
                    break;
                case AuthenticationType.Kerberos:
                    returnValue = System.DirectoryServices.Protocols.AuthType.Kerberos;
                    break;
                case AuthenticationType.NTLM:
                    returnValue = System.DirectoryServices.Protocols.AuthType.Ntlm;
                    break;
                case AuthenticationType.Negociate:
                    returnValue = System.DirectoryServices.Protocols.AuthType.Negotiate;
                    break;
                default:
                    returnValue = System.DirectoryServices.Protocols.AuthType.Anonymous;
                    break;
            }

            return returnValue;
        }

        private string getSearchResponseDSML(System.DirectoryServices.Protocols.SearchResponse response)
        {
            string returnValue = String.Empty;

            if (response != null)
            {
                DataIntegrator.Descriptions.LDAP.DSML.SearchResponse dsmlResponse = new DataIntegrator.Descriptions.LDAP.DSML.SearchResponse();

                dsmlResponse.requestID = response.RequestId;

                if ((response.Entries != null) && (response.Entries.Count > 0))
                {
                    dsmlResponse.searchResultEntry = new DataIntegrator.Descriptions.LDAP.DSML.SearchResultEntry[response.Entries.Count];

                    List<DataIntegrator.Descriptions.LDAP.DSML.DsmlAttr> dsmlAttrs = new List<DataIntegrator.Descriptions.LDAP.DSML.DsmlAttr>();

                    DataIntegrator.Descriptions.LDAP.DSML.DsmlAttr attr = null;

                    System.DirectoryServices.Protocols.DirectoryAttribute directoryAttrb = null;

                    for (int i = 0; i < response.Entries.Count; i++)
                    {
                        dsmlResponse.searchResultEntry[i] = new DataIntegrator.Descriptions.LDAP.DSML.SearchResultEntry();
                        dsmlResponse.searchResultEntry[i].requestID = response.RequestId;
                        dsmlResponse.searchResultEntry[i].dn = response.Entries[i].DistinguishedName;

                        if ((response.Entries[i].Attributes != null) && (response.Entries[i].Attributes.Count > 0))
                        {
                            dsmlResponse.searchResultEntry[i].attr = new DataIntegrator.Descriptions.LDAP.DSML.DsmlAttr[response.Entries[i].Attributes.Count];

                            dsmlAttrs = new List<DataIntegrator.Descriptions.LDAP.DSML.DsmlAttr>();

                            foreach (string attrName in response.Entries[i].Attributes.AttributeNames)
                            {
                                directoryAttrb = response.Entries[i].Attributes[attrName];

                                if ((directoryAttrb != null) && (directoryAttrb.Count > 0))
                                {
                                    attr = new DataIntegrator.Descriptions.LDAP.DSML.DsmlAttr();
                                    attr.name = attrName;
                                    attr.value = new string[directoryAttrb.Count];

                                    for (int j = 0; j < directoryAttrb.Count; j++)
                                    {
                                        if (directoryAttrb[j] is byte[])
                                        {
                                            attr.value[j] = Convert.ToBase64String((directoryAttrb[j] as byte[]), Base64FormattingOptions.None);
                                        }
                                        else
                                        {
                                            attr.value[j] = directoryAttrb[j].ToString();
                                        }
                                    }

                                    dsmlAttrs.Add(attr);
                                }

                                directoryAttrb = null;
                            }

                            if ((dsmlAttrs != null) && (dsmlAttrs.Count > 0))
                            {
                                dsmlAttrs.CopyTo(dsmlResponse.searchResultEntry[i].attr);
                            }

                            dsmlAttrs = null;
                            attr = null;
                        }
                    }

                    Type type = typeof(DataIntegrator.Descriptions.LDAP.DSML.SearchResponse);
                    Type[] types = new Type[5];
                    types[0] = typeof(DataIntegrator.Descriptions.LDAP.DSML.DsmlAttr);
                    types[1] = typeof(DataIntegrator.Descriptions.LDAP.DSML.SearchResultEntry);
                    types[2] = typeof(DataIntegrator.Descriptions.LDAP.DSML.SearchResultReference);
                    types[3] = typeof(DataIntegrator.Descriptions.LDAP.DSML.LDAPResult);
                    types[4] = typeof(DataIntegrator.Descriptions.LDAP.DSML.ResultCode);

                    XmlSerializer serializer = new XmlSerializer(type, types);
                    XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                    namespaces.Add("", "urn:oasis:names:tc:DSML:2:0:core");

                    StringWriter writer = new StringWriter();

                    serializer.Serialize(writer, dsmlResponse, namespaces);

                    writer.Flush();
                    returnValue = writer.ToString();
                    writer.Close();

                    returnValue = returnValue.Replace("&#x", "HEX:");
                }
            }

            return returnValue;
        }

        private System.DirectoryServices.Protocols.SearchScope getSearchScope(string name)
        {
            System.DirectoryServices.Protocols.SearchScope returnValeu = System.DirectoryServices.Protocols.SearchScope.Subtree;

            switch (name.ToLower())
            {
                case ("subtree"):
                    {
                        returnValeu = System.DirectoryServices.Protocols.SearchScope.Subtree;
                        break;
                    }
                case ("base"):
                    {
                        returnValeu = System.DirectoryServices.Protocols.SearchScope.Base;
                        break;
                    }
                case ("onelevel"):
                    {
                        returnValeu = System.DirectoryServices.Protocols.SearchScope.OneLevel;
                        break;
                    }
                default:
                    {
                        returnValeu = System.DirectoryServices.Protocols.SearchScope.Subtree;
                        break;
                    }
            }

            return returnValeu;
        }
    }
}
