using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Configuration;
using Platform.DAAS.OData.Core;
using Platform.DAAS.OData.Core.Security;
using Platform.DAAS.OData.Security;
using Platform.DAAS.OData.Security.Extension;
using Platform.DAAS.OData.Facade;
using Platform.DAAS.OData.Utility;
using DISOpenDataCloud.Models;

namespace DISOpenDataCloud.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        // GET: Role
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-ROLE-VIEW", ShouldByPassSupperUser = true)]
        public ActionResult Index()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext(Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            var roles = roleManager.Roles;

            List<RoleViewModel> viewModel = new List<RoleViewModel>();

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    viewModel.Add(new RoleViewModel()
                    {
                        ID = role.Id,
                        Name = role.Name
                        //, Description = role.Description
                    });
                }
            }

            return View(viewModel);
        }

        // GET:Role/Details/5
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-ROLE-VIEW", ShouldByPassSupperUser = true)]
        public ActionResult Details(string id)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext(Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            var role = roleManager.FindById(id);

            return View(new RoleViewModel()
            {
                ID = role.Id,
                Name = role.Name
                //, Description = role.Description
            });
        }

        // GET: Role/Create
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-ROLE-CREATE", ShouldByPassSupperUser = true)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Role/Create
        [HttpPost]
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-ROLE-CREATE", ShouldByPassSupperUser = true)]
        public ActionResult Create(RoleViewModel role)
        {
            try
            {
                // TODO: Add insert logic here

                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext(Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName)));

                if (!roleManager.RoleExists(role.Name))
                {
                    roleManager.Create(new IdentityRole()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = role.Name//,
                        //Description = role.Description
                    });
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Role/Edit/5
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-ROLE-UPDATE", ShouldByPassSupperUser = true)]
        public ActionResult Edit(string id)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext(Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            var role = roleManager.FindById(id);

            return View(new RoleViewModel()
            {
                ID = role.Id,
                Name = role.Name
                //, Description = role.Description
            });
        }

        // POST: Role/Edit/5
        [HttpPost]
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-ROLE-UPDATE", ShouldByPassSupperUser = true)]
        public ActionResult Edit(string id, RoleViewModel role)
        {
            try
            {
                // TODO: Add update logic here

                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext(Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName)));

                if (roleManager.RoleExists(role.Name))
                {
                    roleManager.Update(new IdentityRole()
                    {
                        Id = role.ID,
                        Name = role.Name//,
                        //Description = role.Description
                    });
                }

                //return RedirectToAction("Index");
                return RedirectToAction("Details", new { id = id });
            }
            catch (Exception ex)
            {
                Provider.ExceptionHandler().HandleException(ex);
                return View();
            }
        }

        // GET: Role/Delete/5
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-ROLE-DELETE", ShouldByPassSupperUser = true)]
        public ActionResult Delete(string id)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext(Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            var role = roleManager.FindById(id);

            return View(new RoleViewModel()
            {
                ID = role.Id,
                Name = role.Name
                //, Description = role.Description
            });
        }

        // POST: Role/Delete/5
        [HttpPost]
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-ROLE-DELETE", ShouldByPassSupperUser = true)]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                var securityManager = Provider.SecurityManager();

                var results = securityManager.DeleteRole(id);

                Provider.Logger().LogUserOperation(ControllerContext.HttpContext.User.Identity.Name, String.Format("User \"{0}\" deleted role \"{1}\".", ControllerContext.HttpContext.User.Identity.Name, id));

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Provider.ExceptionHandler().HandleException(ex);
                return View();
            }
        }

        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-ROLE-DISPATCH-OP", ShouldByPassSupperUser = true)]
        public ActionResult RoleOperations(string id)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext(Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            var role = roleManager.FindById(id);

            if (role != null)
            {
                RoleOperationViewModel viewModel = new RoleOperationViewModel()
                {
                    ID = role.Id,
                    Name = role.Name,
                    Operations = new OperationListViewModel() { DataTypes = new List<string>() },
                    RoleOperations = new List<OperationViewModel>()
                };

                using (AuthEntityModelContainer context = new AuthEntityModelContainer())
                {
                    var operations = context.Operations;

                    if (operations != null)
                    {
                        foreach (var operation in operations.ToArray())
                        {
                            viewModel.Operations.Add(new OperationViewModel()
                            {
                                DataType = operation.DataType,
                                ID = operation.Id,
                                Name = operation.Name
                            });

                            if (!viewModel.Operations.DataTypes.Contains(operation.DataType))
                            {
                                viewModel.Operations.DataTypes.Add(operation.DataType);
                            }
                        }
                    }

                    var roleOps = context.RoleOperations.Where(ro => ro.RoleId.ToLower() == role.Id.ToLower());

                    if (roleOps != null)
                    {
                        foreach (var roleOp in roleOps.ToArray())
                        {
                            viewModel.RoleOperations.Add(new OperationViewModel()
                            {
                                DataType = roleOp.Operation.DataType,
                                ID = roleOp.OperationId,
                                Name = roleOp.Operation.Name
                            });
                        }
                    }
                }

                return View(viewModel);
            }

            return View();
        }

        [HttpPost]
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-ROLE-DISPATCH-OP", ShouldByPassSupperUser = true)]
        public ActionResult RoleOperations(string id, FormCollection opItems)
        {
            try
            {
                var logger = Provider.Logger();

                if (opItems != null)
                {
                    Dictionary<string, object> opItemValues = new Dictionary<string, object>();

                    List<string> roleOperations = null;

                    foreach (string key in opItems.Keys)
                    {
                        if (key.ToLower() != "id")
                        {
                            opItemValues.Add(key, opItems.GetValues(key));
                        }
                    }

                    if (opItemValues != null)
                    {
                        roleOperations = new List<string>();

                        foreach (var key in opItemValues.Keys)
                        {
                            if ((opItemValues[key] != null) && (opItemValues[key] as string[]).Length >= 1)
                            {
                                if ((opItemValues[key] as string[])[0].ToLower() == "true")
                                {
                                    roleOperations.Add(key);
                                }
                            }
                        }
                    }

                    using (AuthEntityModelContainer context = new AuthEntityModelContainer())
                    {
                        var roleOps = context.RoleOperations.Where(ro => (ro.RoleId.ToLower() == id.ToLower()));

                        if (roleOps != null)
                        {
                            var roleOpArray = roleOps.ToArray();

                            var deletedRoleOpes = context.RoleOperations.RemoveRange(roleOpArray);

                            context.SaveChanges();
                        }
                    }

                    if (roleOperations != null)
                    {
                        var securityManager = Provider.SecurityManager();
                        List<object> setRoleResults = new List<object>();

                        foreach (var roleOp in roleOperations)
                        {
                            setRoleResults.Add(securityManager.SetRoleOperation(id, roleOp));
                        }

                        logger.LogUserOperation(ControllerContext.HttpContext.User.Identity.Name, String.Format("User \"{0}\" dispatched operation(s) to role \"{1}\".", ControllerContext.HttpContext.User.Identity.Name, id));
                    }
                }

                return RedirectToAction("Details", new { id = id });
            }
            catch (Exception ex)
            {
                Provider.ExceptionHandler().HandleException(ex);
                return View();
            }
        }

        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-USR-DISPATCH-ROLE", ShouldByPassSupperUser = true)]
        public ActionResult RoleUsers(string id)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext(Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            IdentityRole identityRole = roleManager.FindById(id);

            if (identityRole != null)
            {
                var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new IdentityDbContext(Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName)));
                var securityManager = Provider.SecurityManager();

                var identityUsers = userManager.Users; //userManager.Users.Where(u => !securityManager.IsSupperUser(u.Id));

                RoleUserViewModel viewModel = new RoleUserViewModel()
                {
                    ID = identityRole.Id,
                    Name = identityRole.Name,
                    RoleUsers = new List<UserViewModel>(),
                    Users = new List<UserViewModel>()
                };

                if (identityUsers != null)
                {
                    foreach (var user in identityUsers.ToArray())
                    {
                        if (!securityManager.IsSupperUser(user.Id))
                        {
                            viewModel.Users.Add(new UserViewModel()
                            {
                                ID = user.Id,
                                UserName = user.UserName,
                                //Description = user.Description,
                                Email = user.Email
                            });

                            if (userManager.IsInRole(user.Id, identityRole.Name))
                            {
                                viewModel.RoleUsers.Add(new UserViewModel()
                                {
                                    ID = user.Id,
                                    UserName = user.UserName,
                                    //Description = user.Description,
                                    Email = user.Email
                                });
                            }
                        } 
                    }
                }

                return View(viewModel);
            }

            return View();
        }

        [HttpPost]
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-USR-DISPATCH-ROLE", ShouldByPassSupperUser = true)]
        public ActionResult RoleUsers(string id, FormCollection userItems)
        {
            try
            {
                var logger = Provider.Logger();

                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext(Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName)));

                IdentityRole identityRole = roleManager.FindById(id);

                if ((identityRole != null) && (userItems != null))
                {
                    Dictionary<string, object> userItemValues = new Dictionary<string, object>();

                    List<string> roleUsers = null;

                    foreach (string key in userItems.Keys)
                    {
                        if (key.ToLower() != "id")
                        {
                            userItemValues.Add(key, userItems.GetValues(key));
                        }
                    }

                    if (userItemValues != null)
                    {
                        roleUsers = new List<string>();

                        foreach (var key in userItemValues.Keys)
                        {
                            if ((userItemValues[key] != null) && (userItemValues[key] as string[]).Length >= 1)
                            {
                                if ((userItemValues[key] as string[])[0].ToLower() == "true")
                                {
                                    roleUsers.Add(key);
                                }
                            }
                        }
                    }

                    var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new IdentityDbContext(Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName)));

                    IList<string> currentRoleUsers = null;

                    var securityManager = Provider.SecurityManager();

                    var identityUsers = userManager.Users; //userManager.Users.Where(u => !securityManager.IsSupperUser(u.Id));

                    if (identityUsers != null)
                    {
                        currentRoleUsers = new List<string>();

                        foreach (var user in identityUsers.ToArray())
                        {
                            if (!securityManager.IsSupperUser(user.Id) && userManager.IsInRole(user.Id, identityRole.Name))
                            {
                                currentRoleUsers.Add(user.Id);
                            }
                        }
                    }

                    List<object> setRoleResults = new List<object>();

                    if ((currentRoleUsers != null) && (currentRoleUsers.Count > 0))
                    {
                        IdentityResult identityResult = null;

                        foreach (var user in currentRoleUsers)
                        {
                            identityResult = userManager.RemoveFromRole(user, identityRole.Name);
                            setRoleResults.Add(identityResult);
                        }
                    }

                    if (roleUsers != null)
                    {
                        foreach (var user in roleUsers)
                        {
                            setRoleResults.Add(securityManager.AddActorToRole(user, identityRole.Name)); //Add each of the users selected to the current role
                        }
                    }

                    logger.LogUserOperation(ControllerContext.HttpContext.User.Identity.Name, String.Format("User \"{0}\" added user(s) to role \"{1}\".", ControllerContext.HttpContext.User.Identity.Name, id));
                }

                return RedirectToAction("Details", new { id = id });
            }
            catch (Exception ex)
            {
                Provider.ExceptionHandler().HandleException(ex);
                return View();
            }
        }
    }
}
