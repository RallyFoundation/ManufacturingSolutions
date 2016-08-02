using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.DAAS.OData.Core.Security
{
    public interface IAuthManager
    {
       bool Authenticate(string UserName, string Password);
       bool CheckAccessToken(string TokenPrefix, string TokenValue, object[] Arguments);
    }
}
