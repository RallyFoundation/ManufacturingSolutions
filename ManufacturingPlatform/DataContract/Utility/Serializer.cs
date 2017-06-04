using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace DIS.Common.Utility
{
    /// <summary>
    /// Provides serialization methods
    /// </summary>
    public static class Serializer
    {
        /// <summary>
        /// Serializes an object and write the XML string to disk.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        /// <param name="indent"></param>
        /// <param name="fileMode"></param>
        public static void WriteToXml(object obj, string path, bool indent = true, FileMode fileMode = FileMode.Create)
        {
            using (FileStream stream = new FileStream(path, fileMode, FileAccess.Write, FileShare.None))
            {
                ToXml(obj, stream, indent);
            }
        }

        /// <summary>
        /// Serializes an object to XML string.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="indent"></param>
        /// <returns></returns>
        public static string ToXml(this object obj, bool indent = true)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                ToXml(obj, stream, indent);
                byte[] buffer = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer);
            }
        }

        public static string ToXml(this object obj, bool noNamespace, bool indent = true)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                ToXml(obj, stream, indent, noNamespace);
                byte[] buffer = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer);
            }
        }

        private static void ToXml(object obj, Stream stream, bool indent)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            using (XmlWriter writer = XmlWriter.Create(stream, new XmlWriterSettings()
            {
                Encoding = Encoding.UTF8,
                Indent = indent
            }))
            {
                serializer.Serialize(writer, obj);
            }
        }

        private static void ToXml(object obj, Stream stream, bool indent, bool noNamespace)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            using (XmlWriter writer = XmlWriter.Create(stream, new XmlWriterSettings()
            {
                Encoding = Encoding.UTF8,
                Indent = indent
            }))
            {
                serializer.Serialize(writer, obj, ns);
            }
        }

        /// <summary>
        /// Deserializes an object from the XML string read from disk.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="fileMode"></param>
        /// <returns></returns>
        public static T ReadFromXml<T>(string path, FileMode fileMode = FileMode.Open)
        {
            using (FileStream stream = new FileStream(path, fileMode, FileAccess.Read, FileShare.Read))
            {
                return FromXml<T>(stream);
            }
        }

        /// <summary>
        /// Deserializes an object from XML string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T FromXml<T>(this string xml)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                return FromXml<T>(stream);
            }
        }

        private static T FromXml<T>(Stream stream)
        {

            T obj;
            using (XmlReader reader = XmlReader.Create(stream))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                obj = (T)serializer.Deserialize(reader);
            }
            return obj;

        }

        /// <summary>
        /// Convert DataContract to XML
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Convert XML back to DataContract
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
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