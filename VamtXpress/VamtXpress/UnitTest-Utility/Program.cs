using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Utility;

namespace UnitTest_Utility
{
    class Program
    {
        static void Main(string[] args)
        {
            string userName = "testusr002", password = "P@ssword!", authType = "ntlm", server = "mdosclt001c";
            string dn = "dc=rally,dc=io", filter = "(objectCategory=computer)", scope = "onelevel";

            userName = "Administrator";
            dn = "dc=rally,dc=io";
            //filter = "(&(objectCategory=person)(objectClass=user))";
            filter = "(&(objectCategory=person)(objectClass=user)(cn=Administrator))";
            scope = "subtree";

            var conn = LdapUtility.Connect(userName, password, authType, server, false, 2000);

            var result = LdapUtility.Query(conn, filter, dn, scope);

            Console.WriteLine((result as XmlDocument).InnerXml);

            Console.Read();
        }
    }
}
