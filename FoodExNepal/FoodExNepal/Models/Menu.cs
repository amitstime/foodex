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
    
    public partial class Menu
    {
        public int MenuId { get; set; }
        public string MenuTitle { get; set; }
        public string PageContent { get; set; }
        public Nullable<bool> hasBanner { get; set; }
        public Nullable<bool> hasRecentMemberListing { get; set; }
        public Nullable<bool> hasCarosol { get; set; }
        public Nullable<bool> isActive { get; set; }
    }
}
