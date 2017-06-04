using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNet.Identity;
//using AspNet.Identity.MySQL;
using Microsoft.AspNet.Identity.EntityFramework;
using Platform.DAAS.OData.Utility;
using Platform.DAAS.OData.Core.Security;
using Platform.DAAS.OData.Core.Logging;

namespace Platform.DAAS.OData.Security
{
    public class ModuleConfiguration
    {
        public static string DefaultIdentityStoreConnectionName = "DefaultConnection";

        public static string DefaultAuthorizationStoreConnectionName = "DefaultConnection";

        public static string DefaultContextDataScopePropertyName = "DataScopeAuthItems";

        public static string DefaultResourceACConfigurationFilePath = "ac-items-config.xml";

        public static bool ShouldDeleteObsoleteOperationsOnRegistration = true;

        public static bool ShouldDeleteObsoleteDataScopesOnRegistration = true;

        public static bool ShouldDeleteObsoleteRolesOnRegistration = false;

        public static bool ShouldDeleteObsoleteUsersOnRegistration = false;

        public static string DefaultSystemSuperUserID = "USR-SYS-BUILTIN-SUPER";

        public static string DefaultFixedUserPassword = "P@ssword1";

        public static string DefaultFixedUserEmailTemplate = "{0}@default.org";

        public static string DefaultCasUrl = "";

        public static string DefaultCasAuthorizedUrl = "";

        public static string DefaultAccountServiceUrl = "";

        //public static ILogger Logger = null;

        //public static IExHandler ExceptionHandler = null;

        //public static ITracer Tracer = null;

        public static object Regiser()
        {
            string configXmlPath = DefaultResourceACConfigurationFilePath;
            string configXml = "";

            using (FileStream stream = new System.IO.FileStream(configXmlPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    configXml = reader.ReadToEnd();
                }
            }

            ResourceAuthConfiguration authConf = XmlUtility.XmlDeserialize(configXml, typeof(ResourceAuthConfiguration), new Type[] { typeof(Resource), typeof(Subject), typeof(Scope), typeof(Action) }, "utf-8") as ResourceAuthConfiguration;

            ISecurityManager securityManager = new SecurityManager();

            string regiseredOpId = null, registeredScopeId = null;

            string[] ops = securityManager.GetDataTypeOperations(null), scopes = securityManager.GetDataScopes(null);

            List<string> opsRegistered = null, scopesRegistered = null, rolesRegistered = null, allRoles = new List<string>();

            List<object> identityResults = null;

            if (authConf != null)
            {
                opsRegistered = new List<string>();
                scopesRegistered = new List<string>();
                //rolesRegistered = new List<string>();

                foreach (var resource in authConf.Resources)
                {
                    if (resource != null)
                    {
                        if ((resource.Actions != null) && (resource.Actions.Length > 0))
                        {
                            //ops = securityManager.GetDataTypeOperations(resource.Name);

                            foreach (var action in resource.Actions)
                            {
                                if ((action != null) && (!ops.Contains(action.ID.ToLower())) && (!opsRegistered.Contains(action.ID.ToLower())))
                                {
                                    regiseredOpId = (string)securityManager.RegisterOperation(resource.Name, action.ID, action.Name);

                                    if (!String.IsNullOrEmpty(regiseredOpId))
                                    {
                                        opsRegistered.Add(regiseredOpId.ToLower());
                                    }
                                }
                            }

                            if (ShouldDeleteObsoleteOperationsOnRegistration)
                            {
                                using (AuthEntityModelContainer context = new AuthEntityModelContainer())
                                {
                                    var obsoleteRoleOps = context.RoleOperations.Where(ro => !opsRegistered.Contains(ro.OperationId) && !ops.Contains(ro.OperationId));
                                    context.RoleOperations.RemoveRange(obsoleteRoleOps.ToArray());

                                    var obsoleteObjectAuthItems = context.ObjectOperationAuthItems.Where(oo => !opsRegistered.Contains(oo.OperationId) && !ops.Contains(oo.OperationId));
                                    context.ObjectOperationAuthItems.RemoveRange(obsoleteObjectAuthItems.ToArray());

                                    var obsoleteOps = context.Operations.Where(o => !opsRegistered.Contains(o.Id) && !ops.Contains(o.Id));
                                    context.Operations.RemoveRange(obsoleteOps.ToArray());

                                    context.SaveChanges();
                                }
                            } 
                        }

                        if ((resource.Scopes != null) && (resource.Scopes.Length > 0))
                        {
                            //scopes = securityManager.GetDataScopes(resource.Name);

                            foreach (var scope in resource.Scopes)
                            {
                                if ((scope != null) && (!scopes.Contains(scope.ID.ToLower())) &&(!scopesRegistered.Contains(scope.ID.ToLower())))
                                {
                                    registeredScopeId = (string)securityManager.RegisterDataScope(resource.Name, scope.ID, scope.Name, scope.Type, resource.Key);

                                    if (!String.IsNullOrEmpty(registeredScopeId))
                                    {
                                        scopesRegistered.Add(registeredScopeId.ToLower());
                                    }
                                }
                            }

                            if (ShouldDeleteObsoleteDataScopesOnRegistration)
                            {
                                using (AuthEntityModelContainer context = new AuthEntityModelContainer())
                                {
                                    var obsoleteRoleDataScopes = context.RoleDataScopes.Where(rd => !scopesRegistered.Contains(rd.DataScopeId) && !scopes.Contains(rd.DataScopeId));
                                    context.RoleDataScopes.RemoveRange(obsoleteRoleDataScopes.ToArray());

                                    var obsoleteDataScopes = context.DataScopes.Where(ds => !scopesRegistered.Contains(ds.Id) && !scopes.Contains(ds.Id));
                                    context.DataScopes.RemoveRange(obsoleteDataScopes.ToArray());

                                    context.SaveChanges();
                                }
                            }
                        }

                        if ((resource.Subjects != null) && (resource.Subjects.Length > 0))
                        {
                            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new MySQLDatabase(ModuleConfiguration.DefaultIdentityStoreConnectionName)));
                            //var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new MySQLDatabase(ModuleConfiguration.DefaultIdentityStoreConnectionName)));

                            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext(ModuleConfiguration.DefaultIdentityStoreConnectionName)));
                            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new IdentityDbContext(ModuleConfiguration.DefaultIdentityStoreConnectionName)));

                            IDictionary<string, string> rolesToRegister = new Dictionary<string, string>();
                            IDictionary<string, string> roleDescriptions = new Dictionary<string, string>();

                            IdentityUser identityUser = null;

                            IdentityResult identityResult = null;

                            identityResults = new List<object>();

                            foreach (var subject in resource.Subjects)
                            {
                                if (subject.Type.ToLower() == "fixedrole")
                                {
                                    allRoles.Add(subject.ID);

                                    if (!roleManager.RoleExists(subject.Name))
                                    {
                                        rolesToRegister.Add(subject.ID, subject.Name);
                                        roleDescriptions.Add(subject.ID, subject.Description);
                                    }
                                }
                                else if(subject.Type.ToLower() == "fixeduser")
                                {
                                    identityUser = userManager.FindById(subject.ID);

                                    if (identityUser == null)
                                    {
                                        identityUser = userManager.FindByName(subject.Name);
                                    }

                                    if ((identityUser != null) && ShouldDeleteObsoleteUsersOnRegistration)
                                    {
                                        foreach (var role in roleManager.Roles)
                                        {
                                            identityResult = userManager.RemoveFromRole(identityUser.Id, role.Name);
                                            identityResults.Add(identityResult);
                                        }

                                        identityResult = userManager.Delete(identityUser);
                                        identityResults.Add(identityResult);

                                        if (identityResult.Succeeded)
                                        {
                                            identityUser = null;
                                        }
                                    }

                                    if (identityUser == null)
                                    {
                                        identityUser = new IdentityUser()
                                        {
                                            Id = subject.ID,
                                            UserName = subject.Name,
                                            Email = String.Format(DefaultFixedUserEmailTemplate, subject.Name),
                                            //Description = subject.Description,
                                            //UserType = 1
                                        };

                                        identityResult = userManager.Create(identityUser, DefaultFixedUserPassword);

                                        identityResults.Add(identityResult);
                                    }
                                } 
                            }

                            rolesRegistered = securityManager.AddRoles(rolesToRegister, roleDescriptions) as List<string>;

                            if (rolesRegistered == null)
                            {
                                rolesRegistered = new List<string>();
                            }

                            if (ShouldDeleteObsoleteRolesOnRegistration)
                            {
                                using (AuthEntityModelContainer context = new AuthEntityModelContainer())
                                {
                                    var obsoleteRoleDataScopes = context.RoleDataScopes.Where(rd => !rolesRegistered.Contains(rd.RoleId) && !scopes.Contains(rd.RoleId));
                                    context.RoleDataScopes.RemoveRange(obsoleteRoleDataScopes.ToArray());

                                    var obsoleteRoleOps = context.RoleOperations.Where(ro => !rolesRegistered.Contains(ro.RoleId) && !ops.Contains(ro.RoleId));
                                    context.RoleOperations.RemoveRange(obsoleteRoleOps.ToArray());

                                    context.SaveChanges();
                                }

                                var obsoleteRoles = roleManager.Roles.Where(r => !rolesRegistered.Contains(r.Id) && !allRoles.Contains(r.Id));

                                if (obsoleteRoles != null)
                                {
                                    var users = userManager.Users;

                                    foreach (var role in obsoleteRoles.ToArray())
                                    {
                                        foreach (var user in users)
                                        {
                                           identityResult = userManager.RemoveFromRole(user.Id, role.Name);
                                           identityResults.Add(identityResult);
                                        }

                                        identityResult = roleManager.Delete(role);
                                        identityResults.Add(identityResult);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return ((opsRegistered != null) || (scopesRegistered != null) || (rolesRegistered != null) || (identityResults != null)) ? new object[] { opsRegistered, scopesRegistered, rolesRegistered, identityResults } : null;
        }
    }
}
