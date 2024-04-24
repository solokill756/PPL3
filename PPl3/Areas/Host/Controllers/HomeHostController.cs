using PPl3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                PPL3Entities db = new PPL3Entities();
                List<property> properties = db.properties.Where(item => item.userId == p_user.id).ToList();
                return View(properties);
            }
            else return RedirectToAction("Login", "homeuser", new { area = "User" });

        }

        [HttpPost]
        public JsonResult DeleteListing(int id)
        {
            user p_user = (user)Session["user"];
            PPL3Entities db = new PPL3Entities();
            property find_hotel = db.properties.FirstOrDefault(item => item.id == id);
            db.properties.Remove(find_hotel);
            db.SaveChanges();
            return Json("true" , JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchListing(string text)
        {
            user p_user = (user)Session["user"];
            PPL3Entities db = new PPL3Entities();
            // Select only the required properties instead of the full entity
            var data = (from item in db.properties
                        where item.userId == p_user.id && item.p_name.Contains(text)
                        select new { item.id, item.p_name, item.p_address, item.created,item.room_type.rt_name,item.property_images.FirstOrDefault().pi_image }).ToList();
            return Json(new
            {
                list_pro = data,
                success = true,
                count = data.Count
            }, JsonRequestBehavior.AllowGet);
        }

    }
}


