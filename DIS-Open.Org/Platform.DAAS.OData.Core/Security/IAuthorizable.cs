using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.DAAS.OData.Core.Security
{
    public interface IAuthorizable
    {
        string DataType { get; set; }

        string Operation { get; set; }

        bool IsValidatingDataScope { get; set; }

        bool ShouldByPassSupperUser { get; set; }
    }
}
