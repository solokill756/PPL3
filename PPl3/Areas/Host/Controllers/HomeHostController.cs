using PPl3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PPl3.App_Start;

namespace PPl3.Areas.Host.Controllers
{
    public class HomeHostController : Controller
    {
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
                if (checkHost(p_user) == true)
                {


                    PPL3Entities db = new PPL3Entities();
                    user fix_user = db.users.FirstOrDefault(item => item.id == p_user.id);
                    fix_user.user_type = 3;
                    fix_user.day_become_host = DateTime.Now;
                    db.SaveChanges();
                    Session["user"] = fix_user;
                    List<property> properties = db.properties.Where(item => item.userId == p_user.id).ToList();
                    return View(properties);

                }
                else
                {
                    
                    return RedirectToAction("userPersonalProfile", "homeuser", new { area = "user" , error = true});
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


        public ActionResult addHotel()
        {
            return View();
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


