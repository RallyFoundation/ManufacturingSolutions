using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace WebView
{
    class Utility
    {
        public static string[] GetRemovableDrives()
        {
            List<string> drives = null;

            DriveInfo[] driveInfoes = DriveInfo.GetDrives();

            if ((driveInfoes != null) && (driveInfoes.Length > 0))
            {
                drives = new List<string>();

                foreach (var driveInfo in driveInfoes)
                {
                    if ((driveInfo.DriveType == DriveType.Removable) && (driveInfo.IsReady == true))
                    {
                        drives.Add(driveInfo.Name);
                    }
                }

                return drives.ToArray();
            }

            return null;
        }
        public static void CopyDirectory(string Source, string Destination)
        {
            String[] Files;

            if (Destination[Destination.Length - 1] != Path.DirectorySeparatorChar)
            {
                Destination += Path.DirectorySeparatorChar;
            }

            if (!Directory.Exists(Destination))
            {
                Directory.CreateDirectory(Destination);
            }

            Files = Directory.GetFileSystemEntries(Source);

            foreach (string Element in Files)
            {
                // Sub directories
                if (Directory.Exists(Element))
                {
                    CopyDirectory(Element, Destination + Path.GetFileName(Element));
                }
                // Files in directory
                else
                {
                    File.Copy(Element, Destination + Path.GetFileName(Element), true);
                }
            }
        }

        public static void SetXmlDocumentAttributeValue(string filePath, string xPath, string value)
        {
            XmlDocument document = new XmlDocument();

            document.Load(filePath);

            document.SelectSingleNode(xPath).Value = value;

            document.Save(filePath);
        }

        public static string XmlSerialize(object objectToSerialize, Type[] extraTypes, string outputEncodingName)
        {
            string returnValue = String.Empty;

            XmlSerializer serializer = new XmlSerializer(objectToSerialize.GetType(), extraTypes);

            using (MemoryStream stream = new MemoryStream())
            {

                serializer.Serialize(stream, objectToSerialize);

                //byte[] bytes = stream.GetBuffer();

                //returnValue = Encoding.UTF8.GetString(bytes);

                byte[] bytes = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(bytes, 0, bytes.Length);

                Encoding outputEncoding = String.IsNullOrEmpty(outputEncodingName) ? Encoding.Default : Encoding.GetEncoding(outputEncodingName);

                returnValue = outputEncoding.GetString(bytes);
            }

            return returnValue;
        }

        public static object XmlDeserialize(string xml, Type type, Type[] extraTypes, string inputEncodingName)
        {
            object returnValue = null;

            XmlSerializer serializer = new XmlSerializer(type, extraTypes);

            //MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));

            Encoding inputEncoding = String.IsNullOrEmpty(inputEncodingName) ? Encoding.Default : Encoding.GetEncoding(inputEncodingName);

            using (MemoryStream stream = new MemoryStream(inputEncoding.GetBytes(xml)))
            {
                returnValue = serializer.Deserialize(stream);
            }

            return returnValue;
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


        public static string JsonSerialize(object objectToSerialize, Type objectType)
        {
            string result = "";

            JsonSerializer serializer = new JsonSerializer();

            using (StringWriter stringWriter = new StringWriter())
            {
                using (JsonTextWriter writer = new JsonTextWriter(stringWriter))
                {
                    serializer.Serialize(stringWriter, objectToSerialize, objectType);
                }      

                result = stringWriter.ToString();
            }

            return result;
        }

        public static object JsonDeserialize<T>(string jsonValue)
        {
            object result = null;

            JsonSerializer serializer = new JsonSerializer();

            using (StringReader stringReader = new StringReader(jsonValue))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(stringReader))
                {
                    result = serializer.Deserialize<T>(jsonReader);
                }
            }

            return result;
        }

        public static byte[] JsonSerialize(object objectToSerialize, Type[] extraTypes, string rootName)
        {
            DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();

            settings.KnownTypes = extraTypes;
            settings.RootName = rootName;
            settings.DateTimeFormat = new DateTimeFormat("yyyy-MM-ddTHH:mm:ss.fffZ");

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(objectToSerialize.GetType(), settings);

            byte[] buffer;

            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, objectToSerialize);
                //stream.Flush();
                //buffer = stream.GetBuffer();

                buffer = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(buffer, 0, buffer.Length);
            }

            return buffer;
        }

        public static byte[] JsonSerialize(object objectToSerialize, Type[] extraTypes, string rootName, string dateTimeFormat)
        {
            DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();

            settings.KnownTypes = extraTypes;
            settings.RootName = rootName;

            string dateTimeFormatString = !String.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "yyyy-MM-ddTHH:mm:ss.fffZ";

            settings.DateTimeFormat = new System.Runtime.Serialization.DateTimeFormat(dateTimeFormatString);

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(objectToSerialize.GetType(), settings);

            byte[] buffer;

            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, objectToSerialize);
                //stream.Flush();
                //buffer = stream.GetBuffer();

                buffer = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(buffer, 0, buffer.Length);
            }

            return buffer;
        }

        public static object JsonDeserialize(byte[] jsonBytes, Type objectType, Type[] extraTypes, string rootName)
        {
            DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();

            settings.KnownTypes = extraTypes;
            settings.RootName = rootName;
            settings.DateTimeFormat = new DateTimeFormat("yyyy-MM-ddTHH:mm:ss.fffZ");

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(objectType, settings);

            object returnObject;

            using (MemoryStream stream = new MemoryStream(jsonBytes))
            {
                returnObject = serializer.ReadObject(stream);
            }

            return returnObject;
        }


        public static object JsonDeserialize(byte[] jsonBytes, Type objectType, Type[] extraTypes, string rootName, string dateTimeFormat)
        {
            DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();

            settings.KnownTypes = extraTypes;
            settings.RootName = rootName;

            string dateTimeFormatString = !String.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "yyyy-MM-ddTHH:mm:ss.fffZ";

            settings.DateTimeFormat = new System.Runtime.Serialization.DateTimeFormat(dateTimeFormatString);

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(objectType, settings);

            object returnObject;

            using (MemoryStream stream = new MemoryStream(jsonBytes))
            {
                returnObject = serializer.ReadObject(stream);
            }

            return returnObject;
        }

        public static object EmitObject(String AssemblyName, String TypeName)
        {
            ObjectHandle objectHandle = Activator.CreateInstance(AssemblyName, TypeName);

            return objectHandle.Unwrap();
        }

        public static object EmitObject(String AssemblyName, String TypeName, object[] arguments)
        {
            Assembly assembly = Assembly.Load(AssemblyName);

            Type type = assembly.GetType(TypeName);

            return Activator.CreateInstance(type, arguments);
        }

        public static string GetFullPath(string RelativePath)
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;

            if (rootPath.EndsWith("\\"))
            {
                rootPath = rootPath.Substring(0, (rootPath.Length - 1));
            }

            if (!RelativePath.StartsWith("\\"))
            {
                RelativePath = "\\" + RelativePath;
            }

            return rootPath + RelativePath;
        }

        public static string StartProcess(string AppPath, string AppParams, bool IsCreatingNewWindow, bool IsUsingShellExecute)
        {
            Process process = new Process();

            process.StartInfo.FileName = AppPath;
            process.StartInfo.Arguments = AppParams;
            process.StartInfo.UseShellExecute = IsUsingShellExecute;
            process.StartInfo.RedirectStandardError = !IsUsingShellExecute;
            process.StartInfo.RedirectStandardOutput = !IsUsingShellExecute;
            process.StartInfo.CreateNoWindow = !IsCreatingNewWindow;

            process.Start();

            process.WaitForExit();

            string result = "";

            if (!IsUsingShellExecute)
            {
                using (process.StandardOutput)
                {
                    result = process.StandardOutput.ReadToEnd();
                }
            }

            return result;
        }

        public static string StartProcess(string AppPath, string AppParams, bool IsCreatingNewWindow, bool IsUsingShellExecute, bool ShouldWait)
        {
            string result = "";

            Process process = new Process();

            process.StartInfo.FileName = AppPath;
            process.StartInfo.Arguments = AppParams;
            process.StartInfo.UseShellExecute = IsUsingShellExecute;
            process.StartInfo.RedirectStandardError = !IsUsingShellExecute;
            process.StartInfo.RedirectStandardOutput = !IsUsingShellExecute;
            process.StartInfo.CreateNoWindow = !IsCreatingNewWindow;

            process.Start();

            if (ShouldWait)
            {
                process.WaitForExit();
            }

            if (!IsUsingShellExecute)
            {
                using (process.StandardOutput)
                {
                    result = process.StandardOutput.ReadToEnd();
                }
            }

            return result;
        }
    }
}
