using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIS.Data.DataContract;

namespace DIS.Business.Proxy.KeyProvider.Parameters
{
    /// <summary>
    /// SerialNumberParameter class is used to populate the Parameter relevant information
    /// </summary>
    class SerialNumberParameter : IParameter
    {
        public void Attach(KeySearchCriteria searchCriteria, object value)
        {
            searchCriteria.SerialNumber = value.ToString();
        }
    }
}
