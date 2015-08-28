//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FoodExNepal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FoodCategory
    {
        public FoodCategory()
        {
            this.FoodItems = new HashSet<FoodItem>();
        }
    
        public int FoodCategoryId { get; set; }
        public string FoodCategoryName { get; set; }
        public string FoodCategoryDescription { get; set; }
        public Nullable<int> RestaurantId { get; set; }
    
        public virtual Restaurant Restaurant { get; set; }
        public virtual ICollection<FoodItem> FoodItems { get; set; }
    }
}