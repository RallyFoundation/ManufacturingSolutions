using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DISAdapter
{
    [Serializable]
    public class OA3ServerBasedOptions
    {
        public string HardwareHashVersion { get; set; }
        public int HardwareHashPadding { get; set;}
    }
}
