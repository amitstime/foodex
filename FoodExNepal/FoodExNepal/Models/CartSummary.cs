using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodExNepal.Models
{
    public class CartSummary
    {
        public decimal SubTotal { get; set; }
        public int ServiceChargePercent { get; set; }
        public int VATPercent { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal VAT { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal GrossTotal { get; set; }
    }
}