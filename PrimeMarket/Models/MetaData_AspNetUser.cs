using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimeMarket.Models
{
    [MetadataType(typeof(MetaData_AspNetUser))]
    public partial class AspNetUser
    {
    }
    public class MetaData_AspNetUser
    {
        [Display(Name = "البريد الالكترونى")]
        public string Email { get; set; }

        [Display(Name = "الاسم بالكامل")]
        public string FullName { get; set; }

        [Display(Name = "رقم التليفون")]
        public string PhoneNumber { get; set; }

        [Display(Name = "العنوان")]
        public string Address { get; set; }

        [Display(Name = "صورة الملف الشخصي")]
        public string ImagePath { get; set; }

        [Display(Name = "القرية")]
        public Nullable<decimal> VillageId { get; set; }
    }
}