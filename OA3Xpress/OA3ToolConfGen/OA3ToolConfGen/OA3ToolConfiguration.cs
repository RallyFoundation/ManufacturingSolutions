using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DISAdapter
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName="OA3")]
    public class OA3ToolConfiguration
    {
        public OA3ServerBased ServerBased { get; set; }
        public OA3OutputData OutputData { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class OA3OutputData
    {

        private string assembledBinaryFileField;

        private string reportedXMLFileField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string AssembledBinaryFile
        {
            get
            {
                return this.assembledBinaryFileField;
            }
            set
            {
                this.assembledBinaryFileField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ReportedXMLFile
        {
            get
            {
                return this.reportedXMLFileField;
            }
            set
            {
                this.reportedXMLFileField = value;
            }
        }
    }
}
