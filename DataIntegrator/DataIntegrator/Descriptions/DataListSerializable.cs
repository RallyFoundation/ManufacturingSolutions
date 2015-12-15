using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DataIntegrator.Descriptions.Data
{
    [Serializable]
    [XmlType(AnonymousType = false)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class DataObjects
    {
        private DataObject[] dataObject;

        [XmlElement("DataObject")]
        public DataObject[] DataObject
        {
            get
            {
                return this.dataObject;
            }
            set
            {
                this.dataObject = value;
            }
        }

        public IList<DataObject> GetDataObjects()
        {
            IList<DataObject> returnValue = null;

            if ((this.dataObject != null) && (this.dataObject.Length > 0))
            {
                returnValue = new List<DataObject>();

                for (int i = 0; i < this.dataObject.Length; i++)
                {
                    if (this.dataObject[i] != null)
                    {
                        if ((this.dataObject[i].MemberDataArray != null) && (this.dataObject[i].MemberDataArray.Length > 0))
                        {
                            returnValue.Add(dataObject[i]);
                        }
                    }
                }
            }

            return returnValue;
        }

        public IList<IDictionary<string, object>> GetDataObjectMembers()
        {
            IList<IDictionary<string, object>> returnValue = null;

            if ((this.dataObject != null) && (this.dataObject.Length > 0))
            {
                returnValue = new List<IDictionary<string, object>>();

                IDictionary<string, object> members = null;

                for (int i = 0; i < this.dataObject.Length; i++)
                {
                    if (this.dataObject[i] != null)
                    {
                        members = this.dataObject[i].GetMembers();

                        if ((members != null) && (members.Count > 0))
                        {
                            returnValue.Add(members);
                        }
                    }
                }
            }

            return returnValue;
        }
    }

    [Serializable]
    [XmlType(AnonymousType = false)]
    public class DataObject
    {
        private MemberData[] memberDataArray;

        [XmlArrayItem("MemberData", IsNullable = false)]
        public MemberData[] MemberDataArray
        {
            get
            {
                return this.memberDataArray;
            }
            set
            {
                this.memberDataArray = value;
            }
        }

        public IDictionary<string, object> GetMembers()
        {
            IDictionary<string, object> returnValue = null;

            if ((this.memberDataArray != null) && (this.memberDataArray.Length > 0))
            {
                returnValue = new Dictionary<string, object>();

                for (int i = 0; i < this.memberDataArray.Length; i++)
                {
                    if ((this.memberDataArray[i] != null) && (!String.IsNullOrEmpty(this.memberDataArray[i].Name)))//if ((this.memberDatas[i] != null) && (!String.IsNullOrEmpty(this.memberDatas[i].Name)) && (!String.IsNullOrEmpty(this.memberDatas[i].Value)))
                    {
                        if (!returnValue.ContainsKey(this.memberDataArray[i].Name))
                        {
                            returnValue.Add(this.memberDataArray[i].Name, this.memberDataArray[i].Value);
                        }
                    }
                }
            }

            return returnValue;
        }
    }

    [Serializable]
    [XmlType(AnonymousType = false)]
    public class MemberData
    {
        private string name;
        private string value;

        [XmlAttribute("Name")]
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

        [XmlAttribute("Value")]
        public string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
    }
}
