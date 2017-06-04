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
    public class OperationController : Controller
    {
        // GET: Operation
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-OP-VIEW", ShouldByPassSupperUser = true)]
        public ActionResult Index(string dataType)
        {
            //List<OperationViewModel> viewModel = new List<OperationViewModel>();

            OperationListViewModel viewModel = new OperationListViewModel() { DataTypes = new List<string>() };

            using (AuthEntityModelContainer context = new AuthEntityModelContainer())
            {
                var ops = context.Operations.ToArray();

                if (ops != null)
                {
                    foreach (var op in ops)
                    {
                        if (String.IsNullOrEmpty(dataType) || (op.DataType.ToLower() == dataType.ToLower()))
                        {
                            viewModel.Add(new OperationViewModel()
                            {
                                DataType = op.DataType,
                                ID = op.Id,
                                Name = op.Name
                            });
                        }

                        if (!viewModel.DataTypes.Contains(op.DataType))
                        {
                            viewModel.DataTypes.Add(op.DataType);
                        }
                    }
                }
            }

            return View(viewModel);
        }

        // GET: Operation/Details/5
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-OP-VIEW", ShouldByPassSupperUser = true)]
        public ActionResult Details(string id)
        {
            OperationViewModel viewModel = null;

            using (AuthEntityModelContainer context = new AuthEntityModelContainer())
            {
                var op = context.Operations.FirstOrDefault(o => o.Id == id);

                if (op != null)
                {
                    viewModel = new OperationViewModel()
                    {
                        DataType = op.DataType,
                        ID = op.Id,
                        Name = op.Name
                    };

                    if (op.RoleOperations != null)
                    {
                        viewModel.Roles = new List<RoleViewModel>();

                        foreach (var role in op.RoleOperations)
                        {
                            viewModel.Roles.Add(new RoleViewModel()
                            {
                                ID = role.RoleId
                            });
                        }
                    }
                }
            }

            if (viewModel.Roles != null)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext(Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName)));

                IdentityRole identityRole = null;

                for (int i = 0; i < viewModel.Roles.Count; i++)
                {
                    identityRole = roleManager.FindById(viewModel.Roles[i].ID);

                    if (identityRole != null)
                    {
                        viewModel.Roles[i].Name = identityRole.Name;
                        //viewModel.Roles[i].Description = identityRole.Description;
                    }
                }
            }

            return View(viewModel);
        }

        // GET: Operation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Operation/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Operation/Edit/5
        public ActionResult Edit(string id)
        {
            return View();
        }

        // POST: Operation/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Operation/Delete/5
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-OP-DELETE", ShouldByPassSupperUser = true)]
        public ActionResult Delete(string id)
        {
            OperationViewModel viewModel = null;

            using (AuthEntityModelContainer context = new AuthEntityModelContainer())
            {
                var op = context.Operations.FirstOrDefault(o => o.Id == id);

                if (op != null)
                {
                    viewModel = new OperationViewModel()
                    {
                        DataType = op.DataType,
                        ID = op.Id,
                        Name = op.Name
                    };

                    if (op.RoleOperations != null)
                    {
                        viewModel.Roles = new List<RoleViewModel>();

                        foreach (var role in op.RoleOperations)
                        {
                            viewModel.Roles.Add(new RoleViewModel()
                            {
                                ID = role.RoleId
                            });
                        }
                    }
                }
            }

            if (viewModel.Roles != null)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext(ModuleConfiguration.DefaultIdentityStoreConnectionName)));

                IdentityRole identityRole = null;

                for (int i = 0; i < viewModel.Roles.Count; i++)
                {
                    identityRole = roleManager.FindById(viewModel.Roles[i].ID);

                    if (identityRole != null)
                    {
                        viewModel.Roles[i].Name = identityRole.Name;
                        //viewModel.Roles[i].Description = identityRole.Description;
                    }
                }
            }

            return View(viewModel);
        }

        // POST: Operation/Delete/5
        [HttpPost]
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-OP-DELETE", ShouldByPassSupperUser = true)]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                var securityManager = Provider.SecurityManager();

                var results = securityManager.DeleteOperation(id);

                Provider.Logger().LogUserOperation(ControllerContext.HttpContext.User.Identity.Name, String.Format("User \"{0}\" deleted operation \"{1}\".", ControllerContext.HttpContext.User.Identity.Name, id));

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Provider.ExceptionHandler().HandleException(ex);
                return View();
            }
        }

        // GET: Operation/Roles/
        //[Route("Operation/Roles/")]
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-ROLE-DISPATCH-OP", ShouldByPassSupperUser = true)]
        public ActionResult OperationRoles(string operationId)
        {
            OperationRoleViewModel opViewModel = null;

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext(ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            using (AuthEntityModelContainer context = new AuthEntityModelContainer())
            {
                var op = context.Operations.FirstOrDefault(o => o.Id == operationId);

                if (op != null)
                {
                    opViewModel = new OperationRoleViewModel()
                    {
                        DataType = op.DataType,
                        ID = op.Id,
                        Name = op.Name
                    };

                    if (op.RoleOperations != null)
                    {
                        opViewModel.OperationRoles = new List<RoleViewModel>();

                        foreach (var role in op.RoleOperations)
                        {
                            opViewModel.OperationRoles.Add(new RoleViewModel()
                            {
                                ID = role.RoleId
                            });
                        }
                    }
                }
            }

            if (opViewModel.OperationRoles != null)
            {
                IdentityRole identityRole = null;

                for (int i = 0; i < opViewModel.OperationRoles.Count; i++)
                {
                    identityRole = roleManager.FindById(opViewModel.OperationRoles[i].ID);

                    if (identityRole != null)
                    {
                        opViewModel.OperationRoles[i].Name = identityRole.Name;
                        //opViewModel.OperationRoles[i].Description = identityRole.Description;
                        opViewModel.OperationRoles[i].IsSelected = true;
                    }
                }
            }

            var roles = roleManager.Roles;

            if (roles != null)
            {
                opViewModel.Roles = new List<RoleViewModel>();

                foreach (var role in roles)
                {
                    opViewModel.Roles.Add(new RoleViewModel()
                    {
                        ID = role.Id,
                        Name = role.Name,
                        //Description = role.Description,
                        IsSelected = (opViewModel.OperationRoles != null && opViewModel.OperationRoles.FirstOrDefault(or => or.ID.ToLower() == role.Id.ToLower()) != null)
                    });
                }
            }

            return View(opViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-ROLE-DISPATCH-OP", ShouldByPassSupperUser = true)]
        public async Task<ActionResult> OperationRoles(string id, FormCollection roleItems)
        {
            try
            {
                if (roleItems != null)
                {
                    Dictionary<string, object> roleItemValues = new Dictionary<string, object>();

                    List<string> operationRoles = null;

                    foreach (string key in roleItems.Keys)
                    {
                        if (key.ToLower() != "id")
                        {
                            roleItemValues.Add(key, roleItems.GetValues(key));
                        }
                    }

                    if (roleItemValues != null)
                    {
                        operationRoles = new List<string>();

                        foreach (var key in roleItemValues.Keys)
                        {
                            if ((roleItemValues[key] != null) && (roleItemValues[key] as string[]).Length >= 1)
                            {
                                if ((roleItemValues[key] as string[])[0].ToLower() == "true")
                                {
                                    operationRoles.Add(key);
                                }
                            }
                        }
                    }

                    using (AuthEntityModelContainer context = new AuthEntityModelContainer())
                    {
                        var roleOps = context.RoleOperations.Where(ro => (ro.OperationId.ToLower() == id.ToLower()));//context.RoleOperations.Where((ro => (ro.OperationId.ToLower() == id.ToLower()) && (!operationRoles.Contains(ro.OperationId))));

                        if (roleOps != null)
                        {
                            var roleOpArray = roleOps.ToArray();

                            var deletedRoleOpes = context.RoleOperations.RemoveRange(roleOpArray);

                            context.SaveChanges();
                        }
                    }

                    if (operationRoles != null)
                    {
                        var securityManager = Provider.SecurityManager();

                        List<object> setRoleResults = new List<object>();

                        foreach (var opRole in operationRoles)
                        {
                            setRoleResults.Add(securityManager.SetRoleOperation(opRole, id));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Provider.ExceptionHandler().HandleException(ex);
                return View();
            }

            return RedirectToAction("Details", new { id = id }); //RedirectToAction("Index");
        }
    }
}
