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
    }
}