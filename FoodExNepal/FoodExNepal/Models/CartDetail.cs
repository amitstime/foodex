using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodExNepal.Models
{
    public class CartDetail
    {
        public int FoodItemId { get; set;}
        public string FoodItem { get; set; }
        public decimal Rate { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }

    }
}