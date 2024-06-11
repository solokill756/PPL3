using PPl3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PPl3.App_Start;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Serialization;
using WebGrease.Css.Ast;

namespace PPl3.Areas.Host.Controllers
{
    public class HomeHostController : Controller
    {

       public class MyModel
        {

            public string property_type {  get; set; }
            public string room_type { get; set; }
            public string Country { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            public string StreetAddress { get; set; }
            public string img_1 { get; set; }
            public string img_2 { get; set; }
            public string img_3 { get; set; }
            public string img_4 { get; set; }
            public string img_5 { get; set; }
            public string number_guest { get; set; }
            public string number_bedrooms { get; set; }

            public string number_bathrooms{ get; set; }
            public string number_beds { get; set; }
            public string number_pets { get; set; }

            public string[] list_main_amentities { get; set; }

            public string[] list_other_amentities { get; set; }
            public string[] listHighlight { get; set; }

            public string hotel_name { get; set; }

            public string hotelDescribe { get; set; }

            public string ask_for_booking { get; set; }

            public string price { get; set; }

            public string[] listDiscounts { get; set; }

            public string check_in { get; set; }
            public string check_out { get; set; }
           
        }





        // GET: Host/HomeHost
        public ActionResult HostIndex()
        {
            return View();
        }

        public ActionResult Listing()
        {
            if (Session["user"] != null)
            {
                user p_user = (user)Session["user"];
                PPL3Entities db = new PPL3Entities();
                if (p_user.user_type == 3) {
                    
                    List<property> properties = db.properties.Where(item => item.userId == p_user.id).ToList();
                    return View(properties);
                }
                else if(checkHost(p_user))
                {
                    if(!db.browser_becomes_host.Any(item => item.user_id == p_user.id))
                    {
                        user_notification user_Notification = new user_notification();
                        user_Notification.userid = p_user.id;
                        user_Notification.created = DateTime.Now;
                        user_Notification.content = "Your request to become a host has been sent successfully";
                        user_Notification.un_status = 1;
                        user_Notification.un_url = "#";
                        db.user_notification.Add(user_Notification);
                        user_notification admin_notification = new user_notification();
                        admin_notification.userid = 20;
                        admin_notification.created = DateTime.Now;
                        admin_notification.content = "The user with id " + p_user.id + " requests to become a host";
                        admin_notification.un_status = 0;
                        admin_notification.un_url = "#";
                        db.user_notification.Add(admin_notification);
                        db.SaveChanges();
                        browser_becomes_host browser_Becomes_Host = new browser_becomes_host();
                        browser_Becomes_Host.user_id = p_user.id;
                        browser_Becomes_Host.Date = DateTime.Now;
                        
                        db.browser_becomes_host.Add(browser_Becomes_Host);
                        db.SaveChanges();
                    }
                    else
                    {
                        user_notification user_Notification = new user_notification();
                        user_Notification.userid = p_user.id;
                        user_Notification.created = DateTime.Now;
                        user_Notification.content = "You have already sent a request to become a host";
                        user_Notification.un_status = 1;
                        user_Notification.un_url = "#";
                        db.user_notification.Add(user_Notification);
                        db.SaveChanges();
                    }
                    
                    return RedirectToAction("notification", "homeuser", new { area = "User" });
                }
                else
                {
                    
                    return RedirectToAction("userPersonalProfile", "homeuser", new { area = "user" , error = true});
                }

            }
            else return RedirectToAction("Login", "homeuser", new { area = "User" });

        }
        [HostAuthorize(idChucNang = 7)]
        public ActionResult revenue()
        {
            if (Session["user"] != null)
            {
                user p_user = (user)Session["user"];
                if (p_user.user_type == 3)
                {
                    PPL3Entities db = new PPL3Entities();
                    List<property> properties = db.properties.Where(item => item.userId == p_user.id).ToList();
                    return View(properties);
                }
                
                else
                {

                    return RedirectToAction("userPersonalProfile", "homeuser", new { area = "user", error = true });
                }

            }
            else return RedirectToAction("Login", "homeuser", new { area = "User" });
        }

        [HttpPost]
        [HostAuthorize(idChucNang = 2)]
        public JsonResult DeleteListing(int id)
        {
            user p_user = (user)Session["user"];
            PPL3Entities db = new PPL3Entities();
            property find_hotel = db.properties.FirstOrDefault(item => item.id == id);
            db.properties.Remove(find_hotel);
            db.SaveChanges();
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchListing(string text)
        {
            if (Session["user"] != null)
            {
                user p_user = (user)Session["user"];
                if (p_user.user_type == 3)
                {
                    PPL3Entities db = new PPL3Entities();
                    // Select only the required properties instead of the full entity
                    var data = (from item in db.properties
                                where item.userId == p_user.id && item.p_name.Contains(text)
                                select new { item.id, item.p_name, item.p_address, item.created, item.room_type.rt_name, item.property_images.FirstOrDefault().pi_image }).ToList();
                    return Json(new
                    {
                        list_pro = data,
                        success = true,
                        count = data.Count
                    }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        redirectUrl = Url.Action("Index", "Homeuser", new { area = "user" })
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            else return Json(new
            {
                success = false,
                redirectUrl = Url.Action("Index", "Homeuser", new { area = "user" })
            }, JsonRequestBehavior.AllowGet);
        }

        [HostAuthorize(idChucNang = 3)]
        public ActionResult addHotel()
        {
            return View();
        }


        [HttpPost]
        public JsonResult addHotel(MyModel data , string page) 
        {
            int current_pages = int.Parse(page);
            Console.WriteLine(data);
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
            property new_hotel = new property();

            new_hotel.p_name = data.hotel_name;
            new_hotel.p_description = data.hotelDescribe;
            new_hotel.userId = p_user.id;
            user_notification user_Notification = new user_notification();
            user_Notification.userid = p_user.id;
            user_Notification.created = DateTime.Now;
            user_Notification.content = "The addition of the new hotel was successful. Please check the listing.";
            user_Notification.un_status = 1;
            user_Notification.un_url = "/host/homehost/listing";
            db.user_notification.Add(user_Notification);
            Console.WriteLine(data.property_type);
            if (data.property_type != null)  new_hotel.property_type_id = int.Parse(data.property_type);
            if(data.room_type != null)   new_hotel.room_type_id = int.Parse(data.room_type);
            if(data.Country != null)
            {
                if (db.countries.Any(item => item.ct_name.Trim().ToLower() == data.Country.Trim().ToLower()))
                {
                    new_hotel.country_id = db.countries.FirstOrDefault(item => item.ct_name.Trim().ToLower() == data.Country.Trim().ToLower()).id;

                }
                else
                {
                    country new_country = new country();
                    new_country.ct_name = data.Country.Trim();
                    new_country.status = 1;
                    db.countries.Add(new_country);
                    db.SaveChanges();

                    new_hotel.country_id = new_country.id;
                }
            }


            if (data.State != null)
            {
                if (db.states.Any(item => item.state_name.Trim().ToLower() == data.State.Trim().ToLower()))
                {
                    new_hotel.state_id = db.states.FirstOrDefault(item => item.state_name.Trim().ToLower() == data.State.Trim().ToLower()).id;

                }
                else
                {
                    state new_state = new state();
                    new_state.state_name = data.State.Trim();
                    new_state.state_status = 1;
                    new_state.country_id = new_hotel.country_id;
                    db.states.Add(new_state);
                    db.SaveChanges();

                    new_hotel.state_id = new_state.id;
                }
            }



            if (data.City != null)
            {
                if (db.cities.Any(item => item.city_name.Trim().ToLower() == data.City.Trim().ToLower()))
                {
                    new_hotel.city_id = db.cities.FirstOrDefault(item => item.city_name.Trim().ToLower() == data.City.Trim().ToLower()).id;

                }
                else
                {
                    city new_City = new city();
                    new_City.city_name = data.City.Trim();
                    new_City.city_status = 1;
                    new_City.state_id = new_hotel.state_id;
                    db.cities.Add(new_City);
                    db.SaveChanges();

                    new_hotel.city_id = new_City.id;
                }
            }

            new_hotel.p_address = data.StreetAddress;
            new_hotel.bedroom_count = data.number_bedrooms;
            new_hotel.bathroom_count = data.number_bathrooms;
            new_hotel.accomodates_count = data.number_guest;
            new_hotel.max_pets = data.number_pets;
            new_hotel.bed_count = data.number_beds;
            if (current_pages >= 12)
            {
                new_hotel.p_status = 0;
                new_hotel.current_pages = 1;

            }
            else
            {
                new_hotel.p_status = 0;
                new_hotel.current_pages = current_pages;
            }
            new_hotel.price = Decimal.Parse(data.price);
            new_hotel.currency_id = 1;
            new_hotel.created = DateTime.Now;
           if(data.ask_for_booking != null) new_hotel.ask_for_booking = byte.Parse(data.ask_for_booking);
           if(data.check_in != null) new_hotel.startDate = DateTime.Parse(data.check_in);
           if(data.check_out != null) new_hotel.end_date = DateTime.Parse(data.check_out);
           if(new_hotel.end_date != null && new_hotel.end_date >= DateTime.Now)
            { 
                new_hotel.availability_type = 1; 
            }
            else
            {
                new_hotel.availability_type = 0;
            }
            db.properties.Add(new_hotel);
            db.SaveChanges();
            if (!db.Browse_hotel_listings.Any(item => item.property_id == new_hotel.id) && current_pages >= 12)
            {
                user_Notification = new user_notification();
                user_Notification.userid = p_user.id;
                user_Notification.created = DateTime.Now;
                user_Notification.content = "The room approval request has been sent, please wait a moment";
                user_Notification.un_status = 0;
                user_Notification.un_url = "#";
                db.user_notification.Add(user_Notification);
                user_notification admin_notification = new user_notification();
                admin_notification.userid = 20;
                admin_notification.created = DateTime.Now;
                admin_notification.content = "Hotels with id " + new_hotel.id + " need to be approved";
                admin_notification.un_status = 0;
                admin_notification.un_url = "#";
                db.user_notification.Add(admin_notification);
                Browse_hotel_listings browse_Hotel_ = new Browse_hotel_listings();
                browse_Hotel_.property_id = new_hotel.id;
                
                browse_Hotel_.Date = DateTime.Now;
                db.Browse_hotel_listings.Add(browse_Hotel_);
                db.SaveChanges();

            }
            if (data.listDiscounts != null) {
                
                for (int i = 0; i < data.listDiscounts.Length; ++i)
                {
                    properties_discounts new_discounts = new properties_discounts();
                    new_discounts.properties_id = new_hotel.id;
                    new_discounts.discounts_id = db.discounts.FirstOrDefault(item => item.id == i + 1).id;
                    new_discounts.discounts_value = decimal.Parse(data.listDiscounts[i].Substring(0 , data.listDiscounts[i].Length - 1));
                    db.properties_discounts.Add(new_discounts);
                    db.SaveChanges();
                }

            }



            if (data.list_other_amentities != null)
            {
                foreach (var item in data.list_other_amentities)
                {
                    property_amenities new_conection = new property_amenities();
                    new_conection.pa_status = 1;
                    new_conection.property_id = new_hotel.id;
                    new_conection.amenity_id = int.Parse(item);
                    db.property_amenities.Add(new_conection);
                    db.SaveChanges();
                }
            }
            if(data.list_main_amentities != null)
            {
                foreach (var item in data.list_main_amentities)
                {
                    property_amenities new_conection = new property_amenities();
                    new_conection.pa_status = 1;
                    new_conection.property_id = new_hotel.id;
                    new_conection.amenity_id = int.Parse(item);
                    db.property_amenities.Add(new_conection);
                    db.SaveChanges();
                }
            }

            if(data.listHighlight != null)
            {
                foreach (var item in data.listHighlight)
                {
                    property_amenities new_conection = new property_amenities();
                    new_conection.pa_status = 1;
                    new_conection.property_id = new_hotel.id;
                    new_conection.amenity_id = int.Parse(item);
                    db.property_amenities.Add(new_conection);
                    db.SaveChanges();
                }
            }


            

            if(data.img_1 != null)
            {
                property_images new_image = new property_images();
                new_image.pi_status = 1;
                new_image.property_id = new_hotel.id;
                new_image.added_by_user = p_user.id;
                new_image.created = DateTime.Now;
                new_image.pi_image = data.img_1;
                db.property_images.Add(new_image);
                db.SaveChanges();
            }


           


            if (data.img_2 != null)
            {
                property_images new_image = new property_images();
                new_image.pi_status = 1;
                new_image.property_id = new_hotel.id;
                new_image.added_by_user = p_user.id;
                new_image.created = DateTime.Now;
                new_image.pi_image = data.img_2;
                db.property_images.Add(new_image);
                db.SaveChanges();
            }


            if (data.img_3 != null)
            {
                property_images new_image = new property_images();
                new_image.pi_status = 1;
                new_image.property_id = new_hotel.id;
                new_image.added_by_user = p_user.id;
                new_image.created = DateTime.Now;
                new_image.pi_image = data.img_3;
                db.property_images.Add(new_image);
                db.SaveChanges();
            }


            if (data.img_4 != null)
            {
                property_images new_image = new property_images();
                new_image.pi_status = 1;
                new_image.property_id = new_hotel.id;
                new_image.added_by_user = p_user.id;
                new_image.created = DateTime.Now;
                new_image.pi_image = data.img_4;
                db.property_images.Add(new_image);
                db.SaveChanges();
            }


            if (data.img_5 != null)
            {
                property_images new_image = new property_images();
                new_image.pi_status = 1;
                new_image.property_id = new_hotel.id;
                new_image.added_by_user = p_user.id;
                new_image.created = DateTime.Now;
                new_image.pi_image = data.img_5;
                db.property_images.Add(new_image);
                db.SaveChanges();
            }

            return Json(new
            {
                success = "true",
                redirectUrl = Url.Action("listing", "Homehost", new { area = "Host" })
            }, JsonRequestBehavior.AllowGet);
        }

        [HostAuthorize(idChucNang = 5)]
        public ActionResult editHotel(int id)
        {
            PPL3Entities db = new PPL3Entities();
            property find_hotel = db.properties.FirstOrDefault(item => item.id == id);
            return View(find_hotel);
        }


        [HttpPost]

        public JsonResult editHotel(MyModel data, int id , int current_page)
        {
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
            property find_hotel = db.properties.FirstOrDefault(item => item.id == id);

            find_hotel.p_name = data.hotel_name;
            find_hotel.p_description = data.hotelDescribe;
            find_hotel.userId = p_user.id;
            if(data.property_type != null)  find_hotel.property_type_id = int.Parse(data.property_type);
            if (data.room_type != null)  find_hotel.room_type_id = int.Parse(data.room_type);
            if (data.Country != null)
            {
                if (db.countries.Any(item => item.ct_name.Trim().ToLower() == data.Country.Trim().ToLower()))
                {
                    find_hotel.country_id = db.countries.FirstOrDefault(item => item.ct_name.Trim().ToLower() == data.Country.Trim().ToLower()).id;

                }
                else
                {
                    country new_country = new country();
                    new_country.ct_name = data.Country.Trim();
                    new_country.status = 1;
                    db.countries.Add(new_country);
                    db.SaveChanges();

                    find_hotel.country_id = new_country.id;
                }
            }


            if (data.State != null)
            {
                if (db.states.Any(item => item.state_name.Trim().ToLower() == data.State.Trim().ToLower()))
                {
                    find_hotel.state_id = db.states.FirstOrDefault(item => item.state_name.Trim().ToLower() == data.State.Trim().ToLower()).id;

                }
                else
                {
                    state new_state = new state();
                    new_state.state_name = data.State.Trim();
                    new_state.state_status = 1;
                    new_state.country_id = find_hotel.country_id;
                    db.states.Add(new_state);
                    db.SaveChanges();

                    find_hotel.state_id = new_state.id;
                }
            }



            if (data.City != null)
            {
                if (db.cities.Any(item => item.city_name.Trim().ToLower() == data.City.Trim().ToLower()))
                {
                    find_hotel.city_id = db.cities.FirstOrDefault(item => item.city_name.Trim().ToLower() == data.City.Trim().ToLower()).id;

                }
                else
                {
                    city new_City = new city();
                    new_City.city_name = data.City.Trim();
                    new_City.city_status = 1;
                    new_City.state_id = find_hotel.state_id;
                    db.cities.Add(new_City);
                    db.SaveChanges();

                    find_hotel.city_id = new_City.id;
                }
            }

            find_hotel.p_address = data.StreetAddress;
            find_hotel.bedroom_count = data.number_bedrooms;
            find_hotel.bathroom_count = data.number_bathrooms;
            find_hotel.accomodates_count = data.number_guest;
            find_hotel.max_pets = data.number_pets;
            find_hotel.bed_count = data.number_beds;
            if(current_page >= 12)
            {
                if(!db.Browse_hotel_listings.Any(item => item.property_id == find_hotel.id))
                {
                    user_notification user_Notification = new user_notification();
                    user_Notification.userid = p_user.id;
                    user_Notification.created = DateTime.Now;
                    user_Notification.content = "The room approval request has been sent, please wait a moment";
                    user_Notification.un_status = 0;
                    user_Notification.un_url = "#";
                    db.user_notification.Add(user_Notification);
                    user_notification admin_notification = new user_notification();
                    admin_notification.userid = 20;
                    admin_notification.created = DateTime.Now;
                    admin_notification.content = "Hotels with id " + find_hotel.id + " need to be approved";
                    admin_notification.un_status = 0;
                    admin_notification.un_url = "#";
                    db.user_notification.Add(admin_notification);
                    Browse_hotel_listings browse_Hotel_ = new Browse_hotel_listings();
                    browse_Hotel_.property_id = find_hotel.id;
                    browse_Hotel_.Date = DateTime.Now;
                    db.Browse_hotel_listings.Add(browse_Hotel_);
                    db.SaveChanges();
                }
                find_hotel.current_pages = 1;
            }
            else if(find_hotel.p_status == 1)
            {
                find_hotel.current_pages = current_page;
            }
            else
            {
                find_hotel.p_status = 0;
                find_hotel.current_pages = current_page;
            }
            if (data.price != null)
            {
                find_hotel.price = decimal.Parse(data.price);
            }
            find_hotel.currency_id = 1;
            if (data.ask_for_booking != null) find_hotel.ask_for_booking = byte.Parse(data.ask_for_booking);
            if (data.check_in != null) find_hotel.startDate = DateTime.Parse(data.check_in);
            if (data.check_out != null)  find_hotel.end_date = DateTime.Parse(data.check_out);
            if (find_hotel.end_date != null && find_hotel.end_date >= DateTime.Now)
            {
                find_hotel.availability_type = 1;
            }
            else
            {
                find_hotel.availability_type = 0;
            }
            db.SaveChanges();

            if (data.listDiscounts != null)
            {

                for (int i = 0; i < data.listDiscounts.Length; ++i)
                {
                    properties_discounts new_discounts = db.properties_discounts.FirstOrDefault(item => item.discounts_id == i + 1 && item.properties_id == id);
                    new_discounts.discounts_value = decimal.Parse(data.listDiscounts[i].Substring(0, data.listDiscounts[i].Length - 1));
                    db.SaveChanges();
                }

            }

            while (db.property_amenities.Any(item => item.property_id == id))
            {
                property_amenities find_amennity = db.property_amenities.FirstOrDefault(item => item.property_id == id);
                db.property_amenities.Remove(find_amennity);
                db.SaveChanges();
            }

            if (data.list_other_amentities != null)
            {
               
                foreach (var item in data.list_other_amentities)
                {
                    property_amenities new_conection = new property_amenities();
                    new_conection.pa_status = 1;
                    new_conection.property_id = find_hotel.id;
                    new_conection.amenity_id = int.Parse(item);
                    db.property_amenities.Add(new_conection);
                    db.SaveChanges();
                }
            }
            if (data.list_main_amentities != null)
            {

                foreach (var item in data.list_main_amentities)
                {
                    property_amenities new_conection = new property_amenities();
                    new_conection.pa_status = 1;
                    new_conection.property_id = find_hotel.id;
                    new_conection.amenity_id = int.Parse(item);
                    db.property_amenities.Add(new_conection);
                    db.SaveChanges();
                }
            }

            if (data.listHighlight != null)
            {
                foreach (var item in data.listHighlight)
                {
                    property_amenities new_conection = new property_amenities();
                    new_conection.pa_status = 1;
                    new_conection.property_id = find_hotel.id;
                    new_conection.amenity_id = int.Parse(item);
                    db.property_amenities.Add(new_conection);
                    db.SaveChanges();
                }
            }


            property_images[] list_imgs = db.property_images.Where(item => item.property_id == id).ToArray();
            if(list_imgs.Length > 0)
            {
                int length = list_imgs.Length;
                if (data.img_1 != null)
                {
                    list_imgs[0].created = DateTime.Now;
                    list_imgs[0].pi_image = data.img_1;
                    db.SaveChanges();
                }


                if (data.img_2 != null)
                {
                    if(length > 1)
                    {
                        list_imgs[1].created = DateTime.Now;
                        list_imgs[1].pi_image = data.img_2;
                        db.SaveChanges();
                    }
                    else
                    {
                        property_images new_image = new property_images();
                        new_image.pi_status = 1;
                        new_image.property_id = find_hotel.id;
                        new_image.added_by_user = p_user.id;
                        new_image.created = DateTime.Now;
                        new_image.pi_image = data.img_2;
                        db.property_images.Add(new_image);
                        db.SaveChanges();
                    }
                   
                    
                }


                if (data.img_3 != null)
                {
                    if (length > 2)
                    {
                        list_imgs[2].created = DateTime.Now;
                        list_imgs[2].pi_image = data.img_3;
                        db.SaveChanges();
                    }
                    else
                    {
                        property_images new_image = new property_images();
                        new_image.pi_status = 1;
                        new_image.property_id = find_hotel.id;
                        new_image.added_by_user = p_user.id;
                        new_image.created = DateTime.Now;
                        new_image.pi_image = data.img_3;
                        db.property_images.Add(new_image);
                        db.SaveChanges();
                    }
                }


                if (data.img_4 != null)
                {
                    if (length > 3)
                    {
                        list_imgs[3].created = DateTime.Now;
                        list_imgs[3].pi_image = data.img_4;
                        db.SaveChanges();
                    }
                    else
                    {
                        property_images new_image = new property_images();
                        new_image.pi_status = 1;
                        new_image.property_id = find_hotel.id;
                        new_image.added_by_user = p_user.id;
                        new_image.created = DateTime.Now;
                        new_image.pi_image = data.img_4;
                        db.property_images.Add(new_image);
                        db.SaveChanges();
                    }
                }


                if (data.img_5 != null)
                {
                    if (length > 4)
                    {
                        list_imgs[4].created = DateTime.Now;
                        list_imgs[4].pi_image = data.img_5;
                        db.SaveChanges();
                    }
                    else
                    {
                        property_images new_image = new property_images();
                        new_image.pi_status = 1;
                        new_image.property_id = find_hotel.id;
                        new_image.added_by_user = p_user.id;
                        new_image.created = DateTime.Now;
                        new_image.pi_image = data.img_5;
                        db.property_images.Add(new_image);
                        db.SaveChanges();
                    }
                }

            }

            else
            {
                if (data.img_1 != null)
                {
                    property_images new_image = new property_images();
                    new_image.pi_status = 1;
                    new_image.property_id = find_hotel.id;
                    new_image.added_by_user = p_user.id;
                    new_image.created = DateTime.Now;
                    new_image.pi_image = data.img_1;
                    db.property_images.Add(new_image);
                    db.SaveChanges();
                }





                if (data.img_2 != null)
                {
                    property_images new_image = new property_images();
                    new_image.pi_status = 1;
                    new_image.property_id = find_hotel.id;
                    new_image.added_by_user = p_user.id;
                    new_image.created = DateTime.Now;
                    new_image.pi_image = data.img_2;
                    db.property_images.Add(new_image);
                    db.SaveChanges();
                }


                if (data.img_3 != null)
                {
                    property_images new_image = new property_images();
                    new_image.pi_status = 1;
                    new_image.property_id = find_hotel.id;
                    new_image.added_by_user = p_user.id;
                    new_image.created = DateTime.Now;
                    new_image.pi_image = data.img_3;
                    db.property_images.Add(new_image);
                    db.SaveChanges();
                }


                if (data.img_4 != null)
                {
                    property_images new_image = new property_images();
                    new_image.pi_status = 1;
                    new_image.property_id = find_hotel.id;
                    new_image.added_by_user = p_user.id;
                    new_image.created = DateTime.Now;
                    new_image.pi_image = data.img_4;
                    db.property_images.Add(new_image);
                    db.SaveChanges();
                }


                if (data.img_5 != null)
                {
                    property_images new_image = new property_images();
                    new_image.pi_status = 1;
                    new_image.property_id = find_hotel.id;
                    new_image.added_by_user = p_user.id;
                    new_image.created = DateTime.Now;
                    new_image.pi_image = data.img_5;
                    db.property_images.Add(new_image);
                    db.SaveChanges();
                }
            }

            return Json(new
            {
                success = "true",
                redirectUrl = Url.Action("listing", "Homehost", new { area = "Host" })
            }, JsonRequestBehavior.AllowGet);
        }
        //r o 
        public class GetMoneyModel
        {
            public int month {  get; set; }
            public int year {  get; set; }
            public double money {  get; set; }
        }
        [HttpGet]
        public JsonResult GetListMoneyin12Month()
        {
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
            List<GetMoneyModel> data = new List<GetMoneyModel>();
            DateTime currentDate = DateTime.Now;
            string currentMonth = currentDate.ToString("MM");
            string currentYear = currentDate.ToString("yyyy");
            int firstMonth = int.Parse(currentMonth);
            for(int i = 0; i < 6; i++)
            {
                firstMonth--;
                if(firstMonth == 0)
                {
                    firstMonth = 12;
                }
            }

            for(int i = 0; i < 12; i++)
            {
                int tmpYear;
                if (firstMonth > int.Parse(currentMonth))
                {
                    tmpYear = int.Parse(currentYear) - 1;
                }
                else
                {
                    tmpYear = int.Parse(currentYear);
                }
                GetMoneyModel temp = new GetMoneyModel()
                {
                    month = firstMonth,
                    year = tmpYear,
                    money = 0,
                };
                data.Add(temp);
                firstMonth++;
                if (firstMonth > 12) firstMonth = 1;
            }

            foreach (var item in db.transactions.Where(tmp => tmp.receiver_id == p_user.id).ToList())
            {
                for(int i = 0; i < 12; i++)
                {
                    if (data[i].month == int.Parse(item.transfer_on.Value.ToString("MM")) && data[i].year == int.Parse(item.transfer_on.Value.ToString("yyyy")))
                    {
                        data[i].money += (double)item.amount;
                        db.transactions.Remove(item);
                    }
                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public class RevenueListData
        {
            public int year { get; set; }
            public double money { get; set; }
            public int renters { get; set; }

        }
        public class RevenueListData1
        {
            public int month { get; set; }
            public double money { get; set; }
            public int renters { get; set; }

        }
        [HttpGet]
        public JsonResult GetListDataForRevenue()
        {
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
            List<RevenueListData> data = new List<RevenueListData>();
            List<RevenueListData1> data1 = new List<RevenueListData1>();
            DateTime currentDate = DateTime.Now;
            string currentMonth = currentDate.ToString("MM");
            string currentYear = currentDate.ToString("yyyy");
            double monthRevenue = 0;
            double totalRevenue = 0;
            int monthRenter = 0;
            int totalRenter = 0;
            for(int i = 0; i < 12; i++)
            {
                RevenueListData1 revenueListData1 = new RevenueListData1()
                {
                    month = i + 1,
                    money = 0,
                    renters = 0,
                };
                data1.Add(revenueListData1);
            }
            for(int i = int.Parse(currentYear) - 6; i <= int.Parse(currentYear); i++)
            {
                RevenueListData revenueListData = new RevenueListData()
                {
                    year = i,
                    money = 0,
                    renters = 0,
                };
                data.Add(revenueListData);
            }
            int mocYear = int.Parse(currentYear) - 6;
            foreach (var item in db.transactions.Where(tmp => tmp.receiver_id == p_user.id).ToList())
            {

                data1[int.Parse(item.transfer_on.Value.ToString("MM")) - 1].money = (double)item.amount;
                data1[int.Parse(item.transfer_on.Value.ToString("MM")) - 1].renters++;

                if(int.Parse(item.transfer_on.Value.ToString("yyyy")) >= mocYear)
                {
                    data[int.Parse(item.transfer_on.Value.ToString("yyyy")) - mocYear].money = (double)item.amount;
                    data[int.Parse(item.transfer_on.Value.ToString("yyyy")) - mocYear].renters++;
                }

                if(int.Parse(item.transfer_on.Value.ToString("MM")) == int.Parse(currentMonth) && int.Parse(item.transfer_on.Value.ToString("yyyy")) == int.Parse(currentYear))
                {
                    monthRenter++;
                    monthRevenue += (double)item.amount;
                }
                totalRenter++;
                totalRevenue += (double)item.amount;
            }
            var result = new
            {
                Data = data,
                Data1 = data1,
                MonthRenter = monthRenter,
                MonthRevenue = monthRevenue,
                TotalRenter = totalRenter,
                TotalRevenue = totalRevenue,
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // Fuction

        public bool checkHost(user p_user)
        {

            if (p_user.user_personalInfor.FirstOrDefault().legal_name == null)
            {
                return false;
            }
            if (p_user.user_personalInfor.FirstOrDefault().email_address == null)
            {
                return false;
            }
            //if (p_user.user_personalInfor.FirstOrDefault().facebook_id == null) { return false; }
            if (p_user.user_personalInfor.FirstOrDefault().country_id == null) { return false; }
            if (p_user.user_personalInfor.FirstOrDefault().u_state == null) { return false; }
            if (p_user.user_personalInfor.FirstOrDefault().u_city == null) { return false; }
            if (p_user.phone_number.FirstOrDefault().phone == null) { return false; }
            if (p_user.user_personalInfor.FirstOrDefault().first_name == null || p_user.user_personalInfor.FirstOrDefault().first_name.Trim() == "") { return false; }
            if (p_user.user_personalInfor.FirstOrDefault().last_name == null || p_user.user_personalInfor.FirstOrDefault().last_name.Trim() == "") { return false; }
            if(p_user.governmentIDs.FirstOrDefault().identity_card == null) { return false; }
            if (p_user.governmentIDs.FirstOrDefault().date_range == null) { return false; }
            if (p_user.governmentIDs.FirstOrDefault().expiration_date == null) { return false; }
            if (p_user.governmentIDs.FirstOrDefault().number_card == null) { return false; }
            if (p_user.governmentIDs.FirstOrDefault().backof_id_card == null) { return false; }
            if(p_user.emergency_contact.FirstOrDefault().ec_name == null || p_user.emergency_contact.FirstOrDefault().ec_name.Trim() == "") { return false; }
            if (p_user.emergency_contact.FirstOrDefault().relationship == null || p_user.emergency_contact.FirstOrDefault().relationship.Trim() == "") { return false; }
            if (p_user.emergency_contact.FirstOrDefault().ec_language == null || p_user.emergency_contact.FirstOrDefault().ec_language.Trim() == "") { return false; }
            if (p_user.emergency_contact.FirstOrDefault().email == null || p_user.emergency_contact.FirstOrDefault().email.Trim() == "") { return false; }
            if (p_user.emergency_contact.FirstOrDefault().country == null || p_user.emergency_contact.FirstOrDefault().country.Trim() == "") { return false; }
            if (p_user.emergency_contact.FirstOrDefault().phone_number == null || p_user.emergency_contact.FirstOrDefault().phone_number.Trim() == "") { return false; }
            return true;

        }


    }
}


