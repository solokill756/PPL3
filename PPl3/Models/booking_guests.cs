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
    
    public partial class booking_guests
    {
        public int id { get; set; }
        public Nullable<int> booking_id { get; set; }
        public Nullable<int> guest_type_id { get; set; }
        public Nullable<int> num_guests { get; set; }
    
        public virtual booking booking { get; set; }
        public virtual guest_type guest_type { get; set; }
    }
}
