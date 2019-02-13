using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

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
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserAdmin/Create
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

        // GET: UserAdmin/Edit/5
        public ActionResult Edit(string id)
        {
            return View();
        }

        // POST: UserAdmin/Edit/5
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
