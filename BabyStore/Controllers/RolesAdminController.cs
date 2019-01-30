using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Threading.Tasks;
using BabyStore.Models;
using System.Collections.Generic;
using System.Linq;

namespace BabyStore.Controllers
{
    public class RolesAdminController : Controller
    {
        private ApplicationRoleManager _roleManager;
        private ApplicationUserManager _userManager;
        public ApplicationRoleManager RoleManager {
            get {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            set
            {
                _roleManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().Get<ApplicationUserManager>();
            }

            set
            {
                _userManager = value;
            }
        }

        public RolesAdminController()
        {
        }

        public RolesAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }


        // GET: RolesAdmin
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }

        // GET: RolesAdmin/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManager.FindByIdAsync(id);
            // Get the list of Users in this Role
            var users = new List<ApplicationUser>();

            // Get the list of Users in this Role
            foreach (var user in UserManager.Users.ToList())
            {
                if (await UserManager.IsInRoleAsync(user.Id, role.Name))
                {
                    users.Add(user);
                }
            }

            ViewBag.Users = users;
            ViewBag.Count = users.Count();

            return View(role);
        }

        // GET: RolesAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RolesAdmin/Create
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

        // GET: RolesAdmin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RolesAdmin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
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

        // GET: RolesAdmin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RolesAdmin/Delete/5
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
