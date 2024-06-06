using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Newtonsoft.Json;
using PPl3.Models;
using PPl3.App_Start;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Configuration;
using PPl3.Models.Payments;
using System.Data.Entity;
using Microsoft.AspNet.SignalR;
using PPl3.Hubs;
using System.Text;
using static System.Data.Entity.Infrastructure.Design.Executor;
using System.Security.Policy;
using System.Net;
using System.Security.Cryptography;
using System.Web.UI.WebControls;
namespace PPl3.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        // GET: Admin/HomeAdmin
        public ActionResult Index()
        {
            if (Session["user"] != null)
            {
                if (TempData["CheckComfirm"] != null)
                {
                    ViewBag.CheckComfirm = 1;
                }
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
                            if (item.day_become_host.Value.Month == i) count1++;
                        }

                    }
                    foreach (var item in db.properties)
                    {
                        if (item.Date_Post.Value.Month == i && item.p_status == 1) count2++;
                    }
                    result1 = result1 + count1.ToString() + ",";
                    result2 = result2 + count2.ToString() + ",";
                    i++;
                }
                ViewBag.data_hosts = result1.ToString().Substring(0, result1.Length - 1);
                ViewBag.data_hotels = result2.ToString().Substring(0, result2.Length - 1);
                return View();
            }
            else
            {
                return RedirectToAction("login", "HomeUser", new { area = "user" });
            }
            
        }
        [HttpPost]
        [AdminAuhorize(idChucNang = 17)]
        public JsonResult DeleteUser(int id)
        {
            PPL3Entities db = new PPL3Entities();
            string phanHoi = "true";
            List<int> delprop = new List<int>();
            var delUser = db.users.Where(item => item.id == id).FirstOrDefault();
            if(delUser.user_type == 3)
            {
                foreach(var num in db.properties.Where(item => item.userId == delUser.id).ToList())
                {
                    delprop.Add(num.id);
                }
            }
            try
            {
                db.Messages.RemoveRange(db.Messages.Where(item => item.SenderID == id || item.ReceiverID == id).ToList());
                db.host_reviews.RemoveRange(db.host_reviews.Where(item => item.hostid == id || item.review_by_user == id).ToList());
                db.Friendships.RemoveRange(db.Friendships.Where(item => item.UserId2 == id || item.UserId1 == id).ToList());
                db.transactions.RemoveRange(db.transactions.Where(item => item.receiver_id == id || item.payer_id == id).ToList());
                db.users.Remove(delUser);
                db.SaveChanges();
            }
            catch(Exception e)
            {
                phanHoi = e.ToString();
            }
            var data = new
            {
                phanhoi = phanHoi,
                delprop = delprop,
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AdminAuhorize(idChucNang = 17)]
        public JsonResult DeleteHotel(int id)
        {
            PPL3Entities db = new PPL3Entities();
            string phanHoi = "true";
            try
            {
                db.properties.Remove(db.properties.Where(item => item.id == id).FirstOrDefault());
                db.SaveChanges();
            }
            catch(Exception e) 
            {
                phanHoi = e.ToString();
            }
            return Json(phanHoi, JsonRequestBehavior.AllowGet);
        }
        [AdminAuhorize(idChucNang = 15)]
        public ActionResult confirm_host(int user_id)
        {
            PPL3Entities db = new PPL3Entities();
            browser_becomes_host find_user = db.browser_becomes_host.FirstOrDefault(item => item.user_id == user_id);
            find_user.user.user_type = 3;
            find_user.user.day_become_host = DateTime.Now;
            db.SaveChanges();
            db.browser_becomes_host.Remove(find_user);
            user_notification user_Notification = new user_notification();
            user_Notification.userid = user_id;
            user_Notification.created = DateTime.Now;
            user_Notification.content = "You have successfully become a host";
            user_Notification.un_status = 0;
            user_Notification.un_url = "/host/homehost/Listing";
            db.user_notification.Add(user_Notification);
            db.SaveChanges();
            user find_host = db.users.FirstOrDefault(item => item.id == user_id);
            string emailHtml = RenderRazorViewToString("AccpectHost", find_host);
            SendEmail("hosithao1622004@gmail.com" , "Congrats on becoming a host!" , emailHtml);
            TempData["CheckComfirm"] = 1;
            return RedirectToAction("index");
        }
        [HttpPost]
        [AdminAuhorize(idChucNang = 15)]
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
            
            
            db.user_notification.Add(user_Notification);
            db.SaveChanges();
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        [AdminAuhorize(idChucNang = 16)]
        public ActionResult confirm_hotel(int hotel_id , int user_id)
        {
            PPL3Entities db = new PPL3Entities();
            Browse_hotel_listings find_hotel = db.Browse_hotel_listings.FirstOrDefault(item => item.property_id == hotel_id);
            find_hotel.property.p_status = 1;
            find_hotel.property.Date_Post = DateTime.Now;
            
           
            db.SaveChanges();
            db.Browse_hotel_listings.Remove(find_hotel);
            user_notification user_Notification = new user_notification();
            user_Notification.userid = user_id;
            user_Notification.created = DateTime.Now;
            user_Notification.content = "You have successfully posted your hotel";
            user_Notification.un_status = 0;
            user_Notification.un_url = "#";
            db.user_notification.Add(user_Notification);
            db.SaveChanges();
            TempData["CheckComfirm"] = 1;
            string emailHtml = RenderRazorViewToString("AccpectHotel", find_hotel);
            SendEmail("hosithao1622004@gmail.com", "Your [Room/Hotel] Approval Successful!", emailHtml);
            return RedirectToAction("index");
        }


        [AdminAuhorize(idChucNang = 16)]
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

        [AdminAuhorize(idChucNang = 15)]
        public ActionResult ComfirmAll(int[] id, int check)

        {
            PPL3Entities db = new PPL3Entities();
            if (check == 1 && id.Length > 0)
            {
                List<browser_becomes_host> list_host = new List<browser_becomes_host>();
                for (int i = 0; i <= id.Length - 1; ++i)
                {
                    int find_id = id[i];
                    browser_becomes_host find_host = db.browser_becomes_host.FirstOrDefault(item => item.user_id == find_id);
                    if(find_host != null)
                    list_host.Add(find_host);
                }
                foreach (var item in list_host)
                {
                    item.user.user_type = 3;
                    item.user.day_become_host = DateTime.Now;
                    user_notification user_Notification = new user_notification();
                    user_Notification.userid = item.user_id;
                    user_Notification.created = DateTime.Now;
                    user_Notification.content = "You have successfully posted your hotel";
                    user_Notification.un_status = 0;
                    user_Notification.un_url = "#";
                    db.user_notification.Add(user_Notification);
                    db.SaveChanges();
                    user find_host = db.users.FirstOrDefault(row => row.id == item.user_id);
                    string emailHtml = RenderRazorViewToString("AccpectHost", find_host);
                    SendEmail("hosithao1622004@gmail.com", "Congrats on becoming a host!", emailHtml);

                }
                foreach (var item in list_host)
                {
                    db.browser_becomes_host.Remove(item);
                }
                db.SaveChanges();
                TempData["CheckComfirm"] = 1;
              
            }
            else if (check == 2 && id.Length > 0)
            {

                List<Browse_hotel_listings> list_hotel = new List<Browse_hotel_listings>();
                for (int i = 0; i <= id.Length - 1; ++i)
                {
                    int find_id = id[i];
                    Browse_hotel_listings find_hotel = db.Browse_hotel_listings.FirstOrDefault(item => item.property_id == find_id);
                    if(find_hotel != null)
                    list_hotel.Add(find_hotel);
                }
                foreach (var item in list_hotel)
                {
                    item.property.p_status = 1;
                    item.property.Date_Post = DateTime.Now;
                    user_notification user_Notification = new user_notification();
                    user_Notification.userid = item.property_id;
                    user_Notification.created = DateTime.Now;
                    user_Notification.content = "Your hotel does not meet the listing conditions";
                    user_Notification.un_status = 0;
                    user_Notification.un_url = "#";
                    db.user_notification.Add(user_Notification);
                    db.SaveChanges();
                    property find_hotel = db.properties.FirstOrDefault(row => row.id == item.property_id);
                    string emailHtml = RenderRazorViewToString("AccpectHotel", find_hotel);
                    SendEmail("hosithao1622004@gmail.com", "Your [Room/Hotel] Approval Successful!", emailHtml);
                }
                foreach (var item in list_hotel)
                {
                    db.Browse_hotel_listings.Remove(item);
                }
                db.SaveChanges();
                TempData["CheckComfirm"] = 1;
               
            }
            return RedirectToAction("index");
        }
        [AdminAuhorize(idChucNang = 15)]
        public ActionResult DeniedAll(int[] id , int check)
        {
            PPL3Entities db = new PPL3Entities();
            if (check == 1 && id.Length > 0)
            {
                List<browser_becomes_host> list_host = new List<browser_becomes_host>();
                for (int i = 0; i <= id.Length - 1; ++i)
                {
                    int find_id = id[i];
                    browser_becomes_host find_host = db.browser_becomes_host.FirstOrDefault(item => item.user_id == find_id);
                    if(find_host != null) list_host.Add(find_host);
                }

                foreach (var item in list_host)
                {
                    db.browser_becomes_host.Remove(item);
                }
                db.SaveChanges();
                TempData["CheckComfirm"] = 1;
                return RedirectToAction("index");
            }
            else if (check == 2 && id.Length > 0)
            {

                List<Browse_hotel_listings> list_hotel = new List<Browse_hotel_listings>();
                for (int i = 0; i <= id.Length - 1; ++i)
                {
                    int find_id = id[i];
                    Browse_hotel_listings find_hotel = db.Browse_hotel_listings.FirstOrDefault(item => item.property_id == find_id);
                    if(find_hotel != null)
                    list_hotel.Add(find_hotel);
                }

                foreach (var item in list_hotel)
                {
                    db.Browse_hotel_listings.Remove(item);
                }
                db.SaveChanges();
                TempData["CheckComfirm"] = 1;
                return RedirectToAction("index");
            }
            else return RedirectToAction("index");
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


        

        // Send Email

        private int SendEmail(string recipientEmail, string subject, string body)
        {
            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress("tnthotel2004@gmail.com");
                    mailMessage.To.Add(recipientEmail);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;
                    string appPassword = "tsjp dydc eexq rgik";
                    using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtpClient.Credentials = new NetworkCredential("tnthotel2004@gmail.com", appPassword);
                        smtpClient.EnableSsl = true;
                        smtpClient.Send(mailMessage);
                    }
                }
                return 1;
            }
            catch (SmtpFailedRecipientException)
            {
                return 0;
            }
            catch (SmtpException)
            {
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        private string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
    

}