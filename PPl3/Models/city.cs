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
    
    public partial class city
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public city()
        {
            this.properties = new HashSet<property>();
            this.user_personalInfor = new HashSet<user_personalInfor>();
        }
    
        public int id { get; set; }
        public Nullable<int> state_id { get; set; }
        public string city_name { get; set; }
        public Nullable<byte> city_status { get; set; }
    
        public virtual state state { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<property> properties { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<user_personalInfor> user_personalInfor { get; set; }
    }
}
