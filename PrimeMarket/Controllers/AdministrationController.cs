using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PrimeMarket.Models;
using System.Threading.Tasks;
using System.Net;

namespace PrimeMarket.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private PrimeMarketEntities db = new PrimeMarketEntities();
        private readonly RoleManager<IdentityRole> roleManager;
        private /*readonly*/ UserManager<ApplicationUser> userManager;

        public AdministrationController()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            this.roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            //this.roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());

            this.userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        }

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public ActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                // We just need to specify a unique role name to create a new role
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                // Saves the role in the underlying AspNetRoles table
                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                //foreach (IdentityError error in result.Errors)
                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError("", error);//.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<ActionResult> EditRole(string id)
        {
            // Find the role by Role ID
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"هذه الصلاحية غير موجودة";
                return View("Error");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            
            // Retrieve all the Users
            foreach (var user in userManager.Users)
            {
                // If the user is in this role, add the username to
                // Users property of EditRoleViewModel. This model
                // object is then passed to the view for display
                if (await userManager.IsInRoleAsync(user.Id, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                
                // Update the Role using UpdateAsync
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }

                return View(model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> EditUsersInRole_UserList(string id)
        {
            ViewBag.roleId = id;
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                //return View("NotFound");
            }

            var model = new List<UserRole_UserList_ViewModel>();
            foreach (var user in userManager.Users)
            {
                var userRole_UL_ViewModel = new UserRole_UserList_ViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName
                };
                if (await userManager.IsInRoleAsync(user.Id, role.Name))
                {
                    userRole_UL_ViewModel.IsSelected = true;
                }
                else
                {
                    userRole_UL_ViewModel.IsSelected = false;
                }

                model.Add(userRole_UL_ViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditUsersInRole_UserList(List<UserRole_UserList_ViewModel> model, string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                //return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;
                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user.Id, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user.Id, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user.Id, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user.Id, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        //return RedirectToAction("EditRole", new { id = id });
                        return RedirectToAction("ListRoles");
                }
            }

            //return RedirectToAction("EditRole", new { id = id });
            return RedirectToAction("ListRoles", "Administration");
        }
        [HttpGet]
        public async Task<ActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"هذه الصلاحية غير موجودة";
                return View("Error");
            }
            //load the deleted roles
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };
            // load the roles users
            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user.Id, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [HttpPost, ActionName("DeleteRole")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                //return View("NotFound");
            }
            //delete users of deleted roles
            /*
            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user.Id, role.Name))
                    await userManager.RemoveFromRoleAsync(user.Id, role.Name);
            }
            */
            IdentityResult Result = await roleManager.DeleteAsync(role);

            
            return RedirectToAction("ListRoles");
        }
        /*
        // GET: Administration
        public ActionResult Index()
        {
            return View();
        }
        */
    }
}