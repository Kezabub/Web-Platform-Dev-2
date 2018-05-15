using Microsoft.AspNet.Identity;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using wpd2coursework.Models;

namespace wpd2coursework.Controllers
{
    public class OrdersController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        public HttpContext context;

        // GET: Orders
        /// <summary>
        /// Displays the order History of all customers in  system the view is only accessible to staff members in the Sales Assistant or Store Manager Roles
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="currentFilter"></param>
        /// <param name="searchString"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Authorize]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {

            if (User.IsInRole("Sales Assistant"))
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

                if (searchString != null)
                {

                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                var orders = from o in db.CustomerOrders
                             select o;

                if (!String.IsNullOrEmpty(searchString))
                {
                    orders = orders.Where(s => s.FirstName.ToUpper().Contains(searchString.ToUpper())
                                           || s.LastName.ToUpper().Contains(searchString.ToUpper()));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        orders = orders.OrderByDescending(s => s.FirstName);
                        break;
                    case "Price":
                        orders = orders.OrderBy(s => s.Amount);
                        break;
                    case "price_desc":
                        orders = orders.OrderByDescending(s => s.Amount);
                        break;
                    default:  // Name ascending 
                        orders = orders.OrderBy(s => s.FirstName);
                        break;
                }



                int pageSize = 3;
                int pageNumber = (page ?? 1);
                return View(orders.ToPagedList(pageNumber, pageSize));

            }

            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Allows the user to either 
        /// edit all ordes in the system or only thier own orders depending on the role they belong to.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        // GET: Orders/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerOrder order = db.CustomerOrders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        /// <summary>
        /// Posts the resulting changes made to an order back to the database 
        /// as long as validation rules for the model are not violated
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>

        // POST: Orders/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerOrder order)
        {
            if (order != null)
            {

                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(order);
        }

        /// <summary>
        /// Displays the details of an order selected by the user 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerOrder order = db.CustomerOrders.Find(id);
            var orderDetails = db.Orderedproducts.Where(x => x.CustomerOrderId == id);

            order.Products = orderDetails.ToList();
            if (order == null)
            {
                return HttpNotFound();
            }


            return View(order);

        }

        /// <summary>
        /// Finds a specifc order selected by the user to be deleted
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerOrder order = db.CustomerOrders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        /// <summary>
        ///  removes an order from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerOrder order = db.CustomerOrders.Find(id);
            db.CustomerOrders.Remove(order);
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

        [Authorize]
        public ActionResult MyOrders(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {

            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var currentUserId = this.User.Identity.GetUserName();
            var orders = from o in db.CustomerOrders.Where(e => e.CustomerUserName == currentUserId)

                         select o;

            if (!String.IsNullOrEmpty(searchString))
            {

                orders = orders.Where(s => s.FirstName.ToUpper().Contains(searchString.ToUpper())
                                       || s.LastName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    orders = orders.OrderByDescending(s => s.FirstName);
                    break;
                case "Price":
                    orders = orders.OrderBy(s => s.Amount);
                    break;
                case "price_desc":
                    orders = orders.OrderByDescending(s => s.Amount);
                    break;
                default:  // Name ascending 
                    orders = orders.OrderBy(s => s.FirstName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(orders.ToPagedList(pageNumber, pageSize));
        }  
    }

}
