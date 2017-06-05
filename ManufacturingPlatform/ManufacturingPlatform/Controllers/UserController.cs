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
using Platform.DAAS.OData.Security.Extension;
using Platform.DAAS.OData.Facade;
using Platform.DAAS.OData.Utility;
using DISOpenDataCloud.Models;

namespace DISOpenDataCloud.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        // GET: User
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-USR-VIEW", ShouldByPassSupperUser = true)]
        public ActionResult Index()
        {
            List<UserViewModel> viewModel = null;

            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new IdentityDbContext(Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            var securityManager = Provider.SecurityManager();

            var users = userManager.Users;// userManager.Users.Where(u => (!securityManager.IsSupperUser(u.Id)));

            if (users != null)
            {
                viewModel = new List<UserViewModel>();

                foreach (var user in users.ToArray())
                {
                    if (!securityManager.IsSupperUser(user.Id))
                    {
                        viewModel.Add(new UserViewModel()
                        {
                            ID = user.Id,
                            UserName = user.UserName,
                            Email = user.Email
                        });
                    }
                }
            }

            return View(viewModel);
        }

        // GET: User/Details/5
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-USR-VIEW", ShouldByPassSupperUser = true)]
        public ActionResult Details(string id)
        {
            UserViewModel viewModel = null;

            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new IdentityDbContext(Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            var user = userManager.FindById(id);

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext(Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            var roles = userManager.GetRoles(id);

            IdentityRole identityRole = null;

            viewModel = new UserViewModel()
            {
                Email = user.Email,
                ID = user.Id,
                UserName = user.UserName,
                //Description = user.Description
            };

            //if (account != null)
            //{
            //    viewModel.FirstName = account.FirstName;
            //    viewModel.LastName = account.LastName;
            //    viewModel.NickName = account.NickName;
            //    viewModel.Organization = account.Organization;
            //    viewModel.Position = account.Position;
            //    viewModel.SID = account.SID;
            //    viewModel.Gender = account.Gender;
            //    viewModel.Education = account.Education;
            //    viewModel.Industry = account.Industry;
            //    viewModel.Headline = account.Headline;
            //    viewModel.HeadlImageID = account.HeadImage != null ? account.HeadImage.ID : "";
            //}

            if (roles != null)
            {
                viewModel.Roles = new List<RoleViewModel>();

                foreach (var role in roles)
                {
                    identityRole = roleManager.FindByName(role);

                    if (identityRole != null)
                    {
                        viewModel.Roles.Add(new RoleViewModel()
                        {
                            ID = identityRole.Id,
                            Name = identityRole.Name,
                            //Description = identityRole.Description
                        });
                    }
                }
            }

            return View(viewModel);
        }

        // GET: User/Create
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-USR-CREATE", ShouldByPassSupperUser = true)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-USR-CREATE", ShouldByPassSupperUser = true)]
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

        // GET: User/Edit/5
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-USR-UPDATE", ShouldByPassSupperUser = true)]
        public ActionResult Edit(string id)
        {
            UserViewModel viewModel = null;

            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new IdentityDbContext(Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            var user = userManager.FindById(id);

            //var accountManager = Facade.Facade.GetAccountManager();

            //var account = accountManager.GetAccount(id);

            viewModel = new UserViewModel()
            {
                Email = user.Email,
                ID = user.Id,
                UserName = user.UserName,
                //Description = user.Description
            };

            //if (account != null)
            //{
            //    viewModel.FirstName = account.FirstName;
            //    viewModel.LastName = account.LastName;
            //    viewModel.NickName = account.NickName;
            //    viewModel.Organization = account.Organization;
            //    viewModel.Position = account.Position;
            //    viewModel.SID = account.SID;
            //    viewModel.Gender = account.Gender;
            //    viewModel.Education = account.Education;
            //    viewModel.Industry = account.Industry;
            //    viewModel.Headline = account.Headline;
            //    viewModel.HeadlImageID = account.HeadImage != null ? account.HeadImage.ID : "";
            //}

            return View(viewModel);
        }

        // POST: User/Edit/5
        [HttpPost]
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-USR-UPDATE", ShouldByPassSupperUser = true)]
        public async Task<ActionResult> Edit(string id, FormCollection collection, HttpPostedFileBase headImageFile)
        {
            try
            {
                // TODO: Add update logic here

                var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new IdentityDbContext(Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName)));

                var user = userManager.FindById(id);

                IdentityResult result = null;

                if (user != null)
                {
                    //userManager.SetEmail(user.Id, collection["Email"]);
                    user.Email = collection["Email"];
                    //user.Description = collection["Description"];
                    result = userManager.Update(user);
                }

                //if (result.Succeeded)
                //{
                //    var accountManager = Facade.Facade.GetAccountManager();

                //    var account = accountManager.GetAccount(id);

                //    if (account == null)
                //    {
                //        account = new Core.DomainModel.Account()
                //        {
                //            ID = id,
                //            Name = user.UserName,
                //            FirstName = collection["FirstName"],
                //            LastName = collection["LastName"],
                //            NickName = collection["NickName"],
                //            Organization = collection["Organization"],
                //            Position = collection["Position"],
                //            Gender = int.Parse(collection["Gender"]),
                //            SID = collection["SID"],
                //            Headline = collection["Headline"],
                //            Email = collection["Email"],
                //            Description = collection["Description"]
                //        };

                //        if (headImageFile != null)
                //        {
                //            var ms = imageSize(headImageFile);
                //            account.HeadImage = new Attachment()
                //            {
                //                Name = String.Format("HdImg-{0}-{1}-{2}", ControllerContext.HttpContext.User.Identity.Name, DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss"), headImageFile.FileName.Substring((headImageFile.FileName.LastIndexOf("\\") + 1))), //String.Format("HeadImg-{0}-{1}-{2}", ControllerContext.HttpContext.User.Identity.Name, ControllerContext.HttpContext.User.Identity.GetUserId(), Guid.NewGuid().ToString("N")), //Request.Files[0].FileName,
                //                Bytes = new byte[ms.Length],
                //                Size = ms.Length,
                //                CreationTime = DateTime.Now
                //            };

                //            using (headImageFile.InputStream)
                //            {
                //                account.HeadImage.Bytes = ms.ToArray();
                //            }

                //            if (Facade.Global.Should("DoRemoteFileUpload"))
                //            {
                //                this.DoRemoteFileUpload(account.HeadImage);
                //            }
                //            else
                //            {
                //                accountManager.SetImage(account.HeadImage, account.HeadImage.Bytes);
                //            }
                //        }

                //        accountManager.AddAccount(account);
                //    }
                //    else
                //    {
                //        account.Name = user.UserName;
                //        account.FirstName = collection["FirstName"];
                //        account.LastName = collection["LastName"];
                //        account.NickName = collection["NickName"];
                //        account.Organization = collection["Organization"];
                //        account.Position = collection["Position"];
                //        account.Gender = int.Parse(collection["Gender"]);
                //        account.SID = collection["SID"];
                //        account.Headline = collection["Headline"];
                //        account.Email = collection["Email"];
                //        account.Description = collection["Description"];

                //        if (headImageFile != null)
                //        {
                //            var ms = imageSize(headImageFile);
                //            account.HeadImage = new Attachment()
                //            {
                //                Name = String.Format("HdImg-{0}-{1}-{2}", ControllerContext.HttpContext.User.Identity.Name, DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss"), headImageFile.FileName.Substring((headImageFile.FileName.LastIndexOf("\\") + 1))), //String.Format("HeadImg-{0}-{1}-{2}", ControllerContext.HttpContext.User.Identity.Name, ControllerContext.HttpContext.User.Identity.GetUserId(), Guid.NewGuid().ToString("N")), //Request.Files[0].FileName,
                //                Bytes = new byte[ms.Length],
                //                Size = ms.Length,
                //                CreationTime = DateTime.Now
                //            };

                //            using (headImageFile.InputStream)
                //            {
                //                account.HeadImage.Bytes = ms.ToArray();
                //            }

                //            if (Facade.Global.Should("DoRemoteFileUpload"))
                //            {
                //                this.DoRemoteFileUpload(account.HeadImage);
                //            }
                //            else
                //            {
                //                accountManager.SetImage(account.HeadImage, account.HeadImage.Bytes);
                //            }
                //        }

                //        accountManager.SetAccount(id, account);
                //    }

                    //return RedirectToAction("Index");
                //    return RedirectToAction("Details", new { id = id });
                //}

                //return RedirectToAction("Index");
                return RedirectToAction("Details", new { id = id });
            }
            catch (Exception ex)
            {
                Provider.ExceptionHandler().HandleException(ex);
                return View();
            }
        }

        // GET: User/Delete/5
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-USR-DELETE", ShouldByPassSupperUser = true)]
        public ActionResult Delete(string id)
        {
            UserViewModel viewModel = null;

            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new IdentityDbContext(Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            var user = userManager.FindById(id);

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext(Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            var roles = userManager.GetRoles(id);

            IdentityRole identityRole = null;

            viewModel = new UserViewModel()
            {
                Email = user.Email,
                ID = user.Id,
                UserName = user.UserName,
                //Description = user.Description
            };

            if (roles != null)
            {
                viewModel.Roles = new List<RoleViewModel>();

                foreach (var role in roles)
                {
                    identityRole = roleManager.FindByName(role);

                    if (identityRole != null)
                    {
                        viewModel.Roles.Add(new RoleViewModel()
                        {
                            ID = identityRole.Id,
                            Name = identityRole.Name,
                            //Description = identityRole.Description
                        });
                    }
                }
            }

            return View(viewModel);
        }

        // POST: User/Delete/5
        [HttpPost]
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-USR-DELETE", ShouldByPassSupperUser = true)]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                var securityManager = Provider.SecurityManager();

                var results = securityManager.DeleteActor(id);

                Provider.Logger().LogUserOperation(ControllerContext.HttpContext.User.Identity.Name, String.Format("User \"{0}\" deleted user \"{1}\"", ControllerContext.HttpContext.User.Identity.Name, id));

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Provider.ExceptionHandler().HandleException(ex);
                return View();
            }
        }

        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-USR-DISPATCH-ROLE", ShouldByPassSupperUser = true)]
        public ActionResult UserRoles(string id)
        {
            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new IdentityDbContext(Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext(Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            IList<string> userRoles = userManager.GetRoles(id);

            IdentityUser identityUser = userManager.FindById(id);

            IdentityRole[] identityRoles = roleManager.Roles.ToArray();

            UserRoleViewModel userRoleViewModel = new UserRoleViewModel()
            {
                ID = identityUser.Id,
                UserName = identityUser.UserName,
                Roles = new List<RoleViewModel>(),
                UserRoles = new List<RoleViewModel>()
            };

            foreach (var identityRole in identityRoles)
            {
                userRoleViewModel.Roles.Add(new RoleViewModel()
                {
                    ID = identityRole.Id,
                    Name = identityRole.Name,
                    //Description = identityRole.Description,
                    IsSelected = userRoles.Contains(identityRole.Name),
                });

                if (userRoles.Contains(identityRole.Name))
                {
                    userRoleViewModel.UserRoles.Add(new RoleViewModel()
                    {
                        ID = identityRole.Id,
                        Name = identityRole.Name,
                        //Description = identityRole.Description,
                        IsSelected = true
                    });
                }
            }

            return View(userRoleViewModel);
        }

        [HttpPost]
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-USR-DISPATCH-ROLE", ShouldByPassSupperUser = true)]
        public ActionResult UserRoles(string id, FormCollection roleItems)
        {
            try
            {
                if (roleItems != null)
                {
                    Dictionary<string, object> roleItemValues = new Dictionary<string, object>();

                    List<string> userRoles = null;

                    foreach (string key in roleItems.Keys)
                    {
                        if (key.ToLower() != "id")
                        {
                            roleItemValues.Add(key, roleItems.GetValues(key));
                        }
                    }

                    if (roleItemValues != null)
                    {
                        userRoles = new List<string>();

                        foreach (var key in roleItemValues.Keys)
                        {
                            if ((roleItemValues[key] != null) && (roleItemValues[key] as string[]).Length >= 1)
                            {
                                if ((roleItemValues[key] as string[])[0].ToLower() == "true")
                                {
                                    userRoles.Add(key);
                                }
                            }
                        }
                    }

                    var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new IdentityDbContext(Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName)));

                    IList<string> currentUserRoles = userManager.GetRoles(id);

                    var securityManager = Provider.SecurityManager();

                    List<object> setRoleResults = new List<object>();


                    if ((currentUserRoles != null) && (currentUserRoles.Count > 0))
                    {
                        IdentityResult identityResult = userManager.RemoveFromRoles(id, currentUserRoles.ToArray());

                        setRoleResults.Add(identityResult);
                    }

                    if (userRoles != null)
                    {
                        IdentityRole identityRole = null;
                        var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext(Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName)));

                        foreach (var role in userRoles)
                        {
                            identityRole = roleManager.FindById(role);

                            if (identityRole != null)
                            {
                                setRoleResults.Add(securityManager.AddActorToRole(id, identityRole.Name)); //Add the user to each of the roles selected.
                            }
                        }
                    }
                }

                return RedirectToAction("Details", new { id = id });//return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Provider.ExceptionHandler().HandleException(ex);
                return View();
            }
        }

        // GET: /User/ResetPassword
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-USR-PWD-RESET", ShouldByPassSupperUser = true)]
        public ActionResult ResetPassword(string id)
        {
            ResetPasswordViewModel viewModel = null;

            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new IdentityDbContext(Platform.DAAS.OData.Security.ModuleConfiguration.DefaultIdentityStoreConnectionName)));

            var user = userManager.FindById(id);

            viewModel = new ResetPasswordViewModel()
            {
                Code = user.Id,
                Email = user.Email,
                //PhoneNumber = user.PhoneNumber
            };

            return View(viewModel);
        }

        // POST: /User/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-USR-PWD-RESET", ShouldByPassSupperUser = true)]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); // new UserManager<IdentityUser>(new UserStore<IdentityUser>(new MySQLDatabase(ModuleConfiguration.DefaultIdentityStoreConnectionName)));

                var user = userManager.FindByName(model.Email); //userManager.FindByName(model.PhoneNumber);

                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    //return RedirectToAction("ResetPasswordConfirmation", "Account");
                    return RedirectToAction("ResetPasswordFailure", "User");
                }

                string passwordResetToken = userManager.GeneratePasswordResetToken(user.Id);

                var result = userManager.ResetPassword(user.Id, passwordResetToken, model.Password);

                Provider.Logger().LogUserOperation(ControllerContext.HttpContext.User.Identity.Name, String.Format("User \"{0}\" reset password for user \"{1}\"", ControllerContext.HttpContext.User.Identity.Name, model.Email));

                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordSuccess", "User");
                }

                TempData.Add("Result", result);

                return RedirectToAction("ResetPasswordFailure", "User");
            }
            catch (Exception ex)
            {
                Provider.ExceptionHandler().HandleException(ex);
                return View();
            }
        }

        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-USR-PWD-RESET", ShouldByPassSupperUser = true)]
        public ActionResult ResetPasswordSuccess()
        {
            return View();
        }

        [MVCControllerAuth(DataType = "System", IsValidatingDataScope = false, Operation = "SYS-USR-PWD-RESET", ShouldByPassSupperUser = true)]
        public ActionResult ResetPasswordFailure()
        {
            return View(TempData["Result"]);
        }
    }
}
