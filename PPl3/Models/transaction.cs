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
    
    public partial class transaction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public transaction()
        {
            this.bookings = new HashSet<booking>();
            this.property_reviews = new HashSet<property_reviews>();
        }
    
        public int id { get; set; }
        public Nullable<int> property_id { get; set; }
        public Nullable<int> receiver_id { get; set; }
        public Nullable<int> payer_id { get; set; }
        public Nullable<int> booking_id { get; set; }
        public Nullable<decimal> site_fees { get; set; }
        public Nullable<decimal> amount { get; set; }
        public Nullable<System.DateTime> transfer_on { get; set; }
        public Nullable<int> currency_id { get; set; }
        public Nullable<int> promo_code_id { get; set; }
        public Nullable<decimal> discount_amt { get; set; }
        public Nullable<System.DateTime> created { get; set; }
        public Nullable<System.DateTime> modified { get; set; }
        public Nullable<byte> transaction_status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<booking> bookings { get; set; }
        public virtual booking booking { get; set; }
        public virtual currency currency { get; set; }
        public virtual promo_codes promo_codes { get; set; }
        public virtual property property { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<property_reviews> property_reviews { get; set; }
        public virtual user user { get; set; }
        public virtual user user1 { get; set; }
    }
}
