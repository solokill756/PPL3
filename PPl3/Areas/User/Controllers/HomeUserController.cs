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
                if (TempData["checkId"] != null) id = (int)TempData["checkId"];
                if (TempData["check"] == null) ViewBag.check = true;
                else ViewBag.check = false;
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
            else
            {
                return RedirectToAction("Index" , "home" , new {area = ""});

            }
        }


        //WishList
        public ActionResult AddWishList(int id , int? checkId)
        {
            PPL3Entities2 db = new PPL3Entities2();
            user p_user = (user)Session["user"];
            favourite fa = new favourite();
            fa.property_id = id;
            fa.added_date = DateTime.Now;
            fa.userID = p_user.id;
            db.favourites.Add(fa);
            db.SaveChanges();
            TempData["checkId"] = checkId;
            return RedirectToAction("index");
            
        }

        public ActionResult wishList()
        {
            return View();
        }

        public ActionResult DeleteWishList(int id)
        {
            PPL3Entities2 db = new PPL3Entities2();
            favourite find_favourite = db.favourites.Where(item => item.property_id == id).FirstOrDefault();
            db.favourites.Remove(find_favourite);
            db.SaveChanges();
            return RedirectToAction("wishList");
        }
        ///

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(user model , string email_address)
        {
            PPL3Entities2 entities = new PPL3Entities2();
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
            PPL3Entities2 db = new PPL3Entities2();
    
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

        public ActionResult Detail(int id)
        {
            PPL3Entities2 db = new PPL3Entities2();
            ViewBag.propertyFind = db.properties.Where(item => item.id == id).FirstOrDefault();
            return View();
        }


        



        //Function
        public bool IsGmailExists(string email_address)

        {

            PPL3Entities2 db = new PPL3Entities2();

            return db.user_personalInfor.Any(u => u.email_address== email_address);

        }
        public bool IsPasswordExists(string password)

        {

            PPL3Entities2 db = new PPL3Entities2();

            return db.users.Any(u => u.user_password == password);

        }

    }
}
