using PPl3.Areas.User.Controllers;
using PPl3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
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

        //Thêm các thuộc tính khác tương ứng với các model khác nếu cần

    }
    public class HomeController : Controller
    {
        public ActionResult Index(string id = "") 
        {
            int search = 0;
            if (id != "")
            {
                search = Convert.ToInt32(id);
            }
            var myView = new MyViewModel();
            PPL3Entitie1  db = new PPL3Entitie1 ();
            category main_cate = db.categories.Where(row => row.category_name.Equals("Main") == true).FirstOrDefault();
            var list_amenites = from item in db.amenities.ToList()
                                where item.category_id == main_cate.id
                                select item;
            myView.Model1Data = list_amenites.ToList();
            if (id == "")
            {
                var property_anmentitie = from item in db.property_amenities.ToList()
                                          select item;
                var list_property = from item in db.properties.ToList()
                                    where property_anmentitie.ToList().Find(value => value.property_id == item.id) != null
                                    select item;
                myView.Model2Data = list_property.ToList();
            }
            else
            {
                var property_anmentitie = from item in db.property_amenities.ToList()
                                          where item.amenity_id == search
                                          select item;
                var list_property = from item in db.properties.ToList()
                                    where property_anmentitie.ToList().Find(value => value.property_id == item.id) != null
                                    select item;
                myView.Model2Data = list_property.ToList();
            }
            //Console.WriteLine(inforUser);
            return View(myView);

        }


    }
}