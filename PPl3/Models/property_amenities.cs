//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PPl3.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class property_amenities
    {
        public int id { get; set; }
        public Nullable<int> property_id { get; set; }
        public Nullable<int> amenity_id { get; set; }
        public Nullable<System.DateTime> created { get; set; }
        public Nullable<byte> pa_status { get; set; }
        public string pa_description { get; set; }
    
        public virtual amenity amenity { get; set; }
        public virtual property property { get; set; }
    }
}
