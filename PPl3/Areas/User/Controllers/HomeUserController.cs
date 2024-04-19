﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using PPl3.Models;

namespace PPl3.Areas.User.Controllers
{

    public class MyViewModelPageUser

    {

        public List<amenity> Model1Data { get; set; }

        public List<property> Model2Data { get; set; }

        public List<user> Model3Data { get; set; }

        //Thêm các thuộc tính khác tương ứng với các model khác nếu cần

    }

    
    public class HomeUserController : Controller
    {
        // GET: User/HomeUser
        MyViewModelPageUser myView = new MyViewModelPageUser();
        public ActionResult Index(int? id , int? checkID)
        {
            if (Session["user"] != null)
            {
                
                if (TempData["check"] == null) ViewBag.check = true;
                else ViewBag.check = false;
                if (TempData["checkBook"] != null) ViewBag.checkBook = true;
                else ViewBag.checkBook = false;

                PPL3Entities3 db = new PPL3Entities3();
                category main_cate = db.categories.Where(row => row.category_name.Equals("Main")).FirstOrDefault();
                var list_amenites = from item in db.amenities.ToList()
                                    where item.category_id == main_cate.id
                                    select item;
                myView.Model1Data = list_amenites.ToList();
                if (id == -1 || checkID == id)
                {
                    List<property> list_property = db.properties.ToList();
                    myView.Model2Data = list_property;
                }
                else
                {
                    var property_anmentitie = from item in db.property_amenities.ToList()
                                              where item.amenity_id == id
                                              select item;
                    var list_property = from item in db.properties.ToList()
                                        where property_anmentitie.ToList().Find(value => value.property_id == item.id) != null
                                        select item;
                    myView.Model2Data = list_property.ToList();
                    ViewBag.AmenityId = id;
                }
                if (id != -1) ViewBag.checkId = id;
                
                var list_user = db.users.ToList();
                myView.Model3Data = list_user.ToList();
                return View(myView);
            }
            else
            {
                return RedirectToAction("Index" , "home" , new {area = ""});

            }
        }


        //WishList

        [HttpPost]
        public JsonResult AddWishList(int id)
        {
            PPL3Entities3 db = new PPL3Entities3();
            user p_user = (user)Session["user"];
            favourite fa = new favourite();
            if(db.favourites.Any(item => item.property_id == id && item.userID == p_user.id) == false)
            {
                fa.property_id = id;
                fa.added_date = DateTime.Now;
                fa.userID = p_user.id;
                db.favourites.Add(fa);
                db.SaveChanges();
            }
            return Json("true", JsonRequestBehavior.AllowGet);
            
        }

        public ActionResult wishList()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DeleteWishList(int id)
        {
            PPL3Entities3 db = new PPL3Entities3();
            user p_user = (user)Session["user"];
            favourite find_favourite = db.favourites.Where(item => item.property_id == id && item.userID == p_user.id).FirstOrDefault();
            if(find_favourite != null)
            {
                db.favourites.Remove(find_favourite);
                db.SaveChanges();
            }
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteAllWishList(List<int> list_id)
        {
            PPL3Entities3 db = new PPL3Entities3();
            user p_user = (user)Session["user"];
            foreach (var id in list_id)
            {
                favourite find_favourite = db.favourites.Where(item => item.property_id == id && item.userID == p_user.id).FirstOrDefault();
                if (find_favourite != null)
                {
                    db.favourites.Remove(find_favourite);
                    db.SaveChanges();
                }
            }
            return Json("true", JsonRequestBehavior.AllowGet);
        }
        ///

        // Login and Logout and Sign up
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(user model , string email_address)
        {
            PPL3Entities3 entities = new PPL3Entities3();
            user_personalInfor find_user = entities.user_personalInfor.Where(item => item.email_address == email_address).FirstOrDefault();
            if (!IsGmailExists(email_address))

            {

                ModelState.AddModelError("checkGmail", "Gmail này không tồn tại");

                return View(model);

            }
            else if (!IsPasswordExists(model.user_password , (int)find_user.userID))

            {

                ModelState.AddModelError("Password", "Mật khẩu bạn nhập không đúng");
                return View(model);

            }

            else

            {
                var id_user = entities.user_personalInfor.Where(item => item.email_address == email_address).FirstOrDefault().userID;
                var userInfor = (entities.users.Where(item => item.id ==  id_user).FirstOrDefault());
                if(userInfor.user_type != 1) Session["user"] = userInfor;
                else if(userInfor.user_type == 1) Session["admin"] = userInfor;
                TempData["check"] = false;
                if (Session["user"] != null) return RedirectToAction("Index", "HomeUser", new { area = "User", id = -1, checkID = -1 });
                else
                {
                    return RedirectToAction("Index", "HomeAdmin", new { area = "Admin"});
                }

            }
        }


        public ActionResult LogOut()
        {
            Session.Remove("user");
            FormsAuthentication.SignOut();  
            return RedirectToAction("Index", "home", new { area = "" , checkLogOut = true});
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(user model , string email_address)
        {
            user_personalInfor user_PersonalInfor = new user_personalInfor();
            PPL3Entities3 db = new PPL3Entities3();
            
            if (IsGmailExists(email_address))

            {

                ModelState.AddModelError("Gmail", "Gmail đã được sử dụng");

                return View(model);

            }
            else
            {
                if (model.user_type == null) model.user_type = 2;
                user_PersonalInfor.email_address = email_address;
                user_PersonalInfor.userID = model.id;
                db.user_personalInfor.Add(user_PersonalInfor);
                db.users.Add(model);
                db.SaveChanges();
                var id_user = db.user_personalInfor.Where(item => item.email_address == email_address).FirstOrDefault().userID;
                var userInfor = (db.users.Where(item => item.id == id_user).FirstOrDefault());
                if (userInfor.user_type != 1) Session["user"] = userInfor;
                else if (userInfor.user_type == 1) Session["admin"] = userInfor;
                TempData["check"] = false;
               if(Session["user"] != null) return RedirectToAction("Index", "HomeUser", new { area = "User", id = -1 , checkID = -1 });
               else
                {
                    return RedirectToAction("Index", "HomeAdmin", new { area = "Admin"});
                }

            }
        }


        // Detail
        public ActionResult Detail(int id , int bookingId = -1)
        {
            if(bookingId != -1) ViewBag.bookingId = bookingId;
            PPL3Entities3 db = new PPL3Entities3();
            ViewBag.propertyFind = db.properties.Where(item => item.id == id).FirstOrDefault();
            return View();
        }


        // trip

        public ActionResult Trip()
        {
            user p_user = (user)Session["user"];
            PPL3Entities3 db = new PPL3Entities3();
            var list_booking_hotel = db.bookings.Where(item => item.userId == p_user.id).OrderBy(item => item.check_in_date).ThenBy(item => item.check_out_date).ThenBy(item => item.id).ToList();
            return View(list_booking_hotel) ;
        }

        [HttpPost]
        public JsonResult FixBookingHotel(int bookingId , DateTime check_in_date, DateTime check_out_date, int[] guest_count , int hotelId)
        {
            PPL3Entities3 db = new PPL3Entities3();
            user p_user = (user)Session["user"];
            booking find_hotel = db.bookings.Where(item => item.id == bookingId).FirstOrDefault();
            if ((db.bookings.Any(item => ((item.check_in_date <= check_in_date && item.check_out_date >= check_in_date) || (item.check_out_date >= check_out_date && item.check_in_date <= check_out_date)) && item.userId == p_user.id && item.property_id == hotelId) == false && check_in_date >= DateTime.Now) || (find_hotel.check_in_date == check_in_date && find_hotel.check_out_date == check_out_date)) { 
                find_hotel.check_in_date = check_in_date;
                find_hotel.check_out_date = check_out_date;
                db.SaveChanges();
                while (db.booking_guests.Any(item => item.booking_id == bookingId))
                {
                    booking_guests find_booking_guest = db.booking_guests.Where(item => item.booking_id == bookingId).FirstOrDefault();
                    db.booking_guests.Remove(find_booking_guest);
                    db.SaveChanges();
                }
                for (int i = 0; i <= guest_count.Length - 1; ++i)
                {
                    booking_guests booking_Guests = new booking_guests();
                    booking_Guests.booking_id = find_hotel.id;
                    booking_Guests.guest_type_id = i + 1;
                    booking_Guests.num_guests = guest_count[i];
                    db.booking_guests.Add(booking_Guests);
                }
                db.SaveChanges();
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }


        // BookingDate
        
        public JsonResult BookingDate(int hotelId)
        {
            PPL3Entities3 db = new PPL3Entities3();
            List<booking> bookings = db.bookings.Where(item => item.property_id == hotelId).ToList();
            List<string> booking_Dates = new List<string>();
            
            foreach (var item in bookings)
            {
                string date = "";
                date = item.check_in_date.Value.ToString("dd/MM/yyyy") + "-" + item.check_out_date.Value.ToString("dd/MM/yyyy");
                booking_Dates.Add(date);
            }
            return Json(booking_Dates, JsonRequestBehavior.AllowGet);
            
        }

        //Pay Hotel
        [HttpPost]
        public JsonResult PayHotel(bool checkBook , int id , DateTime checkInDate , DateTime checkOutDate , int[] guest_count)
        {
            PPL3Entities3 db = new PPL3Entities3();
            user p_user = (user)Session["user"];
            if (db.bookings.Any(item => ((item.check_in_date <= checkInDate && item.check_out_date >= checkInDate) || (item.check_out_date >= checkOutDate && item.check_in_date <= checkOutDate)) && item.userId ==  p_user.id && item.property_id == id) == false && checkInDate >= DateTime.Now)
            {
                TempData["checkBook"] = checkBook;
                property findHotel = db.properties.Where(item => item.id == id).FirstOrDefault();
                booking userBooking = new booking();
                userBooking.property_id = id;
                userBooking.userId = p_user.id;
                userBooking.check_in_date = checkInDate;
                userBooking.check_out_date = checkOutDate;
                userBooking.price_per_day = findHotel.price;
                TimeSpan difference = (userBooking.check_out_date.Value - userBooking.check_in_date.Value);
                int daysDifference = (int)difference.TotalDays;

                double sum = daysDifference * (double)userBooking.price_per_day;
                userBooking.amount_paid = (decimal)sum;
                userBooking.booking_date = DateTime.Now;
                userBooking.is_refund = 1;
                userBooking.booking_status = 1;
                db.bookings.Add(userBooking);
                for (int i = 0; i <= guest_count.Length - 1; ++i)
                {
                    booking_guests booking_Guests = new booking_guests();
                    booking_Guests.booking_id = userBooking.id;
                    booking_Guests.guest_type_id = i + 1;
                    booking_Guests.num_guests = guest_count[i];
                    db.booking_guests.Add(booking_Guests);
                }
                db.SaveChanges();
                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("Index", "Homeuser", new { area = "user" })
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    success = false
                }, JsonRequestBehavior.AllowGet);

            }
        }


        //Function
        public bool IsGmailExists(string email_address)

        {

            PPL3Entities3 db = new PPL3Entities3();
            return db.user_personalInfor.Any(u => u.email_address== email_address);

        }
        public bool IsPasswordExists(string password , int id)

        {

            PPL3Entities3 db = new PPL3Entities3();

            return db.users.Any(u => u.user_password == password && u.id == id);

        }

    }
}
