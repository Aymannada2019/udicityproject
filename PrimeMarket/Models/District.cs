
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
    
public partial class District
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public District()
    {

        this.Villages = new HashSet<Village>();

    }


    public int DistrictId { get; set; }

    public string District1 { get; set; }

    public Nullable<short> GovernorateId { get; set; }



    public virtual Governorate Governorate { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Village> Villages { get; set; }

}

}
