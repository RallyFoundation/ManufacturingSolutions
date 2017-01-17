using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using System.Xml.Schema;
using System.Security.Cryptography;

namespace OA3ToolConfGen
{
    class Utility
    {
        /// <summary>
        /// aes decryption.
        /// </summary>
        /// <param name="decryptString">decryption string.</param>
        /// <param name="salt">The specified salt size is not smaller than 8 bytes or the iteration count can't less than 1.</param>
        /// <param name="password">To generate HMACSHA1 password.</param>
        /// <returns>Decrypted byte array.</returns>
        public static byte[] DecryptString(string decryptString, string salt, string password = "")
        {
            byte[] encryptBytes = Convert.FromBase64String(decryptString);
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
            //Provide a AES algorithm implementation 
            AesManaged aes = new AesManaged();
            //Generate a random number based on System.Security.Cryptography.HMACSHA1,Implement password-based key derivation function(PBKDF2)
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, saltBytes);
            //The block size of the encryption operation (in bits).
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            //The key size for symmetric algorithms (in bits).
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;
            //Keys for symmetric algorithms.
            aes.Key = rfc.GetBytes(aes.KeySize / 8);
            //Initialization vector (IV) for the symmetric algorithm.
            aes.IV = rfc.GetBytes(aes.BlockSize / 8);

            //Creates a symmetric decryptor object with the current Key property and initialization vector.
            ICryptoTransform decryptTransform = aes.CreateDecryptor();
            //Decrypted output stream.
            MemoryStream decryptStream = new MemoryStream();
            //Connect the target stream(decryptStream) with decryptTransform.
            CryptoStream decryptor = new CryptoStream(
                decryptStream, decryptTransform, CryptoStreamMode.Write);
            //write byte to CryptoStream (decryption process).
            decryptor.Write(encryptBytes, 0, encryptBytes.Length);
            decryptor.Close();

            //Convert the decrypted stream to a byte array.
            return decryptStream.ToArray();

        }

        /// <summary>
        /// aes decryption.
        /// </summary>
        /// <param name="decryptString">decryption string.</param>
        /// <param name="key">The specified salt size is not smaller than 8 bytes or the iteration count can't less than 1.</param>
        /// <returns>Decrypted byte array.</returns>
        public static string Decrypt(string encryptString, string key)
        {
            try
            {
                return Encoding.UTF8.GetString(DecryptString(encryptString, key));
            }
            catch
            {
                return encryptString;
            }
        }

        public static void ParseConnectionString(string ConnectionString, out string ServerName, out string DatabaseName, out string UserName, out string Password)
        {
            string[] fields = ConnectionString.Split(new string[] { ";" }, StringSplitOptions.None);

            ServerName = null;
            DatabaseName = null;
            UserName = null;
            Password = null;

            if ((fields != null) && (fields.Length == 4))
            {
                string[] pair = null;

                for (int i = 0; i < fields.Length; i++)
                {
                    pair = fields[i].Split(new string[] { "=" }, StringSplitOptions.None);

                    switch (i)
                    {
                        case 0:
                            {
                                ServerName = pair[1];
                                break;
                            }
                        case 1:
                            {
                                DatabaseName = pair[1];
                                break;
                            }
                        case 2:
                            {
                                UserName = pair[1];
                                break;
                            }
                        case 3:
                            {
                                Password = pair[1];
                                break;
                            }
                    }
                }
            }
        }

        public static string BuildConnectionString(string ServerName, string DatabaseName, string UserName, string Password)
        {
            return String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", ServerName, DatabaseName, UserName, Password);
        }

        public static void ParseConnectionString(string ConnectionString, out string ServerName, out string PortNumber, out string DatabaseName, out string UserName, out string Password)
        {
            string[] fields = ConnectionString.Split(new string[] { ";" }, StringSplitOptions.None);

            ServerName = null;
            PortNumber = null;
            DatabaseName = null;
            UserName = null;
            Password = null;

            if ((fields != null) && (fields.Length > 0))
            {
                string[] pair = null;

                for (int i = 0; i < fields.Length; i++)
                {
                    pair = fields[i].Split(new string[] { "=" }, StringSplitOptions.None);

                    if ((pair != null) && (pair.Length >= 2))
                    {
                        switch (pair[0].ToLower())
                        {
                            case "data source":
                                {
                                    ServerName = pair[1];
                                    break;
                                }
                            case "initial catalog":
                                {
                                    DatabaseName = pair[1];
                                    break;
                                }
                            case "user id":
                                {
                                    UserName = pair[1];
                                    break;
                                }
                            case "password":
                                {
                                    Password = pair[1];
                                    break;
                                }
                            case "port":
                                {
                                    PortNumber = pair[1];
                                    break;
                                }
                        }
                    }
                }
            }
        }

        public static string BuildConnectionString(string ServerName, string PortNumber, string DatabaseName, string UserName, string Password)
        {
            return String.Format("Data Source={0};Port={1};Initial Catalog={2};User ID={3};Password={4}", ServerName, PortNumber, DatabaseName, UserName, Password);
        }
        public static byte[] BinarySerialize(object objectToSerialize)
        {
            byte[] returnValue = null;

            IFormatter formater = new BinaryFormatter();

            using (MemoryStream stream = new MemoryStream())
            {
                formater.Serialize(stream, objectToSerialize);

                returnValue = stream.GetBuffer();
            }

            return returnValue;
        }

        public static object BinaryDeserialize(byte[] objectBytes)
        {
            object returnValue = null;

            IFormatter formater = new BinaryFormatter();

            using (MemoryStream stream = new MemoryStream(objectBytes))
            {
                returnValue = formater.Deserialize(stream);
            }

            return returnValue;
        }

        public static SchemaValidationListener NewSchemaValidationListener()
        {
            return new SchemaValidationListener();
        }

        public static bool IsXmlValid(string xml, string schemaUri, ValidationEventHandler schemaValidationEventHandler)
        {
            bool returnValue = false;

            if ((!String.IsNullOrEmpty(schemaUri)) && (!String.IsNullOrEmpty(xml)))
            {
                XmlReader xmlSchemaReader = XmlReader.Create(schemaUri);

                if (xmlSchemaReader != null)
                {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.ValidationType = ValidationType.Schema;
                    settings.ConformanceLevel = ConformanceLevel.Auto;
                    settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
                    settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessIdentityConstraints;

                    if (schemaValidationEventHandler != null)
                    {
                        settings.ValidationEventHandler += schemaValidationEventHandler;
                    }

                    XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();

                    xmlSchemaSet.Add(null, xmlSchemaReader);

                    xmlSchemaSet.Compile();

                    settings.Schemas = xmlSchemaSet;

                    StringReader xmlDocumentStringReader = new StringReader(xml);

                    XmlReader xmlDocumentReader = XmlReader.Create(xmlDocumentStringReader, settings);

                    while (xmlDocumentReader.Read())
                    {
                    }

                    #region Tracing
                    FileStream stream = new FileStream(schemaUri, FileMode.Open, FileAccess.Read, FileShare.Read);

                    StreamReader reader = new StreamReader(stream);

                    reader.Close();
                    stream.Close();
                    #endregion

                    returnValue = true;
                }
            }

            return returnValue;
        }

        public static string ValidateXML(string xml, string schema)
        {
            string returnValue = String.Empty;

            if ((!String.IsNullOrEmpty(xml)) && (!String.IsNullOrEmpty(schema)))
            {
                StringReader stringReader = new StringReader(schema);

                XmlReader xmlSchemaReader = XmlReader.Create(stringReader);

                if (xmlSchemaReader != null)
                {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.ValidationType = ValidationType.Schema;
                    settings.ConformanceLevel = ConformanceLevel.Auto;
                    settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
                    settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessIdentityConstraints;

                    SchemaValidationListener listener = new SchemaValidationListener();

                    settings.ValidationEventHandler += listener.OnSchemaValidating;

                    XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();

                    xmlSchemaSet.Add(null, xmlSchemaReader);

                    xmlSchemaSet.Compile();

                    settings.Schemas = xmlSchemaSet;

                    StringReader xmlDocumentStringReader = new StringReader(xml);

                    XmlReader xmlDocumentReader = XmlReader.Create(xmlDocumentStringReader, settings);

                    while (xmlDocumentReader.Read())
                    {
                    }

                    returnValue = String.Format("Error count:{0};\r\n Error messages:\r\n{1}", listener.ErrorCount, listener.ErrorMessage);
                }
            }

            return returnValue;
        }

        public static string XmlSerialize(object objectToSerialize, Type[] extraTypes, string outputEncodingName)
        {
            string returnValue = String.Empty;

            XmlSerializer serializer = new XmlSerializer(objectToSerialize.GetType(), extraTypes);

            MemoryStream stream = new MemoryStream();

            serializer.Serialize(stream, objectToSerialize);

            byte[] bytes = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, bytes.Length);

            Encoding outputEncoding = String.IsNullOrEmpty(outputEncodingName) ? Encoding.Default : Encoding.GetEncoding(outputEncodingName);

            returnValue = outputEncoding.GetString(bytes);

            stream.Close();

            return returnValue;
        }

        public static object XmlDeserialize(string xml, Type type, Type[] extraTypes, string inputEncodingName)
        {
            object returnValue = null;

            XmlSerializer serializer = new XmlSerializer(type, extraTypes);

            Encoding inputEncoding = String.IsNullOrEmpty(inputEncodingName) ? Encoding.Default : Encoding.GetEncoding(inputEncodingName);

            MemoryStream stream = new MemoryStream(inputEncoding.GetBytes(xml));

            returnValue = serializer.Deserialize(stream);

            stream.Close();

            return returnValue;
        }

        public static string XmlTransform(string xml, string xsltUri, string outputEncodingName)
        {
            string returnValue = String.Empty;

            XslCompiledTransform transform = new XslCompiledTransform();

            transform.Load(xsltUri);

            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);

            MemoryStream stream = new MemoryStream();

            transform.Transform(document, null, stream);

            //byte[] bytes = stream.GetBuffer();

            byte[] bytes = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, bytes.Length);

            //returnValue = Encoding.UTF8.GetString(bytes);

            //returnValue = Encoding.Default.GetString(bytes);

            Encoding outputEncoding = String.IsNullOrEmpty(outputEncodingName) ? Encoding.Default : Encoding.GetEncoding(outputEncodingName);

            returnValue = outputEncoding.GetString(bytes);

            stream.Close();

            return returnValue;
        }
    }

    public class SchemaValidationListener
    {
        public SchemaValidationListener()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private int schemaValidationErrorCount;
        private string schemaValidationErrorMessage;

        public int ErrorCount
        {
            get
            {
                return this.schemaValidationErrorCount;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return this.schemaValidationErrorMessage;
            }
        }

        public void OnSchemaValidating(object sender, ValidationEventArgs e)
        {
            this.schemaValidationErrorCount++;
            this.schemaValidationErrorMessage += e.Message;
            this.schemaValidationErrorMessage += "\r\n";
        }
    }
}
