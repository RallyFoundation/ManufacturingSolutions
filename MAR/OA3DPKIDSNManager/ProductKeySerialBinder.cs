using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OA3DPKIDSNManager
{
    public class ProductKeySerialBinder : IProductKeySerialBinder
    {
        private string dbConnectionString;
        private string sqlInsertCommandText = "INSERT INTO ProductKeyIDSerialNumberPairs (PairID, ProductKeyID, SerialNumber, CreationTime, TransactionID) VALUES (@PairID, @ProductKeyID, @SerialNumber, @CreationTime, @TransactionID)";
        private string filePath = "DPKIDSNPairs.xml";
        private PersistencyMode persistencyMode = PersistencyMode.RDBMSSQLServer;

        public PersistencyMode PersistencyMode 
        { 
            get { return this.persistencyMode; } 
            set { this.persistencyMode = value; } 
        }
        public string Bind(long ProductKeyID, string SerialNumber)
        {
            switch (this.persistencyMode)
            {
                case PersistencyMode.RDBMSSQLServer: 
                { 
                    return this.saveToDB(ProductKeyID, SerialNumber, Utility.CommonUtility.GetMillisecondsOfCurrentDateTime(null));
                    //break;
                }
                case PersistencyMode.FileSystemXML: 
                {
                    return this.saveToXml(ProductKeyID, SerialNumber, Utility.CommonUtility.GetMillisecondsOfCurrentDateTime(null));
                    //break;
                }
                case PersistencyMode.FileSystemJSON: 
                {
                    break;
                }
                default:
                {
                    return this.saveToXml(ProductKeyID, SerialNumber, Utility.CommonUtility.GetMillisecondsOfCurrentDateTime(null));
                    //break;
                }
            }

            return null;
        }

        public string Bind(long ProductKeyID, string SerialNumber, string TransactionID)
        {
            switch (this.persistencyMode)
            {
                case PersistencyMode.RDBMSSQLServer:
                    {
                        return this.saveToDB(ProductKeyID, SerialNumber, TransactionID);
                        //break;
                    }
                case PersistencyMode.FileSystemXML:
                    {
                        return this.saveToXml(ProductKeyID, SerialNumber, TransactionID);
                        //break;
                    }
                case PersistencyMode.FileSystemJSON:
                    {
                        break;
                    }
                default:
                    {
                        return this.saveToXml(ProductKeyID, SerialNumber, TransactionID);
                        //break;
                    }
            }

            return null;
        }

        public void SetDBConnectionString(string DBConnectionString)
        {
            this.dbConnectionString = DBConnectionString;
        }

        public void SetFilePath(string FilePath)
        {
            this.filePath = FilePath;
        }

        public void SetPersistencyMode(int Mode) 
        {
            this.persistencyMode = (PersistencyMode)(Mode);
        }

        private string saveToDB(long ProductKeyID, string SerialNumber, string TransactionID) 
        {
            int returnValue = 0;

            using (SqlConnection connection = new SqlConnection(this.dbConnectionString))
            {
                SqlCommand sqlInsertCommand = connection.CreateCommand();

                sqlInsertCommand.CommandType = CommandType.Text;
                sqlInsertCommand.CommandText = this.sqlInsertCommandText;
                sqlInsertCommand.Parameters.AddRange(new SqlParameter[]
                {
                   new SqlParameter(){ ParameterName = "@PairID", DbType = DbType.Guid, Direction = ParameterDirection.Input, Value = Guid.NewGuid() },
                   new SqlParameter(){ ParameterName = "@ProductKeyID", DbType = DbType.Int64, Direction = ParameterDirection.Input, Value = ProductKeyID },
                   new SqlParameter(){ ParameterName = "@SerialNumber", DbType = DbType.String, Direction = ParameterDirection.Input, Value = SerialNumber },
                   new SqlParameter(){ ParameterName = "@CreationTime", DbType = DbType.DateTime, Direction = ParameterDirection.Input, Value = DateTime.Now },
                   new SqlParameter(){ ParameterName = "@TransactionID", DbType = DbType.String, Direction = ParameterDirection.Input, Value = TransactionID },
                });

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                returnValue = sqlInsertCommand.ExecuteNonQuery();

                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

            return returnValue.ToString();
        }

        private string saveToXml(long ProductKeyID, string SerialNumber, string TransactionID) 
        {
            FileStream stream = null;
            XmlWriter writer = null;

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Encoding = System.Text.Encoding.UTF8,
                CloseOutput = true,
                WriteEndDocumentOnClose = true,
                Indent = true,
                ConformanceLevel = ConformanceLevel.Document
            };

            if (!File.Exists(this.filePath))
            {
               using (stream = new FileStream(this.filePath, FileMode.Create, FileAccess.Write, FileShare.Write))
               {
                    using (writer = XmlWriter.Create(stream, settings)) 
                    { 
                        writer.WriteStartDocument(true);

                        writer.WriteStartElement("Pairs");
                        writer.WriteEndElement();

                        writer.WriteEndDocument();

                        //writer.Flush();
                    }
                }
            }

            XmlDocument document = new XmlDocument();

            using (stream = new FileStream(this.filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                document.Load(stream);
            }

            XmlElement elementPair = document.CreateElement("Pair");

            XmlElement elementProductKeyID = document.CreateElement("ProductKeyID");
            elementProductKeyID.InnerText = ProductKeyID.ToString();
            elementPair.AppendChild(elementProductKeyID);

            XmlElement elementSerialNumber = document.CreateElement("SerialNumber");
            elementSerialNumber.InnerText = SerialNumber;
            elementPair.AppendChild(elementSerialNumber);

            //XmlElement elementPairID = document.CreateElement("PairID");
            //elementPairID.InnerText = Guid.NewGuid().ToString();
            //elementPair.AppendChild(elementPairID);

            XmlElement elementCreationTime = document.CreateElement("CreationTime");
            elementCreationTime.InnerText = DateTime.Now.ToString();
            elementPair.AppendChild(elementCreationTime);

            XmlElement elementTransactionID = document.CreateElement("TransactionID");
            elementTransactionID.InnerText = TransactionID;
            elementPair.AppendChild(elementTransactionID);

            XmlAttribute attributePairID = document.CreateAttribute("ID");
            attributePairID.Value = Guid.NewGuid().ToString();
            elementPair.Attributes.Append(attributePairID);

            document.FirstChild.NextSibling.AppendChild(document.ImportNode(elementPair, true));

            using (stream = new FileStream(this.filePath, FileMode.Open, FileAccess.Write, FileShare.Write))
            {
                using (writer = XmlWriter.Create(stream, settings))
                {
                    document.Save(writer);
                }
            }

            return "1";
        }
    }

    public enum PersistencyMode 
    {
        RDBMSSQLServer = 0,
        FileSystemXML = 1,
        FileSystemJSON = 2
    }
}
