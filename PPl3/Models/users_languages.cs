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
    
    public partial class users_languages
    {
        public int id { get; set; }
        public Nullable<int> userID { get; set; }
        public Nullable<int> language_id { get; set; }
    
        public virtual listLanguage listLanguage { get; set; }
        public virtual user user { get; set; }
    }
}
