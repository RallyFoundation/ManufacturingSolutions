using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Text;

namespace WcfService
{
    public static class Helper
    {
        public static void SaveServiceContractToFile(object obj, string outputPath)
        {
            string xml = obj.ToDataContract();
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xml);
            xDoc.Save(outputPath);
        }

        public static string GetLocalDirectory(string directoryName)
        {
            string currentAssemblyPath = GetExecutingAssemblyPath();
            string result = Path.Combine(currentAssemblyPath, directoryName);

            if (!Directory.Exists(result))
            {
                Directory.CreateDirectory(result);
            }

            return result;
        }

        public static string GetExecutingAssemblyPath()
        {
            ////string path = Environment.CurrentDirectory;
            ////string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            const string path = ".";
            return path;
        }

        public static string ToDataContract(this object obj)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                DataContractSerializer dcSerializer = new DataContractSerializer(obj.GetType());
                var xmlWriterSettings = new XmlWriterSettings
                {
                    Encoding = new UTF8Encoding(false),
                    ConformanceLevel = ConformanceLevel.Document,
                    Indent = true
                };

                XmlWriter xWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);
                dcSerializer.WriteObject(xWriter, obj);
                xWriter.Flush();
                memoryStream.Position = 0;
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(memoryStream);
                return xDoc.InnerXml;
            }
        }

        /// <returns></returns>
        public static T FromDataContract<T>(this string xml)
        {
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                DataContractSerializer dcSerializer = new DataContractSerializer(typeof(T));

                XmlReader reader = XmlReader.Create(memoryStream);
                return (T)dcSerializer.ReadObject(reader);
            }
        }
    }
}