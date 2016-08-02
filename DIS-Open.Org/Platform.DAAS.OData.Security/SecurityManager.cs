using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
//using AspNet.Identity.MySQL;
using Microsoft.AspNet.Identity.EntityFramework;
using Platform.DAAS.OData.Core.Security;
using System.Security.Principal;

namespace Platform.DAAS.OData.Security
{
    public class SecurityManager : ISecurityManager
    {
        public object AddActorToRole(string ActorID, string Role)
        {
            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new Microsoft.AspNet.Identity.EntityFramework.IdentityDbContext(ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            var result = userManager.AddToRole(ActorID, Role);

            if (result.Succeeded)
            {
                return Role;
            }
            else
            {
               return  result.Errors.ToArray();
            }
        }

        public object AddRoles(IDictionary<string, string> Roles, IDictionary<string, string> RoleDescriptions)
        {
            List<object> results = new List<object>();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext(ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            foreach (var roleID in Roles.Keys)
            {
                var result = roleManager.Create(new IdentityRole()
                {
                    Id = roleID,
                    Name = Roles[roleID],
                    //Description = (RoleDescriptions !=null && RoleDescriptions.ContainsKey(roleID)) ? RoleDescriptions[roleID] : null
                });

                if (result.Succeeded)
                {
                    results.Add(roleID);
                }
                else
                {
                    results.Add(new Dictionary<string, object>() { { roleID, result.Errors.ToArray() } });
                }
            }

            return results;
        }

        public object[] GetAuthorizedObjects(string Actor, string DataTypeName, Func<string, object, IDictionary<string, string>, IDictionary<string, IList<object>>,object[]> ComputingFunction)
        {
            IDictionary<string, IList<object>> dataScopeValues = new Dictionary<string, List<object>>() as IDictionary<string, IList<object>>;
            IDictionary<string, string> dataScopeTypes = new Dictionary<string, string>();
            string dataIndetifier = null;

            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());

            var identity = userManager.FindByName(Actor);

            if (identity == null)
            {
                return null;
            }

            var roles = userManager.GetRoles(identity.Id);

            if (roles == null)
            {
                return null;
            }

            using (AuthEntityModelContainer context = new AuthEntityModelContainer())
            {
                var roleDataScopes = context.RoleDataScopes.Where(s => s.DataScope.DataType.ToLower() == DataTypeName.ToLower()).ToArray();

                roleDataScopes = roleDataScopes.Where(s => roles.Contains(s.RoleId)).ToArray();

                foreach (var roleDataScope in roleDataScopes)
                {
                    if (String.IsNullOrEmpty(dataIndetifier))
                    {
                        dataIndetifier = roleDataScope.DataScope.DataIdentifier;
                    }

                    if (!dataScopeTypes.ContainsKey(roleDataScope.DataScope.ScopeName))
                    {
                        dataScopeTypes.Add(roleDataScope.DataScope.ScopeName, roleDataScope.DataScope.ScopeType);
                    }

                    if (!dataScopeValues.ContainsKey(roleDataScope.DataScope.ScopeName))
                    {
                        dataScopeValues.Add(roleDataScope.DataScope.ScopeName, new List<object>());
                    }

                    dataScopeValues[roleDataScope.DataScope.ScopeName].Add(roleDataScope.ScopeValue);
                }
            }

            if (ComputingFunction != null)
            {
               return ComputingFunction(DataTypeName, dataIndetifier, dataScopeTypes, dataScopeValues);
            }

            return null;
        }

        public string[] GetDataTypeOperations(string DataTypeName)
        {
            List<string> opIds = null;

            using (AuthEntityModelContainer context = new AuthEntityModelContainer())
            {
                var ops = context.Operations.Where(o => true);

                if (!String.IsNullOrEmpty(DataTypeName))
                {
                    ops = ops.Where(o => o.DataType.ToLower() == DataTypeName.ToLower());
                }

                if (ops != null)
                {
                    var opArray = ops.ToArray();

                    opIds = new List<string>();

                    foreach (var op in ops)
                    {
                        opIds.Add(op.Id.ToLower());
                    }
                }
            }

            return opIds != null ? opIds.ToArray() : null;
        }

        public string[] GetDataScopes(string DataTypeName)
        {
            List<string> scopeIds = null;

            using (AuthEntityModelContainer context = new AuthEntityModelContainer())
            {
                var scopes = context.DataScopes.Where(s => true);

                if (!String.IsNullOrEmpty(DataTypeName))
                {
                    scopes = scopes.Where(o => o.DataType.ToLower() == DataTypeName.ToLower());
                }

                if (scopes != null)
                {
                    var scopeArray = scopes.ToArray();

                    scopeIds = new List<string>();

                    foreach (var scope in scopes)
                    {
                        scopeIds.Add(scope.Id.ToLower());
                    }
                }
            }

            return scopeIds != null ? scopeIds.ToArray() : null;
        }

        public object[] GetActorDataScopes(string Actor, string DataTypeName)
        {
            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());

            var identity = userManager.FindByName(Actor);

            if (identity == null)
            {
                return null;
            }

            var roles = userManager.GetRoles(identity.Id);

            if (roles == null)
            {
                return null;
            }

            IDictionary<string, IList<object>> dataScopeValues = new Dictionary<string, List<object>>() as IDictionary<string, IList<object>>;
            IDictionary<string, string> dataScopeTypes = new Dictionary<string, string>();
            string dataIndetifier = null;

            using (AuthEntityModelContainer context = new AuthEntityModelContainer())
            {
                var roleDataScopes = context.RoleDataScopes.Where(s => s.DataScope.DataType.ToLower() == DataTypeName.ToLower()).ToArray();

                roleDataScopes = roleDataScopes.Where(s => roles.Contains(s.RoleId)).ToArray();

                foreach (var roleDataScope in roleDataScopes)
                {
                    if (String.IsNullOrEmpty(dataIndetifier))
                    {
                        dataIndetifier = roleDataScope.DataScope.DataIdentifier;
                    }

                    if (!dataScopeTypes.ContainsKey(roleDataScope.DataScope.ScopeName))
                    {
                        dataScopeTypes.Add(roleDataScope.DataScope.ScopeName, roleDataScope.DataScope.ScopeType);
                    }

                    if (!dataScopeValues.ContainsKey(roleDataScope.DataScope.ScopeName))
                    {
                        dataScopeValues.Add(roleDataScope.DataScope.ScopeName, new List<object>());
                    }

                    dataScopeValues[roleDataScope.DataScope.ScopeName].Add(roleDataScope.ScopeValue);
                }
            }

            if (!String.IsNullOrEmpty(dataIndetifier) && dataScopeTypes.Count > 0 && dataScopeValues.Count > 0)
            {
                return new object[] {DataTypeName, dataIndetifier, dataScopeTypes, dataScopeValues };
            }

            return null;
        }

        public bool IsAuthorized(string Actor, string OperationID)
        {
            bool result = false;

            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new IdentityDbContext(ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            var identity = userManager.FindByName(Actor);

            if (identity == null)
            {
                return false;
            }

            var roleNames = userManager.GetRoles(identity.Id);

            if (roleNames == null)
            {
                return false;
            }

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext(ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            var roleIDs = new List<string>();

            var role = new IdentityRole();

            foreach (var roleName in roleNames)
            {
                role = roleManager.FindByName(roleName);

                if (role != null)
                {
                    roleIDs.Add(role.Id);
                } 
            }

            if (roleIDs.Count <= 0)
            {
                return false;
            }

            using (AuthEntityModelContainer context = new AuthEntityModelContainer())
            {
                var roleOps = context.RoleOperations.Where(o => o.OperationId.ToLower() == OperationID.ToLower() && roleIDs.Contains(o.RoleId));

                result = (roleOps != null) && (roleOps.ToArray().Length > 0);
            }

            return result;
        }

        public bool IsAuthorized(string Actor, string ObjectID, string OperationID)
        {
            bool result = false;

            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new IdentityDbContext(ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            var identity = userManager.FindByName(Actor);

            if (identity == null)
            {
                return false;
            }

            //var roleNames = userManager.GetRoles(identity.Id);

            //if (roleNames == null)
            //{
            //    return false;
            //}

            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new MySQLDatabase(ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            //var roleIDs = new List<string>();

            //var role = new IdentityRole();

            //foreach (var roleName in roleNames)
            //{
            //    role = roleManager.FindByName(roleName);

            //    if (role != null)
            //    {
            //        roleIDs.Add(role.Id);
            //    }
            //}

            //if (roleIDs.Count <= 0)
            //{
            //    return false;
            //}

            using (AuthEntityModelContainer context = new AuthEntityModelContainer())
            {
                //var roleOps = context.RoleOperations.Where(o => o.OperationId.ToLower() == OperationID.ToLower() && roleIDs.Contains(o.RoleId));

                //result = (roleOps != null) && (roleOps.ToArray().Length > 0);

                var authItems = context.ObjectOperationAuthItems.Where(a => (a.ActorId.ToLower() == identity.Id.ToLower()) && (a.ObjectId.ToLower() == ObjectID.ToLower()) && (a.OperationId.ToLower() == OperationID.ToLower()));

                result = (authItems != null) && (authItems.ToArray().Length > 0);
            }

            return result;
        }

        public object RegisterDataScope(string DataTypeName, string DataScopeID, string DataScopeName, string DataScopeType, string DataIndentifier)
        {
            string result = "";

            using (AuthEntityModelContainer context = new AuthEntityModelContainer())
            {
                var ds = context.DataScopes.Add(new DataScope()
                {
                    Id = DataScopeID.ToLower(),
                    ScopeName = DataScopeName,
                    ScopeType = DataScopeType,
                    DataType = DataTypeName,
                    DataIdentifier = DataIndentifier
                });

                context.SaveChanges();

                result = ds.Id;
            }

            return result;
        }

        public object RegisterOperation(string DataTypeName, string OperationID, string OperationName)
        {
            string result = "";

            using (AuthEntityModelContainer context = new AuthEntityModelContainer())
            {
                var op = context.Operations.Add(new Operation()
                {
                     Id = OperationID.ToLower(),
                     Name = OperationName,
                     DataType = DataTypeName
                });

                context.SaveChanges();

                result = op.Id;
            }

            return result;
        }

        public object SetActorObjectOperation(string ActorID, string ObjectID, string OperationID)
        {
            string result = "";

            using (AuthEntityModelContainer context = new AuthEntityModelContainer())
            {
                var op = context.Operations.FirstOrDefault(o => o.Id == OperationID);

                if (op != null)
                {
                   var authItem = context.ObjectOperationAuthItems.Add(new ObjectOperationAuthItem()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ActorId = ActorID,
                        ObjectId = ObjectID,
                        OperationId = OperationID
                    });

                    context.SaveChanges();

                    result = authItem.Id;
                }
            }

            return result;
        }

        public object SetActorRole(string ActorID, string Role)
        {
            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new IdentityDbContext(ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            var result = userManager.RemoveFromRole(ActorID, Role);

            if (result.Succeeded)
            {
                return Role;
            }
            else
            {
                return result.Errors.ToArray();
            }
        }

        public object SetRoleDataScope(string RoleID, string DataScopeID, string DataScopeValue)
        {
            string result = "";

            using (AuthEntityModelContainer context = new AuthEntityModelContainer())
            {
                var dataScope = context.DataScopes.FirstOrDefault(s => s.Id == DataScopeID);

                if (dataScope != null)
                {
                    var roleDataScope = context.RoleDataScopes.Add(new RoleDataScope()
                    {
                         Id = Guid.NewGuid().ToString(),
                         DataScopeId = DataScopeID,
                         ScopeValue = DataScopeValue,
                         RoleId = RoleID
                    });

                    context.SaveChanges();

                    result = roleDataScope.Id;
                }
            }

            return result;
        }

        public object SetRoleOperation(string RoleID, string OperationID)
        {
            string result = "";

            using (AuthEntityModelContainer context = new AuthEntityModelContainer())
            {
                var op = context.Operations.FirstOrDefault(o => o.Id == OperationID);

                if (op != null)
                {
                    var roleOps = context.RoleOperations.Add(new RoleOperation()
                    {
                         Id = Guid.NewGuid().ToString(),
                         OperationId = OperationID,
                         RoleId = RoleID
                    });

                    context.SaveChanges();

                    result = roleOps.Id;
                }
            }

            return result;
        }

        public object DeleteOperation(string OperationID)
        {
            List<object> results = new List<object>();

            using (AuthEntityModelContainer context = new AuthEntityModelContainer())
            {
                var obsoleteRoleOps = context.RoleOperations.Where(ro => ro.OperationId.ToLower() == OperationID.ToLower());

                if (obsoleteRoleOps != null)
                {
                   results.Add(context.RoleOperations.RemoveRange(obsoleteRoleOps.ToArray()));
                }

                var obsoleteObjectAuthItems = context.ObjectOperationAuthItems.Where(oo => oo.OperationId.ToLower() == OperationID.ToLower());

                if (obsoleteObjectAuthItems != null)
                {
                    results.Add(context.ObjectOperationAuthItems.RemoveRange(obsoleteObjectAuthItems.ToArray()));
                }

                var obsoleteOps = context.Operations.Where(o => o.Id.ToLower() == OperationID.ToLower());

                if (obsoleteOps != null)
                {
                    results.Add(context.Operations.RemoveRange(obsoleteOps.ToArray()));
                }

                context.SaveChanges();
            }

            return results;
        }

        public object DeleteDataScope(string DataScopeID)
        {
            List<object> results = new List<object>();

            using (AuthEntityModelContainer context = new AuthEntityModelContainer())
            {
                var obsoleteRoleDataScopes = context.RoleDataScopes.Where(rd => rd.DataScopeId.ToLower() == DataScopeID.ToLower());

                if (obsoleteRoleDataScopes != null)
                {
                    results.Add(context.RoleDataScopes.RemoveRange(obsoleteRoleDataScopes.ToArray()));
                }

                var obsoleteDataScopes = context.DataScopes.Where(ds => ds.Id.ToLower() == DataScopeID.ToLower());

                if (obsoleteDataScopes != null)
                {
                    results.Add(context.DataScopes.RemoveRange(obsoleteDataScopes.ToArray()));
                }

                context.SaveChanges();
            }

            return results;
        }

        public object DeleteRole(string RoleID)
        {
            List<object> results = new List<object>();

            using (AuthEntityModelContainer context = new AuthEntityModelContainer())
            {
                var obsoleteRoleOps = context.RoleOperations.Where(ro => ro.RoleId.ToLower() == RoleID.ToLower());

                if (obsoleteRoleOps != null)
                {
                    results.Add(context.RoleOperations.RemoveRange(obsoleteRoleOps.ToArray()));
                }

                var obsoleteRoleDataScopes = context.RoleDataScopes.Where(rd => rd.RoleId.ToLower() == RoleID.ToLower());

                if (obsoleteRoleDataScopes != null)
                {
                    results.Add(context.RoleDataScopes.RemoveRange(obsoleteRoleDataScopes.ToArray()));
                }

                context.SaveChanges();
            }

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext(ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            var obsoleteRoles = roleManager.Roles.Where(r => r.Id.ToLower() == RoleID.ToLower());

            if (obsoleteRoles != null)
            {
                var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new IdentityDbContext(ModuleConfiguration.DefaultIdentityStoreConnectionName)));

                var users = userManager.Users;

                foreach (var role in obsoleteRoles.ToArray())
                {
                    foreach (var user in users)
                    {
                       results.Add(userManager.RemoveFromRole(user.Id, role.Name));
                    }

                    results.Add(roleManager.Delete(role));
                }
            }

            return results;
        }

        public object DeleteActor(string ActorID)
        {
            List<object> results = new List<object>();

            using (AuthEntityModelContainer context = new AuthEntityModelContainer())
            {
                var obsoleteObjectAuthItems = context.ObjectOperationAuthItems.Where(oo => oo.ActorId.ToLower() == ActorID.ToLower());

                if (obsoleteObjectAuthItems != null)
                {
                    results.Add(context.ObjectOperationAuthItems.RemoveRange(obsoleteObjectAuthItems.ToArray()));
                }

                context.SaveChanges();
            }

            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new IdentityDbContext(ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            var obsoleteUsers = userManager.Users.Where(u => u.Id.ToLower() == ActorID.ToLower());

            if (obsoleteUsers != null)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext(ModuleConfiguration.DefaultIdentityStoreConnectionName)));

                var obsoleteRoles = roleManager.Roles;

                if (obsoleteRoles != null)
                {
                    foreach (var role in obsoleteRoles.ToArray())
                    {
                        results.Add(userManager.RemoveFromRole(ActorID, role.Name));
                    }
                }

                foreach (var user in obsoleteUsers.ToArray())
                {
                    results.Add(userManager.Delete(user));
                }
            }

            return results;
        }

        public bool IsSupperUser(string ActorID)
        {
            string supperUserId = ModuleConfiguration.DefaultSystemSuperUserID;

            return (ActorID.ToLower() == supperUserId.ToLower());
        }

        public bool IsSupperUser(IIdentity Identity)
        {
            if (Identity.IsAuthenticated)
            {
                var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new IdentityDbContext(ModuleConfiguration.DefaultIdentityStoreConnectionName)));

                var user = userManager.FindByName(Identity.Name);

                if (user != null)
                {
                    string supperUserId = ModuleConfiguration.DefaultSystemSuperUserID;
                    return (user.Id.ToLower() == supperUserId.ToLower());
                }
            }      

            return false;
        }
    }
}
