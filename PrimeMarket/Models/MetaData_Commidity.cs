using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimeMarket.Models
{
    //[MetadataType(typeof(MetaData_Commodity))]
   
    public partial class Commodity
    {
        //[Display(Name = "المحافظة")]
        //public decimal GovernoratetId { get; set; }
        //public virtual Governorate Governorate { get; set; }

        //[Display(Name = "المركز")]
        //public decimal DistrictId { get; set; }
        //public virtual District District { get; set; }
        [Display(Name = "المنتج الرئيسى")]
        public Nullable<decimal> CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}