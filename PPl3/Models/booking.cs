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
    
    public partial class booking
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public booking()
        {
            this.booking_guests = new HashSet<booking_guests>();
        }
    
        public int id { get; set; }
        public Nullable<int> property_id { get; set; }
        public Nullable<int> userId { get; set; }
        public Nullable<System.DateTime> check_in_date { get; set; }
        public Nullable<System.DateTime> check_out_date { get; set; }
        public Nullable<decimal> price_per_day { get; set; }
        public Nullable<decimal> amount_paid { get; set; }
        public Nullable<byte> is_refund { get; set; }
        public Nullable<System.DateTime> cancel_date { get; set; }
        public Nullable<decimal> refund_paid { get; set; }
        public Nullable<int> transaction_id { get; set; }
        public Nullable<decimal> effective_amount { get; set; }
        public Nullable<System.DateTime> booking_date { get; set; }
        public Nullable<System.DateTime> created { get; set; }
        public Nullable<byte> pay_status { get; set; }
        public Nullable<byte> booking_status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<booking_guests> booking_guests { get; set; }
        public virtual property property { get; set; }
        public virtual transaction transaction { get; set; }
        public virtual user user { get; set; }
    }
}
