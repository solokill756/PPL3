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
    
    public partial class host_reviews
    {
        public int id { get; set; }
        public Nullable<int> hostid { get; set; }
        public Nullable<int> review_by_user { get; set; }
        public string comment { get; set; }
        public Nullable<System.DateTime> created { get; set; }
        public Nullable<byte> hr_status { get; set; }
        public string rating { get; set; }
    
        public virtual user user { get; set; }
        public virtual user user1 { get; set; }
    }
}
