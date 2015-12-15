using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataIntegrator.Descriptions.FileSystem.FSDDL
{
    public class File 
    {
        public String ID { get; set; }

        [XmlAttribute]
        public String Name { get; set; }

        public String Type { get; set; }

        public String Title { get; set; }

        public String Description { get; set; }

        [XmlAttribute]
        public int Size { get; set; }

        public String Owner { get; set; }

        public String Role { get; set; }

        public String Link { get; set; }

        public byte[] Content { get; set; }

        [XmlIgnore]
        public Directory Parent { get; set; }

        public string GetFullName(bool IsUsingBackSlash)
        {
            string returnValue = "";

            string separator = IsUsingBackSlash ? @"\" : "/";

            returnValue = separator + this.Name;

            Directory parent = this.Parent;

            while (parent != null)
            {
                returnValue = separator + parent.Name + returnValue;

                parent = parent.Parent;
            }

            return returnValue;
        }
    }

    public class Directory
    {
        [XmlAttribute]
        public String Name { get; set; }

        [XmlAttribute]
        public String Type { get; set; }

        public String Title { get; set; }

        public String Description { get; set; }

        public String Owner { get; set; }

        public String Role { get; set; }

        //[XmlElement("File")]
        public List<File> Files { get; set; }

        //[XmlElement("Directory")]
        public List<Directory> Directories { get; set; }

        [XmlIgnore]
        public Directory Parent { get; set; }

        public void SetParents(bool IsSettingChildren) 
        {
            if ((this.Files != null) && (this.Files.Count > 0))
            {
                for (int i = 0; i < this.Files.Count; i++)
                {
                    if (this.Files[i] != null)
                    {
                        this.Files[i].Parent = this;
                    }
                }
            }

            if ((this.Directories != null) && this.Directories.Count > 0)
            {
                for (int i = 0; i < this.Directories.Count; i++)
                {
                    if (this.Directories[i] != null)
                    {
                        this.Directories[i].Parent = this;

                        if (IsSettingChildren)
                        {
                            this.Directories[i].SetParents(IsSettingChildren);
                        }
                    }
                }
            }
        }

        public string GetAbsolutePath(bool IsUsingBackSlash) 
        {
            string returnValue = "";

            string separator = IsUsingBackSlash ? @"\" : "/";

            Directory parent = this;

            while (parent != null)
            {
                returnValue = separator + parent.Name + returnValue;

                parent = parent.Parent;
            }

            return returnValue;
        }
    }
}
