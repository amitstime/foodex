using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodExNepal.Models
{
    public class Cart
    {
        public List<CartDetail> cartDetail { get; set; }
        public CartSummary cartSummary { get; set; }
        public Restaurant restaurantsess { get; set; }
    }
}