using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PrimeMarket.Models
{[MetadataType(typeof(CommodityMetaData))]
    public partial class Commodity
    {
    }

    public class CommodityMetaData
    {
        [Required(ErrorMessage = "حدد التصنيف الفرعى")]
        [Range(1,1000000)]
        public Nullable<decimal> SubCategoryId { get; set; }

        [Required( ErrorMessage ="أدخل اسم المنتج")]
        [StringLength(250,MinimumLength =3,ErrorMessage ="اسم المنتج يجب أن لا يقل عن ثلاثة أحرف و لا يزيد عن 250 حرفاً ")]
        public string Title { get; set; }

        [Required(ErrorMessage = "أدخل الكمية المتاحة")]
        [RegularExpression(@"(\d*)+\.?(\d*)+",ErrorMessage ="الكمية يجب أن لا تحتوى على حروف")]
        public Nullable<double> Quantity { get; set; }

        [Display(Name = "سعر الوحدة بالجنيه")]
        [Required(ErrorMessage = "أدخل السعر")]
        [RegularExpression(@"(\d*)+\.?(\d*)+", ErrorMessage = "السعر يجب أن لا يحتوى على حروف")]
        public Nullable<double> Price { get; set; }

        [Display(Name = "سلعة مميزة")]
        public bool IsFeatured { get; set; }

        [Display(Name = "السعر الأصلى للوحدة بالجنيه")]
        [RegularExpression(@"(\d*)+\.?(\d*)+", ErrorMessage = "السعر يجب أن لا يحتوى على حروف")]
        public Nullable<double> OriginalPrice { get; set; }
    }
}