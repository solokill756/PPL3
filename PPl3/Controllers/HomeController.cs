using Newtonsoft.Json;
using PPl3.Areas.User.Controllers;
using PPl3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
namespace PPl3.Controllers
{
    public class MyViewModel

    {

        public List<amenity> Model1Data { get; set; }

        public List<property> Model2Data { get; set; }

        public List<user> Model3Data { get; set; }



        //Thêm các thuộc tính khác tương ứng với các model khác nếu cần

    }
    public class HomeController : Controller
    {
        MyViewModel myView = new MyViewModel();
        public ActionResult Index(int id = -1 , int checkID = -1 , bool checkLogOut = false) 
        {
            ViewBag.checkLogOut = checkLogOut;
            Console.WriteLine(ViewBag.checkLogOut);
            PPL3Entities db = new PPL3Entities();
            category main_cate = db.categories.Where(row => row.category_name.Equals("Main")).FirstOrDefault();
            var list_amenites = from item in db.amenities.ToList()
                                where item.category_id == main_cate.id
                                select item;
            myView.Model1Data = list_amenites.ToList();
            if (id == -1 || checkID == id)
            {
                List<property> list_property = db.properties.ToList().OrderByDescending(item => item.bookings.ToArray().Length).ToList();
                myView.Model2Data = list_property;
            }
            else
            {
                var property_amenities = db.property_amenities.Where(item => item.amenity_id == id).ToList();
                var property_ids = property_amenities.Select(item => item.property_id).ToList();
                var list_property = db.properties.Where(item => property_ids.Contains(item.id)).ToList();
                myView.Model2Data = list_property.OrderByDescending(item => item.bookings.ToArray().Length).ToList();
                ViewBag.AmenityId = id;
            }
            if (id != -1) ViewBag.checkId = id;
            var list_user = db.users.ToList();
            myView.Model3Data = list_user.ToList();
            return View(myView);
        }

        public ActionResult Detail(int id)
        {
            PPL3Entities db = new PPL3Entities();
            ViewBag.propertyFind = db.properties.Where(item => item.id == id).FirstOrDefault();
            List<booking> bookings = db.bookings.Where(item => item.property_id == id).ToList();
            string date = "";
            foreach (var item in bookings)
            {
                date = date + item.check_in_date.Value.ToString("dd/MM/yyyy") + "-" + item.check_out_date.Value.ToString("dd/MM/yyyy") + ",";
            }
            if(date.Trim() != "")  date = date.Substring(0, date.Length - 1);
            ViewBag.dataJson = JsonConvert.SerializeObject(date);
            return View();
        }
        public ActionResult profile(int id)
        {
            PPL3Entities db = new PPL3Entities();
            ViewBag.userfind = db.users.Where(item => item.id == id).FirstOrDefault();
            return View();
        }
    }
    
}