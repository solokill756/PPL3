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
    
    public partial class favourite
    {
        public int id { get; set; }
        public Nullable<int> userID { get; set; }
        public Nullable<int> property_id { get; set; }
        public Nullable<System.DateTime> added_date { get; set; }
    
        public virtual property property { get; set; }
        public virtual user user { get; set; }
    }
}
