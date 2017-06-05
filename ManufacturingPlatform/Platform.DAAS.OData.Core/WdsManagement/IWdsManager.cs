using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.DAAS.OData.Core.DomainModel;

namespace Platform.DAAS.OData.Core.WdsManagement
{
    public interface IWdsManager
    {
        object AddInstallImageGroup(string GroupName);

        object ImportBootImage(OSImage BootImage);

        object ImportInstallImage(OSImage InstallImage);

        object GetRawImageContent(string ImageFilePath);

        string GetWdsImageNamespace(OSImage Image);

        void Initialize();

        void Cleanup();
    }
}
