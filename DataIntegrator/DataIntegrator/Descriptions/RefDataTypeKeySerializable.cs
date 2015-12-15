using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DataIntegrator.Descriptions.Data
{
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class RefDataTypes
    {
        private RefDataType[] items;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RefDataType", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public RefDataType[] Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        public IDictionary<string, IDictionary<string, string>> GetRefDataTypes() 
        {
            IDictionary<string, IDictionary<string, string>> returnValue = null;

            if ((this.items != null) && (this.items.Length > 0))
            {
                returnValue = new SortedDictionary<string, IDictionary<string, string>>();

                string key = String.Empty;
                IDictionary<string, string> refMemberDictionary = null;

                for (int i = 0; i < this.items.Length; i++)
                {
                    if (this.items[i] != null)
                    {
                        key = this.items[i].ID;
                        refMemberDictionary = this.items[i].GetRefMemberDictionary();

                        if ((!String.IsNullOrEmpty(key)) && (refMemberDictionary != null))
                        {
                            if (!returnValue.ContainsKey(key))
                            {
                                returnValue.Add(key, refMemberDictionary);
                            }
                        }
                    }
                }
            }

            return returnValue;
        }
    }

    [System.SerializableAttribute()]
    public class RefDataType
    {
        private RefMember[] refMember;

        private string id;

        private string name;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RefMember", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public RefMember[] RefMember
        {
            get
            {
                return this.refMember;
            }
            set
            {
                this.refMember = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ID
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public IDictionary<string, string> GetRefMemberDictionary() 
        {
            IDictionary<string, string> returnValue = null;

            if ((this.refMember != null) && (this.refMember.Length > 0))
            {
                returnValue = new SortedDictionary<string, string>();

                string key = String.Empty;

                for (int i = 0; i < this.refMember.Length; i++)
                {
                    if (this.refMember[i] != null)
                    {
                        key = refMember[i].Type + "." + this.refMember[i].Name;

                        if (!returnValue.ContainsKey(key))
                        {
                            if (!String.IsNullOrEmpty(this.refMember[i].ReferencedMemberName))
                            {
                                returnValue.Add(key, this.refMember[i].ReferencedMemberName);
                            }
                            else
                            {
                                returnValue.Add(key, this.refMember[i].Name);
                            }
                        }
                    }
                }
            }

            return returnValue;
        }
    }

    [System.SerializableAttribute()]
    public class RefMember
    {
        private string name;

        private string type;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        [System.Xml.Serialization.XmlTextAttribute()]
        public string ReferencedMemberName { get; set; }
    }

}
