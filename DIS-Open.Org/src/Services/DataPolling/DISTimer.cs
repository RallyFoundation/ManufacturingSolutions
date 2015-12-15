using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using DIS.Business.Proxy;

namespace DIS.Services.DataPolling
{
    public class DISTimer : Timer
    {
        public string CustomerID { get; set; }

        public string ConfigurationID { get; set; }

        public string DBConnectionString { get; set; }

        public IUserProxy UserProxy 
        {
            get
            {
                return new UserProxy(this.DBConnectionString);
            }
        }

        public IConfigProxy ConfigProxy
        {
            get 
            { 
                return new ConfigProxy(this.UserProxy.GetFirstManager(), this.DBConnectionString, this.ConfigurationID, this.CustomerID);
            }
        }

        public IHeadQuarterProxy HeadQuaterProxy
        {
            get 
            {
                return new HeadQuarterProxy(this.DBConnectionString);
            }
        }
    }
}
