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
    
    public partial class cms_pages
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cms_pages()
        {
            this.cms_page_languages = new HashSet<cms_page_languages>();
        }
    
        public int id { get; set; }
        public string unique_name { get; set; }
        public Nullable<System.DateTime> created { get; set; }
        public Nullable<System.DateTime> modified { get; set; }
        public Nullable<byte> cp_status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cms_page_languages> cms_page_languages { get; set; }
    }
}
