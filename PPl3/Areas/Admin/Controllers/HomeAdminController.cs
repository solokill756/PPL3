using PPl3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Collections.Specialized.BitVector32;
using System.Web.Security;

namespace PPl3.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        // GET: Admin/HomeAdmin
        public ActionResult Index()
        {
            PPL3Entities db = new PPL3Entities();
            string result1 = "";
            string result2 = "";
            int i = 1;
            while (i <= 12)
            {
                int count1 = 0;
                int count2 = 0;
                foreach (var item in db.users)
                {
                    if (item.user_type == 3)
                    {
                        if(item.day_become_host.Value.Month == i) count1++; 
                    }
                    
                }
                foreach(var item in db.Browse_hotel_listings)
                {
                    if (item.Date.Value.Month == i && item.status == true) count2++;
                }
                result1 = result1 + count1.ToString() + ",";
                result2 = result2 + count2.ToString() + ",";
                i++;
            }
            ViewBag.data_hosts = result1.ToString().Substring(0, result1.Length - 1);
            ViewBag.data_hotels = result2.ToString().Substring(0, result2.Length - 1);
            return View();
        }
        [HttpPost]
        public JsonResult confirm_host(int user_id)
        {
            PPL3Entities db = new PPL3Entities();
            browser_becomes_host find_user = db.browser_becomes_host.FirstOrDefault(item => item.user_id == user_id);
            find_user.status = true;
            find_user.user.user_type = 3;
            db.SaveChanges();
            user_notification user_Notification = new user_notification();
            user_Notification.userid = user_id;
            user_Notification.created = DateTime.Now;
            user_Notification.content = "You have successfully become a host";
            user_Notification.un_status = 0;
            user_Notification.un_url = "/host/homehost/Listing";
            db.user_notification.Add(user_Notification);
            db.SaveChanges();
            return Json("true", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult host_denied(int user_id)
        {
            PPL3Entities db = new PPL3Entities();
            browser_becomes_host find_user = db.browser_becomes_host.FirstOrDefault(item => item.user_id == user_id);
            db.browser_becomes_host.Remove(find_user);
            db.SaveChanges();
            user_notification user_Notification = new user_notification();
            user_Notification.userid = user_id;
            user_Notification.created = DateTime.Now;
            user_Notification.content = "You have been rejected to become a host";
            user_Notification.un_status = 0;
            user_Notification.un_url = "#";
            db.users.FirstOrDefault(item => item.id == user_id).day_become_host = DateTime.Now;
            db.user_notification.Add(user_Notification);
            db.SaveChanges();
            return Json("true", JsonRequestBehavior.AllowGet);
        }


        public JsonResult confirm_hotel(int hotel_id , int user_id)
        {
            PPL3Entities db = new PPL3Entities();
            Browse_hotel_listings find_hotel = db.Browse_hotel_listings.FirstOrDefault(item => item.property_id == hotel_id);
            find_hotel.status = true;
            find_hotel.property.p_status  = 1;
            db.SaveChanges();
            user_notification user_Notification = new user_notification();
            user_Notification.userid = user_id;
            user_Notification.created = DateTime.Now;
            user_Notification.content = "You have successfully posted your hotel";
            user_Notification.un_status = 0;
            user_Notification.un_url = "#";
            db.user_notification.Add(user_Notification);
            db.SaveChanges();
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        public JsonResult hotel_denied(int hotel_id, int user_id)
        {
            PPL3Entities db = new PPL3Entities();
            Browse_hotel_listings find_hotel = db.Browse_hotel_listings.FirstOrDefault(item => item.property_id == hotel_id);
            db.Browse_hotel_listings.Remove(find_hotel);
            user_notification user_Notification = new user_notification();
            user_Notification.userid = user_id;
            user_Notification.created = DateTime.Now;
            user_Notification.content = "Your hotel does not meet the listing conditions";
            user_Notification.un_status = 0;
            user_Notification.un_url = "#";
            db.user_notification.Add(user_Notification);
            db.SaveChanges();
            return Json("true", JsonRequestBehavior.AllowGet);
        }
        public ActionResult LogOut()
        {
            PPL3Entities entities = new PPL3Entities();
            user p_user = (user)Session["user"];
            user find_user = entities.users.FirstOrDefault(item => item.id == p_user.id);
            find_user.user_status = 0;
            entities.SaveChanges();
            Session.Remove("user");
            FormsAuthentication.SignOut();
            return RedirectToAction("login", "homeuser", new { area = "user", checkLogOut = true });
        }
    }
    

}