using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataIntegrator.Descriptions.FileSystem.FSDDL;
using DataIntegrator.Helpers.Tracing;

namespace DataIntegrator.Helpers.FileSystem
{
    public class FileSystemHelper
    {
        public FileSystemHelper() 
        {
        }

        public FileSystemHelper(bool enableTracing, string traceSourceName) 
        {
            this.EnableTracing = enableTracing;
            this.TraceSourceName = traceSourceName;
        }

        public bool EnableTracing { get; set; }

        public string TraceSourceName { get; set; }

        public object GetFile(string filePath) 
        {
            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { filePath }, this.TraceSourceName);
            }

            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            byte[] fileBytes = new byte[fileStream.Length];

            fileStream.Read(fileBytes, 0, fileBytes.Length);

            fileStream.Close();

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { filePath, fileBytes.Length }, this.TraceSourceName);
            }

            return fileBytes;
        }

        public object GetFileContentByEncoding(string filePath, string encodingName) 
        {
            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { filePath, encodingName }, this.TraceSourceName);
            }

            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            Encoding fileEncoding = String.IsNullOrEmpty(encodingName) ? Encoding.Default : Encoding.GetEncoding(encodingName);

            StreamReader reader = new StreamReader(fileStream, fileEncoding);

            string content = reader.ReadToEnd();

            reader.Close();

            fileStream.Close();

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { filePath, encodingName, fileEncoding.BodyName, content }, this.TraceSourceName);
            }

            return content;
        }

        public void WriteFileContentByEncoding(string filePath, string fileConent, string encodingName) 
        {
            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { filePath, fileConent, encodingName }, this.TraceSourceName);
            }

            FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write);

            Encoding fileEncoding = String.IsNullOrEmpty(encodingName) ? Encoding.Default : Encoding.GetEncoding(encodingName);

            StreamWriter writer = new StreamWriter(fileStream, fileEncoding);

            writer.Write(fileConent);

            writer.Flush();

            fileStream.Flush();

            writer.Close();

            fileStream.Close();

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { filePath, fileEncoding.BodyName }, this.TraceSourceName);
            }
        }

        public void WriteFileByBase64(string filePath, string fileBytesBase64String) 
        {
            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { filePath, fileBytesBase64String }, this.TraceSourceName);
            }

            byte[] fileBytes = Convert.FromBase64String(fileBytesBase64String);

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { filePath, fileBytes.Length }, this.TraceSourceName);
            }

            this.WriteFile(filePath, fileBytes);
        }

        public void WriteFile(string filePath, byte[] fileBytes)
        {
            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { filePath, fileBytes.Length }, this.TraceSourceName);
            }

            FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write);

            fileStream.Write(fileBytes, 0, fileBytes.Length);

            fileStream.Flush();

            fileStream.Close();
        }

        public DataIntegrator.Descriptions.FileSystem.FSDDL.Directory ListFiles(string root, string searchPattern) 
        {
            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { root, searchPattern }, this.TraceSourceName);
            }

            DataIntegrator.Descriptions.FileSystem.FSDDL.Directory directory = null;

            string[] fileNames = System.IO.Directory.GetFiles(root, searchPattern, SearchOption.TopDirectoryOnly);

            string[] directoryNames = System.IO.Directory.GetDirectories(root, "*", SearchOption.TopDirectoryOnly);

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { fileNames, directoryNames }, this.TraceSourceName);
            }

            directory = new Descriptions.FileSystem.FSDDL.Directory();

            System.IO.DirectoryInfo directoryInfo = new DirectoryInfo(root);

            directory.Name = directoryInfo.Name;

            if ((fileNames != null) && (fileNames.Length > 0))
            {
                directory.Files = new List<Descriptions.FileSystem.FSDDL.File>();

                foreach (string fileName in fileNames)
                {
                    byte[] fileBytes = new byte[1024];

                    FileInfo fileInfo = new FileInfo(fileName);

                    using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        fileStream.Read(fileBytes, 0, ((int)(fileStream.Length)));

                        directory.Files.Add(new Descriptions.FileSystem.FSDDL.File() {Name = fileInfo.Name, Content = fileBytes, Size = ((int)(fileInfo.Length))});
                    }
                }
            }

            if ((directoryNames != null) && (directoryNames.Length > 0))
            {
                directory.Directories = new List<Descriptions.FileSystem.FSDDL.Directory>();

                foreach (string directoryName in directoryNames)
                {
                    directory.Directories.Add(this.ListFiles(directoryName, searchPattern));
                }
            }

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { directory }, this.TraceSourceName);
            }
           
            return directory;
        }

        public object WriteFilesByFSDDL(string root, string fsddl, string encodingName) 
        {
            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { root, fsddl, encodingName }, this.TraceSourceName);
            }

            DataIntegrator.Descriptions.FileSystem.FSDDL.Directory directory = Utility.XmlDeserialize(fsddl, typeof(DataIntegrator.Descriptions.FileSystem.FSDDL.Directory), new Type[] { typeof(DataIntegrator.Descriptions.FileSystem.FSDDL.File) }, encodingName) as DataIntegrator.Descriptions.FileSystem.FSDDL.Directory;

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { directory }, this.TraceSourceName);
            }

            return this.WriteFiles(root, directory);
        }

        public object WriteFiles(string root, DataIntegrator.Descriptions.FileSystem.FSDDL.Directory directory) 
        {
            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { root, directory }, this.TraceSourceName);
            }

            System.IO.DirectoryInfo directoryInfo = null;

            if (!System.IO.Directory.Exists(root))
            {
                directoryInfo = System.IO.Directory.CreateDirectory(root);
            }
            else
            {
                directoryInfo = new DirectoryInfo(root);
            }

            directoryInfo = directoryInfo.CreateSubdirectory(directory.Name);

            if ((directory.Files != null) && (directory.Files.Count > 0))
            {
                foreach (DataIntegrator.Descriptions.FileSystem.FSDDL.File file in directory.Files)
                {
                    string fileAbsolutePath = directoryInfo.FullName + "\\" + file.Name;

                    using (System.IO.FileStream fileStream = new FileStream(fileAbsolutePath, FileMode.Create, FileAccess.Write, FileShare.Write))
                    {
                        fileStream.Write(file.Content, 0, file.Content.Length);
                        fileStream.Flush();
                    }
                }
            }

            if ((directory.Directories != null) && (directory.Directories.Count > 0))
            {
                foreach (DataIntegrator.Descriptions.FileSystem.FSDDL.Directory dir in directory.Directories)
                {
                   this.WriteFiles(directoryInfo.FullName, dir) ;
                }
                
            }

            return directoryInfo;
        }
    }
}
