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
    
    public partial class interest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public interest()
        {
            this.users_interests = new HashSet<users_interests>();
        }
    
        public int id { get; set; }
        public Nullable<int> interest_cate_id { get; set; }
        public string interest_name { get; set; }
        public string icon { get; set; }
        public string itr_description { get; set; }
        public Nullable<byte> itr_status { get; set; }
    
        public virtual interest_categories interest_categories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<users_interests> users_interests { get; set; }
    }
}
