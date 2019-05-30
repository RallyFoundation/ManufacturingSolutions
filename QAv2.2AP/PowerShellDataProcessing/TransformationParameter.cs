using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerShellDataProcessing
{
    public class TransformationParameter
    {
        public string XmlPath { get; set; }

        public string XsltPath { get; set; }

        public IDictionary<string, object> XsltArguments { get; set; }

        public IDictionary<string, object> XsltExtensionObjects { get; set; }
    }
}
