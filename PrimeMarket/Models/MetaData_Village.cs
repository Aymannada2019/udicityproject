using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimeMarket.Models
{
    //[MetadataType(typeof(MetaData_Commodity))]
   
    public partial class Village
    {
       
    //[Display(Name = "المحافظة")]
    //public decimal GovernoratetId { get; set; }
    //public virtual Governorate Governorate { get; set; }

    //[Display(Name = "المركز")]
    //public decimal DistrictId { get; set; }
    //public virtual District District { get; set; }
    [Display(Name = "المحافظة")]
        public decimal GovernoratetId { get; set; }
        public virtual Governorate Governorate { get; set;} 
         

    }
    public class VillageViewModel
    {

        public List<Village> VillageList = new List<Village>();
        public List<Governorate> GovernorateList = new List<Governorate>();
        public PrimeMarketEntities db = new PrimeMarketEntities();
        public VillageViewModel()
        {
            VillageList = db.Villages.Include(c => c.District).Include(g=>g.Governorate).Where(vg=>vg.DistrictId==vg.District.DistrictId && vg.District.GovernorateId==vg.Governorate.GovernorateId).ToList();
        }
    }
}