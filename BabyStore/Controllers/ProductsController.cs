using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BabyStore.DAL;
using BabyStore.Models;
using BabyStore.ViewModel;
using PagedList;

namespace BabyStore.Controllers
{
    public class ProductsController : Controller
    {
        //Initiate a new viewmodel
        ProductIndexViewModel productViewModel = new ProductIndexViewModel();

        private StoreContext db = new StoreContext();

        // Get Products
        public ActionResult Index(string category, string search, string sortBy, int? page)
        {
            var products = db.Products.Include(p => p.Category);

            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.Contains(search) ||
                p.Description.Contains(search) ||
                p.Category.Name.Contains(search));
                productViewModel.Search = search;
            }

            // group search result into categories and count how many items in each category
            productViewModel.CategsWithCount = from matchingProduct in products
                                               where
                                               matchingProduct.CategoryId != null
                                               group matchingProduct by matchingProduct.Category.Name
                                               into categGroup
                                               select new CategoryWithCounts
                                               {
                                                   CategoryName = categGroup.Key,
                                                   ProductCount = categGroup.Count()
                                               };


            // should be after search string to allow get all categories
            if (!String.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category.Name == category);
                productViewModel.Category = category;
            }

            //sort by price
            switch (sortBy)
            {
                case "price_lowest":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_highest":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }

            int currentPage = (page ?? 1);
            productViewModel.Products = products.ToPagedList(currentPage, Constants.PageItems);
            productViewModel.SortBy = sortBy;
            //productViewModel.Products = products;
            productViewModel.Sorts = new Dictionary<string, string>
            {
                { "Price low to high", "price_lowest"},
                { "Price high to low", "price_highest"},

            };

            return View(productViewModel);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            var productViewModel = new ProductViewModel();
            productViewModel.CategoryList = new SelectList(db.Categories, "ID", "Name");
            productViewModel.ImageLists = new List<SelectList>();
            for (int i = 0; i < Constants.NumberOfProdcutImages; i++)
            {
                productViewModel.ImageLists.Add(new SelectList(db.ProductImages, "ID", "FileName"));
            }
            return View(productViewModel);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel productViewModel)
        {
            var product = new Product();
            product.Name = productViewModel.Name;
            product.Description = productViewModel.Description;
            product.Price = productViewModel.Price;
            product.CategoryId = productViewModel.CategoryID;
            product.ProductImageMappings = new List<ProductImageMapping>();

            // get a list of all selected images without any blanks
            string[] productImages = productViewModel.ProductImages.Where(pi => !string.IsNullOrEmpty(pi)).ToArray();
            for (int i = 0; i < productImages.Length; i++)
            {
                product.ProductImageMappings.Add(new ProductImageMapping
                {
                    ProductImage = db.ProductImages.Find(int.Parse(productImages[i])),
                    ImageNumber = i
                });
            }
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            productViewModel.CategoryList = new SelectList(db.Categories, "ID", "Name", product.CategoryId);
            productViewModel.ImageLists = new List<SelectList>();
            for (int i = 0; i < Constants.NumberOfProdcutImages; i++)
            {
                productViewModel.ImageLists.Add(new SelectList(db.ProductImages, "ID", "FileName", productViewModel.ProductImages[i]));
            }

            return View(productViewModel);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var productViewModel = new ProductViewModel();
            productViewModel.CategoryList = new SelectList(db.Categories, "ID", "Name", product.CategoryId);
            productViewModel.ImageLists = new List<SelectList>();

            foreach (var item in product.ProductImageMappings.OrderBy(pim => pim.ImageNumber))
            {
                productViewModel.ImageLists.Add(new SelectList(db.ProductImages, "ID", "FileName", item.ProductImageID));
            }

            for (int i = productViewModel.ImageLists.Count; i < Constants.NumberOfProdcutImages; i++)
            {
                productViewModel.ImageLists.Add(new SelectList(db.ProductImages, "ID", "FileName"));
            }

            productViewModel.ID = product.ID;
            productViewModel.Name = product.Name;
            productViewModel.Description = product.Description;
            productViewModel.Price = product.Price;
            
            return View(productViewModel);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,Price,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "ID", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
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
