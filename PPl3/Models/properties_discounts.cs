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
    
    public partial class properties_discounts
    {
        public int id { get; set; }
        public Nullable<int> properties_id { get; set; }
        public Nullable<int> discounts_id { get; set; }
    
        public virtual discount discount { get; set; }
        public virtual property property { get; set; }
    }
}
