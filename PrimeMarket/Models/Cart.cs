//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PrimeMarket.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cart
    {
        public decimal CartId { get; set; }
        public Nullable<decimal> CustomerId { get; set; }
        public Nullable<decimal> ShowCommodityId { get; set; }
        public Nullable<System.DateTime> CartDate { get; set; }
        public Nullable<double> Quantity { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual ShowCommodity ShowCommodity { get; set; }
    }
}
