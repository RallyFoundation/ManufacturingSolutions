using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataIntegrator;

namespace UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //serializeAdapter();

            //deserializeAdpter();

            //serializeFSDDL();

            //deserializeFSDDL();

            //serializeFSDDL2();

            //deserializeFSDDL2();

            //reflectObject();

            //getData();

            //setData();

            //compareAndUpdateData();

            fsddlTest();
        }

        static void serializeAdapter() 
        {
            Adapter adpter = new Adapter();
            adpter.Source = new HTTPEndPoint();
            adpter.Source.Address = "https://api.microsoftoem.info";
            adpter.Source.Authentication = new Authentication();
            adpter.Source.Authentication.Type = AuthenticationType.X509Certificate;
            adpter.Source.Authentication.Identifier = @"D:\backup\Shared\Pilot\PilotCert.pfx";
            adpter.Source.Authentication.Password = "68pQakPj";
            adpter.Source.DataType = "OrderAcknowledgement";
            adpter.Source.Protocol = new Protocol();
            adpter.Source.Protocol.Component = ".NET Framework4.0";
            adpter.Source.Protocol.Name = "HTTP1.1";
            adpter.Source.Protocol.Vendor = "Microsoft";
            adpter.Source.Protocol.Version = "1.1";
            adpter.Source.Operations = new List<Operation>();
            adpter.Source.Operations.Add(new Operation() { Name = "GetOrderAcks", Method = OperationMethod.Retrieve });

            adpter.Destination = new RDBMSEndPoint();
            adpter.Destination.Address = @"Data Source=.\SQLExpress;Initial Catalog=OemKeyStore;Integrated Security=True";
            adpter.Destination.Authentication = new Authentication();
            adpter.Destination.Authentication.Type = AuthenticationType.NTLM;
            adpter.Destination.DataType = "ProductKeyInfo";
            adpter.Destination.Protocol = new Protocol();
            adpter.Destination.Protocol.Name = "ADO.NET";
            adpter.Destination.Protocol.Version = "4.0";
            adpter.Destination.Protocol.Vendor = "Microsoft";
            adpter.Destination.Protocol.Component = ".NET Framework Data Provider for SQL Server";
            adpter.Destination.Operations = new List<Operation>();
            adpter.Destination.Operations.Add(new Operation() { Name = "AddProductKeys", Method = OperationMethod.Create, Message = "Insert into dbo.ProductKeyInfo values()"});

            adpter.Transformer = new XSLTEndPoint();
            adpter.Transformer.Address = @"d:\dbtransform.xsl";

            string adpterXml = XMLUtility.XmlSerialize(adpter, new Type[] { typeof(Authentication), typeof(Operation), typeof(Protocol), typeof(AuthenticationType), typeof(OperationMethod), typeof(List<Operation>), typeof(List<Argument>), typeof(Argument), typeof(HTTPEndPoint), typeof(RDBMSEndPoint), typeof(XSLTEndPoint), typeof(LDAPEndPoint), typeof(FileSystemEndPoint), typeof(FTPEndPoint) });//, typeof(SOAPEndPoint), typeof(HTTPEndPoint), typeof(RDBMSEndPoint), typeof(XSLTEndPoint), typeof(LDAPEndPoint), typeof(FileSystemEndPoint), typeof(FTPEndPoint)

            Console.WriteLine(adpterXml);

            FileStream fileStream = new FileStream(@"D:\backup\AdpterTest.xml", FileMode.Create, FileAccess.Write, FileShare.Write);

            StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8);

            adpterXml = adpterXml.Insert(adpterXml.IndexOf("?>"), " encoding=\"utf-8\"");
            adpterXml = adpterXml.Substring(0, adpterXml.LastIndexOf(">") + 1);

            writer.Write(adpterXml);

            writer.Flush();
            fileStream.Flush();

            writer.Close();
            fileStream.Close();

            Console.WriteLine(adpterXml);

            Console.Read();
        }

        static void deserializeAdpter() 
        {
            string adpterXml = "";

            FileStream fileStream = new FileStream(@"D:\backup\AdpterTest.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader reader = new StreamReader(fileStream);

            adpterXml = reader.ReadToEnd();

            reader.Close();
            fileStream.Close();

            Adapter adpter = XMLUtility.XmlDeserialize(adpterXml, typeof(Adapter), new Type[] { typeof(Authentication), typeof(Operation), typeof(Protocol), typeof(AuthenticationType), typeof(OperationMethod), typeof(List<Operation>), typeof(List<Argument>), typeof(Argument), typeof(HTTPEndPoint), typeof(RDBMSEndPoint), typeof(XSLTEndPoint), typeof(LDAPEndPoint), typeof(FileSystemEndPoint), typeof(FTPEndPoint) }) as Adapter;

            Console.Write(adpter);

            Console.Write(adpter.Destination.Address);

            Console.Read();
        }

        static void serializeFSDDL() 
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(@"D:\backup\AdpterTest.xml");

            DataIntegrator.Descriptions.FileSystem.FSDDL.Directory dir = new DataIntegrator.Descriptions.FileSystem.FSDDL.Directory()
            {
                Name = "Root",
                Description = "Root directoy",
                Type = "Root",
                Title = "Root Directory",
                Files = new List<DataIntegrator.Descriptions.FileSystem.FSDDL.File>( new DataIntegrator.Descriptions.FileSystem.FSDDL.File[]
                {
                     new DataIntegrator.Descriptions.FileSystem.FSDDL.File()
                     {
                          Name = "File01",
                          Content = fileBytes,
                          Size = fileBytes.Length
                     }
                }),

                Directories  = new List<DataIntegrator.Descriptions.FileSystem.FSDDL.Directory>(  new DataIntegrator.Descriptions.FileSystem.FSDDL.Directory[]
                {
                    new DataIntegrator.Descriptions.FileSystem.FSDDL.Directory()
                    {
                        Name = "dir01",
                        Description = "dir01",
                        Title = "dir01",
                        Files = new List<DataIntegrator.Descriptions.FileSystem.FSDDL.File>( new DataIntegrator.Descriptions.FileSystem.FSDDL.File[]
                        {
                             new DataIntegrator.Descriptions.FileSystem.FSDDL.File()
                             {
                                  Name = "File02",
                                  Content = fileBytes,
                                  Size = fileBytes.Length
                             }
                        }),
                        Directories  = new List<DataIntegrator.Descriptions.FileSystem.FSDDL.Directory>(  new DataIntegrator.Descriptions.FileSystem.FSDDL.Directory[]
                        {
                            new DataIntegrator.Descriptions.FileSystem.FSDDL.Directory()
                            {
                                Name = "dir02",
                                Description = "dir02",
                                Title = "dir02",
                                Files = new List<DataIntegrator.Descriptions.FileSystem.FSDDL.File>( new DataIntegrator.Descriptions.FileSystem.FSDDL.File[]
                                {
                                     new DataIntegrator.Descriptions.FileSystem.FSDDL.File()
                                     {
                                          Name = "File03",
                                          Content = fileBytes,
                                          Size = fileBytes.Length
                                     }
                                })
                            }
                        })
                    }
                })
            };

            string dirXml = XMLUtility.XmlSerialize(dir, new Type[] {typeof(DataIntegrator.Descriptions.FileSystem.FSDDL.File) });

            Console.Write(dirXml);

            FileStream fileStream = new FileStream(@"D:\backup\FSDDLTest.xml", FileMode.Create, FileAccess.Write, FileShare.Write);

            StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8);

            dirXml = dirXml.Insert(dirXml.IndexOf("?>"), " encoding=\"utf-8\"");
            dirXml = dirXml.Substring(0, dirXml.LastIndexOf(">") + 1);

            writer.Write(dirXml);

            writer.Flush();
            fileStream.Flush();

            writer.Close();
            fileStream.Close();

            Console.Write(dirXml);

            Console.Read();
        }

        static void deserializeFSDDL()
        {
            string dirXml = "";

            FileStream fileStream = new FileStream(@"D:\backup\FSDDLTest.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader reader = new StreamReader(fileStream);

            dirXml = reader.ReadToEnd();

            reader.Close();
            fileStream.Close();

            DataIntegrator.Descriptions.FileSystem.FSDDL.Directory dir = XMLUtility.XmlDeserialize(dirXml, typeof(DataIntegrator.Descriptions.FileSystem.FSDDL.Directory), new Type[] { typeof(DataIntegrator.Descriptions.FileSystem.FSDDL.File)}) as DataIntegrator.Descriptions.FileSystem.FSDDL.Directory;

            Console.WriteLine(dir);

            Console.WriteLine(dir.Name);

            Console.WriteLine(dir.Files[0].Name);

            Console.WriteLine(Encoding.UTF8.GetString(dir.Files[0].Content));

            Console.Read();
        }

        static void serializeFSDDL2() 
        {
            DataIntegrator.Helpers.FileSystem.FileSystemHelper helper = new DataIntegrator.Helpers.FileSystem.FileSystemHelper();

            DataIntegrator.Descriptions.FileSystem.FSDDL.Directory dir = helper.ListFiles(@"D:\home\v-rawang\Documents\Quanta\Quanta 2nd round\", "*.xml");

            string dirXml = XMLUtility.XmlSerialize(dir, new Type[] { typeof(DataIntegrator.Descriptions.FileSystem.FSDDL.File) });

            Console.Write(dirXml);

            FileStream fileStream = new FileStream(@"D:\backup\FSDDLTest2.xml", FileMode.Create, FileAccess.Write, FileShare.Write);

            StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8);

            dirXml = dirXml.Insert(dirXml.IndexOf("?>"), " encoding=\"utf-8\"");
            dirXml = dirXml.Substring(0, dirXml.LastIndexOf(">") + 1);

            writer.Write(dirXml);

            writer.Flush();
            fileStream.Flush();

            writer.Close();
            fileStream.Close();

            Console.Write(dirXml);

            Console.Read();
        }

        static void deserializeFSDDL2() 
        {
            string dirXml = "";

            FileStream fileStream = new FileStream(@"D:\backup\FSDDLTest2.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader reader = new StreamReader(fileStream);

            dirXml = reader.ReadToEnd();

            reader.Close();
            fileStream.Close();

            DataIntegrator.Descriptions.FileSystem.FSDDL.Directory dir = XMLUtility.XmlDeserialize(dirXml, typeof(DataIntegrator.Descriptions.FileSystem.FSDDL.Directory), new Type[] { typeof(DataIntegrator.Descriptions.FileSystem.FSDDL.File) }) as DataIntegrator.Descriptions.FileSystem.FSDDL.Directory;

            Console.WriteLine(dir);

            Console.WriteLine(dir.Name);

            Console.WriteLine(dir.Directories[0].Directories[0].Files[0].Name);

            Console.WriteLine(Encoding.UTF8.GetString(dir.Directories[0].Directories[0].Files[0].Content));

            DataIntegrator.Helpers.FileSystem.FileSystemHelper helper = new DataIntegrator.Helpers.FileSystem.FileSystemHelper();

            System.IO.DirectoryInfo dirInfo = helper.WriteFiles(@"D:\bakcup\DITest", dir) as System.IO.DirectoryInfo;

            Console.WriteLine(dirInfo.FullName);

            Console.Read();
        }

        static void reflectObject() 
        {
            string connStr = @"Data Source=.\SQLExpress;Initial Catalog=OemKeyStore;Integrated Security=True";

            string typeName = "Oracle.DataAccess.Client.OracleBulkCopy";

            string assemblyName = "System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

            typeName = "System.Data.SqlClient.SqlBulkCopy";

            //assemblyName = "Oracle.DataAccess, Version=4.112.3.0, Culture=neutral, PublicKeyToken=89b483f429c47342";

            System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(assemblyName);

            Type type = assembly.GetType(typeName);

            object obj = System.Activator.CreateInstance(type, new object[]{ connStr});

            obj.GetType().GetProperty("DestinationTableName").SetValue(obj, "MyTestTable");

            string destTableName = obj.GetType().GetProperty("DestinationTableName").GetValue(obj).ToString();

            Console.Write(destTableName);

            Console.Read();
        }

        static void getData() 
        {
            DataIntegrator.Helpers.RDBMS.RDBMSHelper helper = new DataIntegrator.Helpers.RDBMS.RDBMSHelper();

            string providerName = "System.Data.SqlClient";

            string connStr = @"Data Source=.\SQLExpress;Initial Catalog=FactoryFloorKeyStore;Integrated Security=True";

            string commandText = "select [ProductKeyID],[ProductKey],[ProductKeyStateID],[OEMPartNumber],[ProductKeyState],[ActionCode],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate] from [dbo].[ProductKeyInfo]";// FOR XML EXPLICIT";

            //System.Xml.XmlDocument document = helper.Query(providerName, connStr, commandText, "text", OperationMethod.Retrieve, false) as System.Xml.XmlDocument;

            //System.Xml.XmlDocument document = new System.Xml.XmlDocument();

            //document.Load(reader);

            string xml = helper.Query(providerName, connStr, commandText, "text", OperationMethod.Retrieve).ToString(); //document.InnerXml;

            Console.WriteLine(xml);

            FileStream fileStream = new FileStream(@"D:\backup\RDBMSXmlTest.xml", FileMode.Create, FileAccess.Write, FileShare.Write);

            StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8);

            //xml = xml.Insert(xml.IndexOf("?>"), " encoding=\"utf-8\"");
            //xml = xml.Substring(0, xml.LastIndexOf(">") + 1);

            writer.Write(xml);

            writer.Flush();
            fileStream.Flush();

            writer.Close();
            fileStream.Close();

            Console.Read();
        }

        static void setData()
        {
            DataIntegrator.Helpers.RDBMS.RDBMSHelper helper = new DataIntegrator.Helpers.RDBMS.RDBMSHelper();

            string providerName = "System.Data.SqlClient";

            string connStr = @"Data Source=.\SQLExpress;Initial Catalog=FactoryFloorKeyStore;Integrated Security=True";

            string commandText = "select [ProductKeyID],[ProductKey],[ProductKeyStateID],[OEMPartNumber],[ProductKeyState],[ActionCode],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate] from [dbo].[ProductKeyInfo]";// FOR XML EXPLICIT";

            commandText = "select * from dbo.ProductKeyInfo";

            //commandText = "select [ProductKeyID],[ProductKey],[ProductKeyStateID],[OEMPartNumber],[ProductKeyState] from dbo.ProductKeyInfo";

            //System.Xml.XmlDocument document = helper.Query(providerName, connStr, commandText, "text", OperationMethod.Retrieve, false) as System.Xml.XmlDocument;

            //System.Xml.XmlDocument document = new System.Xml.XmlDocument();

            //document.Load(reader);

            string xml = helper.Query(providerName, connStr, commandText, "text", OperationMethod.Retrieve).ToString(); //document.InnerXml;

            //Console.WriteLine(xml);

            FileStream fileStream = new FileStream(@"D:\backup\RDBMSXmlTest2.xml", FileMode.Create, FileAccess.Write, FileShare.Write);

            StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8);

            //xml = xml.Insert(xml.IndexOf("?>"), " encoding=\"utf-8\"");
            //xml = xml.Substring(0, xml.LastIndexOf(">") + 1);

            writer.Write(xml);

            writer.Flush();
            fileStream.Flush();

            writer.Close();
            fileStream.Close();

            connStr = @"Data Source=.\SQLExpress;Initial Catalog=FactoryFloorKeyStore_2;Integrated Security=True";

            string typeName = "Oracle.DataAccess.Client.OracleBulkCopy";

            string assemblyName = "System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

            typeName = "System.Data.SqlClient.SqlBulkCopy";

            //assemblyName = "Oracle.DataAccess, Version=4.112.3.0, Culture=neutral, PublicKeyToken=89b483f429c47342";

            helper.BulkCopy(xml, "System.Data.SQLClient", connStr, "ProductKeyInfo", assemblyName, typeName);

            Console.Read();
        }

        static void compareAndUpdateData() 
        {
            string conStr = @"Data Source=.\SQLExpress;Initial Catalog=FactoryFloorKeyStore_2;Integrated Security=True";

            string selectCommandText = "select * from ProductKeyInfo";

            string dataSetXmlFile = @"D:\backup\RDBMSXmlTest2_2.xml";

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(conStr);

            System.Data.SqlClient.SqlCommand selectCmd = new System.Data.SqlClient.SqlCommand(selectCommandText);

            selectCmd.Connection = conn;

            System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(selectCmd);

            System.Data.SqlClient.SqlCommandBuilder builder = new System.Data.SqlClient.SqlCommandBuilder(adapter);

            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
            }

            System.Data.DataSet dataSet = new System.Data.DataSet();

            dataSet.ReadXml(dataSetXmlFile);

            dataSet.AcceptChanges();

            System.Data.DataSet dataSet2 = new System.Data.DataSet();

            adapter.Fill(dataSet2);

            //dataSet2.Merge(dataSet);

            //dataSet.Merge(dataSet2);

            //dataSet2 = dataSet.Clone();

            List<string> dpkids = new List<string>();

            foreach (System.Data.DataRow row in dataSet.Tables[0].Rows)
            {
                foreach (System.Data.DataRow row2 in dataSet2.Tables[0].Rows)
                {
                    if (row["ProductKeyID"].ToString() == row2["ProductKeyID"].ToString())
                    {
                        dpkids.Add(row["ProductKeyID"].ToString());
                    }
                }
            }

            int count = dataSet.Tables[0].Rows.Count;//dataSet2.Tables[0].Rows.Count;

            //for (int i = 0; i < count; i++)
            //{
            //    if (!dpkids.Contains(dataSet2.Tables[0].Rows[i]["ProductKeyID"].ToString()))
            //    {
            //        dataSet2.Tables[0].Rows[i].Delete();

            //        //dataSet2.Tables[0].Rows[i].AcceptChanges();

            //        Console.WriteLine(dataSet2.Tables[0].Rows[i].RowState);

            //        //i--;
            //        //count--;
            //    }
            //    else
            //    {
            //        dataSet2.Tables[0].Rows[i]["CreatedBy"] = "Rally";

            //        Console.WriteLine(dataSet2.Tables[0].Rows[i].RowState);
            //    }
            //}

            for (int i = 0; i < count; i++)
            {
                //if (!dpkids.Contains(dataSet.Tables[0].Rows[i]["ProductKeyID"].ToString()))
                //{
                //    dataSet.Tables[0].Rows[i].Delete();

                //    //dataSet2.Tables[0].Rows[i].AcceptChanges();

                //    Console.WriteLine(dataSet.Tables[0].Rows[i].RowState);

                //    //i--;
                //    //count--;
                //}
                //else
                //{
                //    dataSet.Tables[0].Rows[i]["CreatedBy"] = "Rally";

                //    //dataSet.Tables[0].Rows[i].SetModified();

                //    Console.WriteLine(dataSet.Tables[0].Rows[i].RowState);
                //}
            }

            //dataSet2.Tables[0].AcceptChanges();

            //dataSet2.AcceptChanges();

            //int result = adapter.Update(dataSet);

            Console.WriteLine(builder.GetDeleteCommand().CommandText);

            Console.WriteLine(builder.GetInsertCommand().CommandText);

            Console.WriteLine(builder.GetUpdateCommand().CommandText);

            //int result = adapter.Update(dataSet2);

            //int result = adapter.Update(dataSet);


            foreach (System.Data.DataColumn col in dataSet2.Tables[0].PrimaryKey)
            {
                Console.WriteLine(col.ColumnName);
            }

            if (conn.State != System.Data.ConnectionState.Closed)
            {
                conn.Close();
            }

            //Console.WriteLine(result);

            Console.Read();
        }

        static void compareData() 
        {
            System.Data.DataSet dataSet1 = new System.Data.DataSet();

            System.Data.DataSet dataSet2 = new System.Data.DataSet();

            for (int i = 0; i < dataSet1.Tables.Count; i++)
            {
                for (int j = 0; j < dataSet2.Tables.Count; j++)
                {
                    for (int k = 0; k < dataSet1.Tables[i].Rows.Count; k++)
                    {
                        for (int l = 0; l < dataSet2.Tables[j].Rows.Count; l++)
                        {
                            for (int m = 0; m < dataSet1.Tables[i].Columns.Count; m++)
                            {
                                if (dataSet1.Tables[i].Rows[k][m].ToString() != dataSet2.Tables[j].Rows[l][m].ToString())
                                {
                                    if (dataSet1.Tables[i].PrimaryKey.Contains(dataSet1.Tables[i].Columns[m]))
                                    {
                                        dataSet1.Tables[i].Rows.Add(dataSet2.Tables[j].Rows[l]);
                                    }
                                    else
                                    {
                                        dataSet1.Tables[i].Rows[k].SetModified();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        static void fsddlTest() 
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(@"D:\backup\AdpterTest.xml");

            DataIntegrator.Descriptions.FileSystem.FSDDL.Directory dir = new DataIntegrator.Descriptions.FileSystem.FSDDL.Directory()
            {
                Name = "Root",
                Description = "Root directoy",
                Type = "Root",
                Title = "Root Directory",
                Files = new List<DataIntegrator.Descriptions.FileSystem.FSDDL.File>(new DataIntegrator.Descriptions.FileSystem.FSDDL.File[]
                {
                     new DataIntegrator.Descriptions.FileSystem.FSDDL.File()
                     {
                          Name = "File01",
                          Content = fileBytes,
                          Size = fileBytes.Length
                     }
                }),

                Directories = new List<DataIntegrator.Descriptions.FileSystem.FSDDL.Directory>(new DataIntegrator.Descriptions.FileSystem.FSDDL.Directory[]
                {
                    new DataIntegrator.Descriptions.FileSystem.FSDDL.Directory()
                    {
                        Name = "dir01",
                        Description = "dir01",
                        Title = "dir01",
                        Files = new List<DataIntegrator.Descriptions.FileSystem.FSDDL.File>( new DataIntegrator.Descriptions.FileSystem.FSDDL.File[]
                        {
                             new DataIntegrator.Descriptions.FileSystem.FSDDL.File()
                             {
                                  Name = "File02",
                                  Content = fileBytes,
                                  Size = fileBytes.Length
                             }
                        }),
                        Directories  = new List<DataIntegrator.Descriptions.FileSystem.FSDDL.Directory>(  new DataIntegrator.Descriptions.FileSystem.FSDDL.Directory[]
                        {
                            new DataIntegrator.Descriptions.FileSystem.FSDDL.Directory()
                            {
                                Name = "dir02",
                                Description = "dir02",
                                Title = "dir02",
                                Files = new List<DataIntegrator.Descriptions.FileSystem.FSDDL.File>( new DataIntegrator.Descriptions.FileSystem.FSDDL.File[]
                                {
                                     new DataIntegrator.Descriptions.FileSystem.FSDDL.File()
                                     {
                                          Name = "File03",
                                          Content = fileBytes,
                                          Size = fileBytes.Length
                                     }
                                })
                            }
                        })
                    }
                })
            };

            dir.SetParents(true);

            string absolutePath = dir.GetAbsolutePath(false);

            Console.WriteLine(absolutePath);

            foreach (DataIntegrator.Descriptions.FileSystem.FSDDL.Directory directory in dir.Directories)
            {
                Console.WriteLine(directory.GetAbsolutePath(false));

                foreach (DataIntegrator.Descriptions.FileSystem.FSDDL.File file in directory.Files)
                {
                    Console.WriteLine(file.GetFullName(false));
                }

                foreach (DataIntegrator.Descriptions.FileSystem.FSDDL.Directory subDir  in directory.Directories)
                {
                    Console.WriteLine(subDir.GetAbsolutePath(false));

                    foreach (DataIntegrator.Descriptions.FileSystem.FSDDL.File file in subDir.Files)
                    {
                        Console.WriteLine(file.GetFullName(false));
                    }
                }
            }


            absolutePath = dir.GetAbsolutePath(true);

            Console.WriteLine(absolutePath);

            foreach (DataIntegrator.Descriptions.FileSystem.FSDDL.Directory directory in dir.Directories)
            {
                Console.WriteLine(directory.GetAbsolutePath(true));

                foreach (DataIntegrator.Descriptions.FileSystem.FSDDL.File file in directory.Files)
                {
                    Console.WriteLine(file.GetFullName(true));
                }

                foreach (DataIntegrator.Descriptions.FileSystem.FSDDL.Directory subDir in directory.Directories)
                {
                    Console.WriteLine(subDir.GetAbsolutePath(true));

                    foreach (DataIntegrator.Descriptions.FileSystem.FSDDL.File file in subDir.Files)
                    {
                        Console.WriteLine(file.GetFullName(true));
                    }
                }
            }

            Console.Read();
        }
    }
}