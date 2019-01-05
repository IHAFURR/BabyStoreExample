using System;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using BabyStore.DAL;
using BabyStore.Models;

namespace BabyStore.Controllers
{
    public class ProductImagesController : Controller
    {
        private StoreContext db = new StoreContext();

        // GET: ProductImages
        public ActionResult Index()
        {
            return View(db.ProductImages.ToList());
        }

        // GET: ProductImages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = db.ProductImages.Find(id);
            if (productImage == null)
            {
                return HttpNotFound();
            }
            return View(productImage);
        }

        // GET: ProductImages/Create
        public ActionResult Upload()
        {
            return View();
        }

        // POST: ProductImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            // check if user entered a file
            if (file != null)
            {
                // check if file is valid
                if (ValidateFile(file))
                {
                    try
                    {
                        SaveFileToDisk(file);
                    }
                    catch (Exception)
                    {

                        ModelState.AddModelError("FileName", "Sorry an error occured saving the file to disk, please try again.");
                    }
                }
                else
                {   
                    // validation error
                    ModelState.AddModelError("FileName", "The file must be .gif, .png, .jpeg or jpg and less then 2MB in size.");
                }
            }
            else
            {
                // if user has not entered a file return an error message 
                ModelState.AddModelError("FileName", "Please choose a file.");
            }

            if (ModelState.IsValid)
            {
                db.ProductImages.Add(new ProductImage { FileName = file.FileName });
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException ex)
                {

                    SqlException innerException = ex.InnerException.InnerException as SqlException;
                    if (innerException != null && innerException.Number == 2601)
                    {
                        ModelState.AddModelError("FileName", "The file " + file.FileName + " already exsits in the system." +
                            " Please delete it and try again if you wish re-add it.");
                    }
                    else
                    {
                        ModelState.AddModelError("FileName", "Sorry an error has occured saving to the database, please try again.");
                    }
                    return View();
                }               
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: ProductImages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = db.ProductImages.Find(id);
            if (productImage == null)
            {
                return HttpNotFound();
            }
            return View(productImage);
        }

        // POST: ProductImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FileName")] ProductImage productImage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productImage);
        }

        // GET: ProductImages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = db.ProductImages.Find(id);
            if (productImage == null)
            {
                return HttpNotFound();
            }
            return View(productImage);
        }

        // POST: ProductImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductImage productImage = db.ProductImages.Find(id);
            db.ProductImages.Remove(productImage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Move this method to another reusable class
        private bool ValidateFile(HttpPostedFileBase file)
        {
            string[] allowedFileTypes = { ".gif", ".gpeg", ".png", ".jpg" };
            string fileExt = System.IO.Path.GetExtension(file.FileName).ToLower();
            if ((file.ContentLength > 0 && file.ContentLength < 2097152) && allowedFileTypes.Contains(fileExt))
            {
                return true;
            }
            return false;
        }

        // resize and save image and thimbnail
        private void SaveFileToDisk(HttpPostedFileBase file)
        {
            WebImage img = new WebImage(file.InputStream);
            if (img.Width > 190)
            {
                img.Resize(190, img.Height);
            }
            img.Save(Constants.ProductImagePath + img.FileName);
            if (img.Width > 100)
            {
                img.Resize(100, img.Height);
            }
            img.Save(Constants.ProductThumbnailPath + file.FileName);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
