using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.DAAS.OData.Core.Logging
{
    public interface IExHandler
    {
        void HandleException(Exception Ex);

        void HandleException(Exception Ex, string Policy);
    }
}
