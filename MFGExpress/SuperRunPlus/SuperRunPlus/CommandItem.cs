using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SuperRunPlus
{
    public class CommandItem
    {
        [XmlAttribute]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Command { get; set; }

        public string Arguments { get; set; }
    }

    [XmlRoot("CommandItems")]
    public class CommandItems : List<CommandItem> { }
}
