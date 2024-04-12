﻿using PPl3.Areas.User.Controllers;
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
            PPL3Entities2 db = new PPL3Entities2();
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

        public ActionResult Detail(int id)
        {
            PPL3Entities2 db = new PPL3Entities2();
            ViewBag.propertyFind = db.properties.Where(item => item.id == id).FirstOrDefault();
            return View();
        }
    }
}