using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.DAAS.OData.Core.Security
{
    public interface IRoleManager
    {
        IDictionary<string, string[]> GetAllUsersInRoles();

        string[] SetUserRole(string userName, string roleName);
    }
}
