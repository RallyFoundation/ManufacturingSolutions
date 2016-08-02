using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;

namespace Platform.DAAS.OData.Core.Security
{
    public interface ISecurityManager
    {
        bool IsSupperUser(string ActorID);

        bool IsSupperUser(IIdentity Identity);

        bool IsAuthorized(string Actor, string OperationID);

        bool IsAuthorized(string Actor, string ObjectID, string OperationID);

        object[] GetAuthorizedObjects(string Actor, string DataTypeName, Func<string, object, IDictionary<string, string>, IDictionary<string, IList<object>>, object[]> ComputingFunction);

        object RegisterOperation(string DataTypeName, string OperationID, string OperationName);

        object RegisterDataScope(string DataTypeName, string DataScopeID, string DataScopeName, string DataScopeType, string DataIdentifier);

        object SetRoleOperation(string RoleID, string OperationID);

        object SetRoleDataScope(string RoleID, string DataScopeID, string DataScopeValue);

        object SetActorObjectOperation(string ActorID, string ObjectID, string OperationID);

        object AddRoles(IDictionary<string, string> Roles, IDictionary<string, string> RoleDescriptions);

        object AddActorToRole(string ActorID, string Role);

        object SetActorRole(string ActorID, string Role);

        string[] GetDataTypeOperations(string DataTypeName);

        string[] GetDataScopes(string DataTypeName);

        object[] GetActorDataScopes(string Actor, string DataTypeName);

        object DeleteOperation(string OperationID);

        object DeleteDataScope(string DataScopeID);

        object DeleteRole(string RoleID);

        object DeleteActor(string ActorID);
    }
}
