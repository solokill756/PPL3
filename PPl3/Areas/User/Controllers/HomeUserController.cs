using System;
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
        public List<user_profile> Model4Data { get; set; }

        public user InforUser { get; set; }

        //Thêm các thuộc tính khác tương ứng với các model khác nếu cần

    }
    public class HomeUserController : Controller
    {
        // GET: User/HomeUser
        MyViewModelPageUser myView = new MyViewModelPageUser();
        public ActionResult Index(int id = -1)
        {
            if (Session["user"] != null && Session["inForUser"] != null)
            {
                myView.InforUser = (user)Session["InforUser"];
                PPL3Entities db = new PPL3Entities();
                category main_cate = db.categories.Where(row => row.category_name.Equals("Main")).FirstOrDefault();
                var list_amenites = from item in db.amenities.ToList()
                                    where item.category_id == main_cate.id
                                    select item;
                myView.Model1Data = list_amenites.ToList();
                if (id == -1)
                {
                    ViewBag.Check = false;
                    List<property> list_property = db.properties.ToList();
                    myView.Model2Data = list_property;
                }
                else
                {
                   ViewBag.Check = true;
                   var property_anmentitie = from item in db.property_amenities.ToList()
                                          where item.amenity_id == id
                                          select item;
                    var list_property = from item in db.properties.ToList()
                                        where property_anmentitie.ToList().Find(value => value.property_id == item.id) != null
                                        select item;
                    myView.Model2Data = list_property.ToList();
                }

                var list_user = db.users.ToList();
                myView.Model3Data = list_user.ToList();

                var list_user_pro = db.user_profile.ToList();
                myView.Model4Data = list_user_pro.ToList();
                //Console.WriteLine(inforUser);
                return View(myView);
            }
            else
            {
                return RedirectToAction("Index" , "home" , new {area = ""});

            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(user model , string email_address)
        {
            PPL3Entities entities = new PPL3Entities();
            if (!IsGmailExists(email_address))

            {

                ModelState.AddModelError("checkGmail", "Gmail này không tồn tại");

                return View(model);

            }
            else if (!IsPasswordExists(model.user_password))

            {

                ModelState.AddModelError("Password", "Mật khẩu bạn nhập không đúng");
                return View(model);

            }

            else

            {
                Session["user"] = "user";
                var id_user = entities.user_personalInfor.Where(item => item.email_address == email_address).FirstOrDefault().userID;
                var userInfor = (entities.users.Where(item => item.id ==  id_user).FirstOrDefault());
                Session["inForUser"] = userInfor;
                return RedirectToAction("Index", "HomeUser", new { area = "User", id = -1 });



            }
        }


        public ActionResult LogOut()
        {
            Session.Remove("user");
            Session.Remove("inForUser");
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "home", new { area = "" });
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(user model , string email_address)
        {
            user_personalInfor user_PersonalInfor = new user_personalInfor();
            PPL3Entities db = new PPL3Entities();
    
            if (IsGmailExists(email_address))

            {

                ModelState.AddModelError("Gmail", "Gmail đã được sử dụng");

                return View(model);

            }
            else
            {
                
                user_PersonalInfor.email_address = email_address;
                user_PersonalInfor.userID = model.id;
                db.user_personalInfor.Add(user_PersonalInfor);
                db.users.Add(model);
                db.SaveChanges();
                Session["user"] = "user";
                var id_user = db.user_personalInfor.Where(item => item.email_address == email_address).FirstOrDefault().userID;
                var userInfor = (db.users.Where(item => item.id == id_user).FirstOrDefault());
                Session["inForUser"] = userInfor;
                return RedirectToAction("Index", "HomeUser", new { area = "User", id = -1 });

            }
        }





        //Function
        public bool IsGmailExists(string email_address)

        {

            PPL3Entities db = new PPL3Entities();

            return db.user_personalInfor.Any(u => u.email_address== email_address);

        }
        public bool IsPasswordExists(string password)

        {

            PPL3Entities db = new PPL3Entities();

            return db.users.Any(u => u.user_password == password);

        }

    }
}
