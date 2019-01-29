using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.DirectoryServices.Protocols;
using System.Xml;
using System.Xml.Serialization;
using OASIS.DSML;

namespace Utility
{
    public class LdapUtility
    {
        public static object Connection(string UserName, string Password, string AuthType, string Server, bool IsAutoBind, int Timeout)
        {
            LdapConnection returnValue = null;

            returnValue = getLdapConnection(UserName, Password, AuthType, Server, IsAutoBind, Timeout);

            return returnValue;
        }

        public static object Query(object Connection, string Filter, string DistinguishedName, string Scope)
        {
            object returnValue = null;

            if ((Connection != null) && (Connection is System.DirectoryServices.Protocols.LdapConnection) && (!String.IsNullOrEmpty(Filter)) && (!String.IsNullOrEmpty(Scope)))
            {
                System.DirectoryServices.Protocols.SearchRequest request = new System.DirectoryServices.Protocols.SearchRequest();

                if (!String.IsNullOrEmpty(DistinguishedName))
                {
                    request.DistinguishedName = DistinguishedName;
                }

                request.Scope = getSearchScope(Scope);
                request.Filter = Filter;

                returnValue = (Connection as System.DirectoryServices.Protocols.LdapConnection).SendRequest(request);

                if ((returnValue != null) && (returnValue is System.DirectoryServices.Protocols.SearchResponse))
                {
                    string responseXml = GetSearchResponseXml(returnValue as System.DirectoryServices.Protocols.SearchResponse);

                    if (!String.IsNullOrEmpty(responseXml))
                    {
                        returnValue = new XmlDocument();
                        (returnValue as XmlDocument).LoadXml(responseXml);
                    }
                }
            }

            return returnValue;
        }

        private static LdapConnection getLdapConnection(string userName, string password, string authType, string server, bool isAutoBind, int timeout)
        {
            LdapConnection returnValue = null;

            if ((!String.IsNullOrEmpty(server)) && (!String.IsNullOrEmpty(userName)) && (!String.IsNullOrEmpty(password)) && (!String.IsNullOrEmpty(authType)))
            {
                string[] servers = server.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                LdapDirectoryIdentifier identifier = new LdapDirectoryIdentifier(servers, false, false);

                NetworkCredential credential = new NetworkCredential(userName, password);

                returnValue = new LdapConnection(identifier, credential);

                returnValue.AutoBind = isAutoBind;

                returnValue.AuthType = getAuthType(authType);

                if (timeout > 0)
                {
                    returnValue.Timeout = TimeSpan.FromSeconds(timeout);
                }
            }

            return returnValue;
        }

        private static AuthType getAuthType(string authType)
        {
            AuthType returnValue = AuthType.Ntlm;

            switch (authType.ToLower())
            {

                case ("anonymous"):
                    {
                        returnValue = AuthType.Anonymous;
                        break;
                    }
                case ("basic"):
                    {
                        returnValue = AuthType.Basic;
                        break;
                    }
                case ("digest"):
                    {
                        returnValue = AuthType.Digest;
                        break;
                    }
                case ("dpa"):
                    {
                        returnValue = AuthType.Dpa;
                        break;
                    }
                case ("external"):
                    {
                        returnValue = AuthType.External;
                        break;
                    }
                case ("kerberos"):
                    {
                        returnValue = AuthType.Kerberos;
                        break;
                    }
                case ("msn"):
                    {
                        returnValue = AuthType.Msn;
                        break;
                    }
                case ("ntlm"):
                    {
                        returnValue = AuthType.Ntlm;
                        break;
                    }
                case ("negotiate"):
                    {
                        returnValue = AuthType.Negotiate;
                        break;
                    }
                case ("sicily"):
                    {
                        returnValue = AuthType.Sicily;
                        break;
                    }
                default:
                    {
                        returnValue = AuthType.Ntlm;
                        break;
                    }
            }

            return returnValue;
        }

        private static string GetSearchResponseXml(System.DirectoryServices.Protocols.SearchResponse response)
        {
            string returnValue = String.Empty;

            if (response != null)
            {
                OASIS.DSML.SearchResponse dsmlResponse = new OASIS.DSML.SearchResponse();

                dsmlResponse.requestID = response.RequestId;

                if ((response.Entries != null) && (response.Entries.Count > 0))
                {
                    dsmlResponse.searchResultEntry = new OASIS.DSML.SearchResultEntry[response.Entries.Count];

                    List<OASIS.DSML.DsmlAttr> dsmlAttrs = new List<OASIS.DSML.DsmlAttr>();

                    OASIS.DSML.DsmlAttr attr = null;

                    System.DirectoryServices.Protocols.DirectoryAttribute directoryAttrb = null;

                    for (int i = 0; i < response.Entries.Count; i++)
                    {
                        dsmlResponse.searchResultEntry[i] = new OASIS.DSML.SearchResultEntry();
                        dsmlResponse.searchResultEntry[i].requestID = response.RequestId;
                        dsmlResponse.searchResultEntry[i].dn = response.Entries[i].DistinguishedName;

                        if ((response.Entries[i].Attributes != null) && (response.Entries[i].Attributes.Count > 0))
                        {
                            dsmlResponse.searchResultEntry[i].attr = new OASIS.DSML.DsmlAttr[response.Entries[i].Attributes.Count];

                            dsmlAttrs = new List<OASIS.DSML.DsmlAttr>();

                            foreach (string attrName in response.Entries[i].Attributes.AttributeNames)
                            {
                                directoryAttrb = response.Entries[i].Attributes[attrName];

                                if ((directoryAttrb != null) && (directoryAttrb.Count > 0))
                                {
                                    attr = new OASIS.DSML.DsmlAttr();
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

                    Type type = typeof(OASIS.DSML.SearchResponse);
                    Type[] types = new Type[5];
                    types[0] = typeof(OASIS.DSML.DsmlAttr);
                    types[1] = typeof(OASIS.DSML.SearchResultEntry);
                    types[2] = typeof(OASIS.DSML.SearchResultReference);
                    types[3] = typeof(OASIS.DSML.LDAPResult);
                    types[4] = typeof(OASIS.DSML.ResultCode);

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

        private static SearchScope getSearchScope(string name)
        {
            SearchScope returnValue = SearchScope.Subtree;

            switch (name.ToLower())
            {
                case ("subtree"):
                    {
                        returnValue = SearchScope.Subtree;
                        break;
                    }
                case ("base"):
                    {
                        returnValue = SearchScope.Base;
                        break;
                    }
                case ("onelevel"):
                    {
                        returnValue = SearchScope.OneLevel;
                        break;
                    }
                default:
                    {
                        returnValue = SearchScope.Subtree;
                        break;
                    }
            }

            return returnValue;
        }
    }
}
