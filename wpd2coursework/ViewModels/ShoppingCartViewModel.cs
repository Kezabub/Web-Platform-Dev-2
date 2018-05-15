using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wpd2coursework.Models;

namespace wpd2coursework.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}