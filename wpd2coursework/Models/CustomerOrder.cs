using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Web.Mvc;


namespace wpd2coursework.Models
{
    public class CustomerOrder
    {
        /// <summary>int Unique identifier for the order</summary>
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        /// <summary>string First name of the customer</summary>
        [Required(ErrorMessage = "First Name is required")]
        [DisplayName("First Name")]
        [StringLength(160)]
        public string FirstName { get; set; }
        /// <summary>string last name of the customer</summary>
        [Required(ErrorMessage = "Last Name is required")]
        [DisplayName("Last Name")]
        [StringLength(160)]
        public string LastName { get; set; }
        /// <summary>string Address entered for an order</summary>
        [Required(ErrorMessage = "Address is required")]
        [StringLength(70)]
        public string Address { get; set; }
        /// <summary>string City entered for an order</summary>
        [Required(ErrorMessage = "City is required")]
        [StringLength(40)]
        public string City { get; set; }
        /// <summary>string State of residence entered for an order</summary>
        [Required(ErrorMessage = "State is required")]
        [StringLength(40)]
        public string State { get; set; }
        /// <summary>string Postcode for the customer</summary>
        [Required(ErrorMessage = "Postal Code is required")]
        [DisplayName("Postal Code")]
        [StringLength(10)]
        public string PostalCode { get; set; }
        /// <summary>string Country entered for an order</summary>
        [Required(ErrorMessage = "Country is required")]
        [StringLength(40)]
        public string Country { get; set; }
        /// <summary>string phone number for the customer</summary>
        [Required(ErrorMessage = "Phone is required")]
        [StringLength(24)]
        public string Phone { get; set; }
        /// <summary>string Email address of the customer</summary>
        [Required(ErrorMessage = "Email Address is required")]
        [DisplayName("Email Address")]

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
                    ErrorMessage = "Email is is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>DateTime when the order was created</summary>
        [ScaffoldColumn(false)]
        [Column(TypeName = "datetime2")]
        public DateTime DateCreated { get; set; }

        /// <summary>int Total cost of an order</summary>
        [ScaffoldColumn(false)]
        public decimal Amount { get; set; }

        /// <summary>string Username of the customer asociated with the order</summary>
        [ScaffoldColumn(false)]
        public string CustomerUserName { get; set; }

        //[ScaffoldColumn(false)]
        //[Required (ErrorMessage = "Please Include Status Information" )]
        //public string Status { get; set; }
        //[ScaffoldColumn(false)]

        /// <summary>Virtual collection of products specific to the order</summary>
        public List<OrderedProduct> Products { get; set; }

        /// <summary>builds a string which is set to equal the concatenated properties of an order (used in details action result method in OrdersController)</summary>
        public string ToString(CustomerOrder order)
        {
            StringBuilder bob = new StringBuilder();

            bob.Append("<p>Order Information for Order: " + order.Id + "<br>Placed at: " + order.DateCreated + "</p>").AppendLine();
            bob.Append("<p>Name: " + order.FirstName + " " + order.LastName + "<br>");
            bob.Append("Address: " + order.Address + " " + order.City + " " + order.State + " " + order.PostalCode + "<br>");
            bob.Append("Contact: " + order.Email + "     " + order.Phone + "</p>");

            bob.Append("<br>").AppendLine();
            bob.Append("<Table>").AppendLine();
            // Display header 
            string header = "<tr> <th>Item Name</th>" + "<th>Quantity</th>" + "<th>Price</th> <th></th> </tr>";
            bob.Append(header).AppendLine();

            String output = String.Empty;
            try
            {
                foreach (var item in order.Products)
                {
                    bob.Append("<tr>");
                    output = "<td>" + item.Product.Name + "</td>" + "<td>" + item.Quantity + "</td>" + "<td>" + item.Quantity * item.Product.Price + "</td>";
                    bob.Append(output).AppendLine();
                    Console.WriteLine(output);
                    bob.Append("</tr>");
                }
            }
            catch (Exception ex)
            {
                output = "No items ordered.";
            }
            bob.Append("</Table>");
            bob.Append("<b>");
            // Display footer 
            string footer = String.Format("{0,-12}{1,12}\n",
                                          "Total", order.Amount);
            bob.Append(footer).AppendLine();
            bob.Append("</b>");

            return bob.ToString();
        }
    }
}