﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PPL3Entities : DbContext
    {
        public PPL3Entities()
            : base("name=PPL3Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<amenity> amenities { get; set; }
        public virtual DbSet<booking_guests> booking_guests { get; set; }
        public virtual DbSet<booking> bookings { get; set; }
        public virtual DbSet<category> categories { get; set; }
        public virtual DbSet<city> cities { get; set; }
        public virtual DbSet<cms_page_languages> cms_page_languages { get; set; }
        public virtual DbSet<cms_pages> cms_pages { get; set; }
        public virtual DbSet<component_rating> component_rating { get; set; }
        public virtual DbSet<country> countries { get; set; }
        public virtual DbSet<currency> currencies { get; set; }
        public virtual DbSet<dispute> disputes { get; set; }
        public virtual DbSet<email_content_languages> email_content_languages { get; set; }
        public virtual DbSet<email_contents> email_contents { get; set; }
        public virtual DbSet<favourite> favourites { get; set; }
        public virtual DbSet<governmentID> governmentIDs { get; set; }
        public virtual DbSet<guest_type> guest_type { get; set; }
        public virtual DbSet<host_reviews> host_reviews { get; set; }
        public virtual DbSet<interest_categories> interest_categories { get; set; }
        public virtual DbSet<interest> interests { get; set; }
        public virtual DbSet<language> languages { get; set; }
        public virtual DbSet<listLanguage> listLanguages { get; set; }
        public virtual DbSet<neighbourhood> neighbourhoods { get; set; }
        public virtual DbSet<phone_number> phone_number { get; set; }
        public virtual DbSet<promo_codes> promo_codes { get; set; }
        public virtual DbSet<property> properties { get; set; }
        public virtual DbSet<property_amenities> property_amenities { get; set; }
        public virtual DbSet<property_images> property_images { get; set; }
        public virtual DbSet<property_reviews> property_reviews { get; set; }
        public virtual DbSet<property_type> property_type { get; set; }
        public virtual DbSet<review_component> review_component { get; set; }
        public virtual DbSet<room_type> room_type { get; set; }
        public virtual DbSet<state> states { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<transaction> transactions { get; set; }
        public virtual DbSet<user_personalInfor> user_personalInfor { get; set; }
        public virtual DbSet<user_profile> user_profile { get; set; }
        public virtual DbSet<user_role> user_role { get; set; }
        public virtual DbSet<user_type> user_type { get; set; }
        public virtual DbSet<user_type_user_role> user_type_user_role { get; set; }
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<users_interests> users_interests { get; set; }
        public virtual DbSet<users_languages> users_languages { get; set; }
    }
}
