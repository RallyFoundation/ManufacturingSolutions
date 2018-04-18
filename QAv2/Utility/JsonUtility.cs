using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace QA.Utility
{
    public class JsonUtility
    {
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
    }
}
