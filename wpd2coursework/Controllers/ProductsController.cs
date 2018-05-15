using Rotativa;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using wpd2coursework.Models;

namespace wpd2coursework.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <returns></returns>
        // GET: Products
        public ActionResult Index(string searchString, string currentFilter)
        {
            var products = db.Products.Include(p => p.Category);

            if (searchString != null)
            {

            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper())
                                       || s.Category.Name.ToUpper().Contains(searchString.ToUpper()));
            }
            return View(products.ToList());
        }

        public ActionResult WishlistList()
        {
            var products = db.Products.Where(o => o.UserWish == User.Identity.Name);
            return View(products.ToList());
        }

        public ActionResult ExportWishlistToPdf()
        {
            return new ActionAsPdf("WishlistList")
            {
                FileName = Server.MapPath("~/Content/")
            };
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "ID", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CategoryId,Name,Price,Stock,UserWish,DateAdded")] Product product)
        {
            product.UserWish = "none";
            product.DateAdded = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "ID", "Name", product.CategoryId);
            return View(product);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
            ViewBag.CategoryId = new SelectList(db.Categories, "ID", "Name", product.CategoryId);
            return View(product);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CategoryId,Name,Price,Stock")] Product product)
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        [HttpPost]
        public ActionResult AddWishlist(int? id)
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
            try
            {
                var newitem = new Product();
                newitem.Name = product.Name;
                newitem.CategoryId = product.CategoryId;
                newitem.Price = product.Price;
                newitem.Stock = product.Stock;
                newitem.DateAdded = DateTime.Now;
                newitem.UserWish = User.Identity.Name;

                var check = db.Products.Any(o => o.Name == newitem.Name && o.UserWish == User.Identity.Name);

                if (ModelState.IsValid && check == false)
                {
                    db.Products.Add(newitem);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(product);
                }
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        //public ActionResult AddWishlist(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Product product = db.Products.Find(id);
        //    if (product == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(product);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AddWishlist([Bind(Include = "ID,Name,Price,DateAdded,UserWish,Note")] Wishlist wishlist, int? id)
        //{
        //    Product product = db.Products.Find(id);

        //    wishlist.Name = product.Name;
        //    wishlist.Price = product.Price;
        //    wishlist.DateAdded = DateTime.Now;
        //    wishlist.UserWish = User.Identity.Name;

        //    var check = db.Wishlists.Any(o => o.Name == wishlist.Name && o.UserWish == User.Identity.Name);

        //    if (ModelState.IsValid && check == false)
        //    {
        //        db.Wishlists.Add(wishlist);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(product);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
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