using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace wpd2coursework.Models
{
    public class Category
    {
        /// <summary>int Unique Identifier for an instace of Category</summary>
        [Key]
        [DisplayName("Catagory ID")]
        public int ID { get; set; }

        /// <summary>string Name of Category</summary>
        [DisplayName("Catagory")]
        public string Name { get; set; }

        /// <summary>virtual collection Contains all the products belonging to a specific instance of Category</summary>
        public virtual ICollection<Product> Products { get; set; }
    }
}