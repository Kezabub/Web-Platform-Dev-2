using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wpd2coursework.Models;
using wpd2coursework.ViewModels;

namespace wpd2coursework.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// ///   <para>Method:Index- Displays shopping</para>
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            return View(viewModel);
        }


        /// <summary>
        /// ///   <para>Method: AddToCart- Adds a selected product from the product index page to the shopping cart</para>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddToCart(int id)
        {
            var addedProduct = db.Products.Single(product => product.ID == id);

            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedProduct);


            return RedirectToAction("Index");
        }

        /// <summary>
        /// ///   <para>Method: RemoveFromCart- Removes a single instance of a selected product from the shopping cart</para>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            string productName = db.Carts.FirstOrDefault(item => item.ProductId == id).Product.Name;

            int itemCount = cart.RemoveFromCart(id);

            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(productName) + " has been removed from your shopping cart",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

            return Json(results);
        }
        /// <summary>
        /// ///   <para>Method: CartSummary- Displays a price and quantity for each product in the cart as well as displaying the total order cost</para>
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }

    }
}