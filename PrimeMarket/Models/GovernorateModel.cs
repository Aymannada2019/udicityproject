using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrimeMarket.Models
{
    public class GovernorateModel
    {
        public List<Governorate> Governorates { get; set; }
        public int CurrentPageIndex { get; set; }
        ///<summary>
        /// Gets or sets PageCount.
        ///</summary>
        public int PageCount { get; set; }
    }
}