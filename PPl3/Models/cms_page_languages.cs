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
    
    public partial class cms_page_languages
    {
        public int id { get; set; }
        public Nullable<int> cms_page_id { get; set; }
        public Nullable<int> language_id { get; set; }
        public string title { get; set; }
        public string cpl_description { get; set; }
    
        public virtual cms_pages cms_pages { get; set; }
        public virtual language language { get; set; }
    }
}
