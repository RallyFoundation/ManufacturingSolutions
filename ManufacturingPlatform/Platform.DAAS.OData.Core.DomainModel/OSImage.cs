using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.DAAS.OData.Core.DomainModel
{
    public class OSImage
    {
        public string ImageGroupName { get; set; }
        public string ImageFilePath { get; set; }

        public string RawImageNameInFile { get; set; }

        public string NewImageName { get; set; }

        public string NewDescription { get; set; }

        public string NewFileName { get; set; }

        public bool EnableMulticastTransmission { get; set; }

        public string MulticastTransmissionName { get; set; }
    }
}
