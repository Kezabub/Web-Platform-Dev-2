using Microsoft.AspNet.Identity;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using wpd2coursework.Models;

namespace wpd2coursework.Controllers
{
        [Authorize]
        public class CheckoutController : Controller
        {

            public ApplicationDbContext db = new ApplicationDbContext();
            public RegisterViewModel register = new RegisterViewModel();

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public ActionResult AddressAndPayment()
            {
                //  ResponseModel.SendSimpleMessage();
                return View();

            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="values"></param>
            /// <returns></returns>
            [HttpPost]
            public ActionResult AddressAndPayment(FormCollection values)
            {
                var order = new CustomerOrder();

                var UserId = User.Identity.GetUserId();

                TryUpdateModel(order);

                order.CustomerUserName = User.Identity.Name;
                order.DateCreated = DateTime.Now;

                db.CustomerOrders.Add(order);
                db.SaveChanges();

                var cart = ShoppingCart.GetCart(this.HttpContext);
                cart.CreateOrder(order);

                db.SaveChanges();//we have received the total amount lets update it

                return RedirectToAction("Complete", new { id = order.Id, email = order.Email });
            }


            [Authorize]
            public ActionResult GenerateInvoice(int? id, string email)
            {
                if (User.IsInRole("Business Customer"))
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
                    ResponseModel.SendSimpleMessage(email);
                    return View(order);
                }

                else
                {
                    return RedirectToAction("Index", "ShoppingCart");
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="id"></param>
            /// <param name="email"></param>
            /// <returns></returns>
            public ActionResult Complete(int id, string email)
            {


                bool isValid = db.CustomerOrders.Any(
                        o => o.Id == id &&
                             o.CustomerUserName == User.Identity.Name
                        );

                if (isValid)
                {


                    ResponseModel.SendSimpleMessage(email);
                    return View(id);
                }
                else
                {
                    var order = new CustomerOrder();
                    order = db.CustomerOrders.Find(id);
                    db.CustomerOrders.Remove(order);
                    return View("Error");
                }
            }
        }
    }

//Response Model makes use of mailgun API to send emails to distributees when there is documents ready for them to view
public class ResponseModel
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public static IRestResponse SendSimpleMessage(string email)
    {
        var order = new CustomerOrder();
        RestClient client = new RestClient();
        client.BaseUrl = new Uri("https://api.mailgun.net/v3");
        client.Authenticator =
                new HttpBasicAuthenticator("api",
                                           "key-f72d70c645350a58d4ce92d0df30c281");
        RestRequest request = new RestRequest();
        request.AddParameter("domain",
                             "sandbox914ac94e81cc46408d1ecd8a6ca9d54d.mailgun.org", ParameterType.UrlSegment);
        request.Resource = "{domain}/messages";
        request.AddParameter("from", "Shop Staff <postmaster@sandbox914ac94e81cc46408d1ecd8a6ca9d54d.mailgun.org>");
        request.AddParameter("to", email);
        request.AddParameter("subject", "Your Order has been placed");
        request.AddParameter("text", "Thank you for placing an order with our shop, we have just begun processing your order. You will recieve a follow up email when your order is ready for collection");
        request.Method = Method.POST;
        return client.Execute(request);
    }
}