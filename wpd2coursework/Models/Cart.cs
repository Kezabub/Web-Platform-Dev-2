using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace wpd2coursework.Models
{
    public class Cart
    {
        /// <summary>Unique Identifier of type int for cart</summary>
        [Key]
        public int Id { get; set; }

        /// <summary>string variable used to hold a session key which makes each instance of the cart unique to a user</summary>
        public string CartId { get; set; }

        /// <summary>Variable of type int that allows cart to Identify each product that is added</summary>
        public int ProductId { get; set; }
        /// <summary>The  number of products within each instance of cart</summary>
        public int Count { get; set; }

        /// <summary>DateTime of when the cart was created</summary>
        [Column(TypeName = "datetime2")]
        public DateTime DateCreated { get; set; }

        /// <summary>public virtual collection which gives cart access to the relevant information for each product it contains</summary>
        public virtual Product Product { get; set; }
        /// <summary>public virtual collection used to give the cart the access necessary to add products to an order and add that order to the database</summary>
        public virtual CustomerOrder order { get; set; }
    }
}