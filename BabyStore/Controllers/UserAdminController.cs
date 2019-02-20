using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using BabyStore.Models;
using BabyStore.ViewModel;

namespace BabyStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserAdminController : Controller
    {

        private ApplicationUserManager _applicationUserManager;
        private ApplicationRoleManager _applicationRoleManager;
        public UserAdminController()
        {
        }

        public UserAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            ApplicationUserManager = userManager;
            ApplicationRoleManager = roleManager;
        }

        public ApplicationRoleManager ApplicationRoleManager {
            get
            {
                return _applicationRoleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            set
            {
                _applicationRoleManager = value;
            }
        }

        public ApplicationUserManager ApplicationUserManager {
            get
            {
                return _applicationUserManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _applicationUserManager = value;
            }
        }


        // GET: UserAdmin
        public async Task<ActionResult> Index()
        {
            return View(await ApplicationUserManager.Users.ToListAsync());
        }

        // GET: UserAdmin/Details/5        
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await ApplicationUserManager.FindByIdAsync(id);

            ViewBag.RoleNames = await ApplicationUserManager.GetRolesAsync(user.Id);
            return View(user);
        }

        // GET: UserAdmin/Create
        public async Task<ActionResult> Create()
        {
            // get the list of roles
            ViewBag.RoleId = new SelectList(await ApplicationRoleManager.Roles.ToListAsync(), "Name", "Name");
            return View();
        }

        // POST: UserAdmin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel registerViewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = registerViewModel.Email,
                    Email = registerViewModel.Email,
                    DateOfBirth = registerViewModel.DateOfBirth,
                    FirstName = registerViewModel.FirstName,
                    LastName = registerViewModel.LastName,
                    Address = registerViewModel.Address
                };
                var adminResult = await ApplicationUserManager.CreateAsync(user, registerViewModel.Password);

                // Add User to the selected roles
                if (adminResult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        // for each selected roles add user id
                        foreach (var item in selectedRoles)
                        {
                            var result = await ApplicationUserManager.AddToRoleAsync(user.Id, item.ToString());
                            if (!result.Succeeded)
                            {
                                ModelState.AddModelError("", result.Errors.First());
                                ViewBag.RoleId = new SelectList(await ApplicationRoleManager.Roles.ToListAsync(), "Name", "Name");
                                return View();
                            }
                        }                                                
                    }
                }
                else
                {
                    ModelState.AddModelError("", adminResult.Errors.First());
                    ViewBag.RoleId = new SelectList(ApplicationRoleManager.Roles, "Name", "Name");
                    return View();
                }
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(ApplicationRoleManager.Roles, "Names", "Name");
            return View();
        }

        // GET: UserAdmin/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await ApplicationUserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = await ApplicationUserManager.GetRolesAsync(user.Id);
            var userViewModel = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                RoleList = ApplicationRoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                }
                )
            };
            return View(userViewModel);
        }

        // POST: UserAdmin/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(EditUserViewModel editUserViewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = await ApplicationUserManager.FindByIdAsync(editUserViewModel.Id);

                if (user == null)
                {
                    return HttpNotFound();
                }

                user.DateOfBirth = editUserViewModel.DateOfBirth;
                user.FirstName = editUserViewModel.FirstName;
                user.LastName = editUserViewModel.LastName;
                user.Address = editUserViewModel.Address;

                var userRoles = await ApplicationUserManager.GetRolesAsync(user.Id);

                selectedRoles = selectedRoles ?? new string[] { };

                var result = await ApplicationUserManager.AddToRolesAsync(user.Id, selectedRoles.Except(userRoles).ToArray<String>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }

                result = await ApplicationUserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Something is wrong, ask administrator.");
            return View();
        }

        // GET: UserAdmin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserAdmin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
