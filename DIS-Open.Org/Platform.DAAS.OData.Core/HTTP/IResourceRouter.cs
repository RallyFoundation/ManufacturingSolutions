using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.DAAS.OData.Core.HTTP
{
    public interface IResourceRouter
    {
        object Post(string Uri, string Data, Authentication Authentication, Dictionary<string, string> Headers);

        object Get(string Uri, Authentication Authentication, Dictionary<string, string> Headers);

        void PostAsync(string Uri, byte[] Data, Authentication Authentication, Dictionary<string, string> Headers);

        void GetAsync(string Uri, Authentication Authentication, Dictionary<string, string> Headers);

    }

}