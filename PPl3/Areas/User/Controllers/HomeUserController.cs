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
                
                if (TempData["check"] == null) ViewBag.check = true;
                else ViewBag.check = false;
                if (TempData["checkBook"] != null) ViewBag.checkBook = true;
                else ViewBag.checkBook = false;

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
            else
            {
                return RedirectToAction("login");

            }
        }


        //WishList

        [HttpPost]

        [UserAuthorize(idChucNang = 6)]

        public JsonResult AddWishList(int id)
        {
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
            favourite fa = new favourite();
            if(db.favourites.Any(item => item.property_id == id && item.userID == p_user.id) == false)
            {
                fa.property_id = id;
                fa.added_date = DateTime.Now;
                fa.userID = p_user.id;
                property tmp = db.properties.FirstOrDefault(item => item.id == fa.property_id);
                user_notification user_Notification = new user_notification();
                user_Notification.userid = p_user.id;
                user_Notification.created = DateTime.Now;
                user_Notification.content = "Successfully added "+ tmp.p_name +" to wishlist! Please check your wishlist.";
                user_Notification.un_status = 0;
                user_Notification.un_url = "/user/homeuser/wishlist";
                db.user_notification.Add(user_Notification);
                db.favourites.Add(fa);
                db.SaveChanges();
                
            }
            int cun = db.user_notification.Where(item => item.userid == p_user.id && item.un_status == 0).Count();
            var data = new
            {
                success = "true",
                count = cun,
            };
            return Json(data, JsonRequestBehavior.AllowGet);
            
        }

        public ActionResult wishList()
        {
            if (Session["user"] != null) return View();
            else return RedirectToAction("login");
        }

        [HttpPost]
        [UserAuthorize(idChucNang = 9)]
        public JsonResult DeleteWishList(int id)
        {
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
            favourite find_favourite = db.favourites.Where(item => item.property_id == id && item.userID == p_user.id).FirstOrDefault();
            if(find_favourite != null)
            {
                db.favourites.Remove(find_favourite);
                db.SaveChanges();
            }
            user_notification user_Notification = new user_notification();
            user_Notification.userid = p_user.id;
            user_Notification.created = DateTime.Now;
            user_Notification.content = "Favorite hotel successfully deleted";
            user_Notification.un_status = 0;
            user_Notification.un_url = "/user/homeuser/wishlist";
            db.user_notification.Add(user_Notification);
            db.SaveChanges();
            int cun = db.user_notification.Where(item => item.userid == p_user.id && item.un_status == 0).Count();
            var data = new
            {
                success = "true",
                count = cun,
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [UserAuthorize(idChucNang = 9)]
        public JsonResult DeleteAllWishList(List<int> list_id)
        {
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
            foreach (var id in list_id)
            {
                favourite find_favourite = db.favourites.Where(item => item.property_id == id && item.userID == p_user.id).FirstOrDefault();
                if (find_favourite != null)
                {
                    db.favourites.Remove(find_favourite);
                    db.SaveChanges();
                }
            }
            return Json("true", JsonRequestBehavior.AllowGet);
        }
        ///

        //user profile
        public ActionResult profile(int id)
        {
            if (Session["user"] != null)
            {
              
                PPL3Entities db = new PPL3Entities();
                user p_user = (user)Session["user"];
                if (!(db.user_profile.Any(item => item.userID == p_user.id)))
                {
                    user_profile new_profile = new user_profile();
                    new_profile.userID = p_user.id;
                    db.user_profile.Add(new_profile);
                    db.SaveChanges();
                    var userInfor = (db.users.Where(item => item.id == p_user.id).FirstOrDefault());
                    Session["user"] = userInfor;
                    return RedirectToAction("editProfile");
                }
                ViewBag.is_user_id = id;
                return View();
            }
            else return RedirectToAction("login");
        }
        //

        // edit user profile
        public class CreateViewModel
        {
            public user_profile user_profile { get; set; }
        }

        private string SaveImage(HttpPostedFileBase imageFile)
        {
            if (imageFile != null && imageFile.ContentLength > 0)
            {
                string newFileName = Guid.NewGuid().ToString(); 
                string fileExtension = Path.GetExtension(imageFile.FileName);
                string fileName = newFileName + fileExtension;
                string uploadsFolder = "~/Uploads/";
                string filePath = Path.Combine(Server.MapPath(uploadsFolder), fileName);
                imageFile.SaveAs(filePath);
                // Trả về đường dẫn URL của ảnh trên môi trường web
                return VirtualPathUtility.ToAbsolute(Path.Combine(uploadsFolder, fileName));
            }
            return null;
        }

        private void DeleteImage(string imagePath)
        {
            if(!IsImageUrl(imagePath))
            {
                string physicalPath = ControllerContext.HttpContext.Server.MapPath(imagePath);
                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }
            }
           
        }

        public ActionResult editProfile()
        {
            if (Session["user"] != null)
                return View();
            else return RedirectToAction("login");
        }
        [HttpPost]
        [UserAuthorize(idChucNang = 10)]
        public ActionResult editProfile(CreateViewModel viewModel , HttpPostedFileBase userAvatar)
        {
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];

            user_profile up = db.user_profile.Where(item => item.userID == p_user.id).FirstOrDefault();

            up.user_fun_fact = viewModel.user_profile.user_fun_fact;
            up.user_about = viewModel.user_profile.user_about;
            up.user_time_spend = viewModel.user_profile.user_time_spend;
            up.user_biography_title = viewModel.user_profile.user_biography_title;
            up.user_birthday = viewModel.user_profile.user_birthday;
            up.user_obsessed_with = viewModel.user_profile.user_obsessed_with;
            up.user_favourite_song = viewModel.user_profile.user_favourite_song;
            up.user_pets = viewModel.user_profile.user_pets;
            up.user_unless_skill = viewModel.user_profile.user_unless_skill;
            up.user_work = viewModel.user_profile.user_work;
            up.user_school = viewModel.user_profile.user_school;
            up.user_address = viewModel.user_profile.user_address;
            if (userAvatar != null && userAvatar.ContentLength > 0)
            {
                // Lưu ảnh đã tải lên vào thư mục hoặc cơ sở dữ liệu
                if(up.user_avatar != null)
                {
                    DeleteImage(up.user_avatar);
                }
                up.user_avatar = SaveImage(userAvatar);
            }
            db.SaveChanges();
            var userInfor = (db.users.Where(item => item.id == p_user.id).FirstOrDefault());
            Session["user"] = userInfor;
            return RedirectToAction("profile", "HomeUser", new { id = p_user.id });
        }
        [HttpPost]
        [UserAuthorize(idChucNang = 10)]
        public JsonResult addUserLanguages(List<int> list_id)
        {
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];

            if(list_id != null)
            {
                var selectedLanguages = db.users_languages.Where(item => item.userID == p_user.id).ToList();
                foreach (var language in selectedLanguages)
                {
                    if (!list_id.Contains((int)language.language_id))
                    {
                        db.users_languages.Remove(language);
                    }
                }
                foreach (var id in list_id)
                {
                    var findUserLanguage = db.users_languages.FirstOrDefault(item => item.userID == p_user.id && item.language_id == id);
                    if (findUserLanguage == null)
                    {
                        findUserLanguage = new users_languages
                        {
                            userID = p_user.id,
                            language_id = id
                        };
                        db.users_languages.Add(findUserLanguage);
                    }
                }
            }
            else
            {
                var tLanguage = db.users_languages.Where(item => item.userID == p_user.id).ToList();
                db.users_languages.RemoveRange(tLanguage);
            }
            db.SaveChanges();
            var userInfor = (db.users.Where(item => item.id == p_user.id).FirstOrDefault());
            Session["user"] = userInfor;
            return Json("true", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [UserAuthorize(idChucNang = 10)]
        public JsonResult addUserInterests(List<int> list_id)
        {
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
            if(list_id != null)
            {
                var selectedInterests = db.users_interests.Where(item => item.userid == p_user.id).ToList();
                foreach (var interest in selectedInterests)
                {
                    if (!list_id.Contains((int)interest.interest_id))
                    {
                        db.users_interests.Remove(interest);
                    }
                }
                foreach (var id in list_id)
                {
                    var findUserInterest = db.users_interests.FirstOrDefault(item => item.userid == p_user.id && item.interest_id == id);
                    if (findUserInterest == null)
                    {
                        findUserInterest = new users_interests
                        {
                            userid = p_user.id,
                            interest_id = id,
                            uitr_status = 1,
                        };
                        db.users_interests.Add(findUserInterest);
                    }
                }
            }
            else
            {
                var tInterests = db.users_interests.Where(item => item.userid == p_user.id).ToList();
                db.users_interests.RemoveRange(tInterests);
            }
            db.SaveChanges();
            var userInfor = (db.users.Where(item => item.id == p_user.id).FirstOrDefault());
            Session["user"] = userInfor;
            return Json("true", JsonRequestBehavior.AllowGet);
        }
        //user personal profile 
        public ActionResult userPersonalProfile(bool error = false)
        {
            if (error == true) ViewBag.Error = error;
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
            if(p_user != null)
            {
                if (!(db.user_personalInfor.Any(item => item.userID == p_user.id)))
                {
                    user_personalInfor new_personal = new user_personalInfor();
                    new_personal.userID = p_user.id;
                    db.user_personalInfor.Add(new_personal);
                    db.SaveChanges();
                    var userInfor = (db.users.Where(item => item.id == p_user.id).FirstOrDefault());
                    Session["user"] = userInfor;
                }
                if (!(db.governmentIDs.Any(item => item.userID == p_user.id)))
                {
                    governmentID new_gvid = new governmentID();
                    new_gvid.userID = p_user.id;
                    db.governmentIDs.Add(new_gvid);
                    db.SaveChanges();
                    var userInfor = (db.users.Where(item => item.id == p_user.id).FirstOrDefault());
                    Session["user"] = userInfor;
                }
            }
            else
            {
                return RedirectToAction("login", "homeuser", new { area = "user" });
            }    
           
            return View();
        }
        [HttpGet]
        public JsonResult GetCountries()
        {
            PPL3Entities db = new PPL3Entities();
            var countries = db.countries
                           .Select(s => new {
                               id = s.id,
                               country_name = s.ct_name
                           }).ToList();
            return Json(countries, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetStates(int countryId)
        {
            PPL3Entities db = new PPL3Entities();
            var states = db.states
                           .Where(s => s.country_id == countryId)
                           .Select(s => new {
                               id = s.id,
                               state_name = s.state_name
                           }).ToList();
            return Json(states, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetCities(int stateId)
        {
            PPL3Entities db = new PPL3Entities();
            var cities = db.cities
                           .Where(s => s.state_id == stateId)
                           .Select(s => new {
                               id = s.id,
                               city_name = s.city_name
                           }).ToList();
            return Json(cities, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [UserAuthorize(idChucNang = 12)]
        public JsonResult addLegalName(string first_name, string last_name)
        {
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
            user_personalInfor tmpprofile = db.user_personalInfor.Where(item => item.userID == p_user.id).FirstOrDefault();
            tmpprofile.legal_name = first_name + " " + last_name;
            tmpprofile.first_name = first_name;
            tmpprofile.last_name = last_name;
            db.SaveChanges();
            var userInfor = (db.users.Where(item => item.id == p_user.id).FirstOrDefault());
            Session["user"] = userInfor;
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [UserAuthorize(idChucNang = 12)]
        public JsonResult addUserPhone(string phone_number)
        {
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
            phone_number find_phone = db.phone_number.Where(item => item.userID == p_user.id).FirstOrDefault();
            if (find_phone != null)
            {
                db.phone_number.Remove(find_phone);
                phone_number newphone = new phone_number()
                {
                    userID = p_user.id,
                    phone = phone_number
                };
                db.phone_number.Add(newphone);
                db.SaveChanges();
                var userInfor = (db.users.Where(item => item.id == p_user.id).FirstOrDefault());
                Session["user"] = userInfor;
            }
            else
            {
                phone_number newphone = new phone_number()
                {
                    userID = p_user.id,
                    phone = phone_number
                };
                db.phone_number.Add(newphone);
                db.SaveChanges();
                var userInfor = (db.users.Where(item => item.id == p_user.id).FirstOrDefault());
                Session["user"] = userInfor;
            }
            return Json("true", JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        [UserAuthorize(idChucNang = 12)]
        public JsonResult addUserIDCard(string date_range, string Expiration_date, string cccd_number)
        {
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
            governmentID up = db.governmentIDs.Where(item => item.userID == p_user.id).FirstOrDefault();
            if (date_range != null)
            {
                DateTime tmpdate = DateTime.ParseExact(date_range, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                up.date_range = tmpdate;
            }
            if (Expiration_date != null)
            {
                DateTime tmpdate = DateTime.ParseExact(Expiration_date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                up.expiration_date = tmpdate;
            }
            if (cccd_number != null)
            {
                up.number_card = cccd_number;
            }
            db.SaveChanges();
            var userInfor = (db.users.Where(item => item.id == p_user.id).FirstOrDefault());
            Session["user"] = userInfor;
            return Json("true", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [UserAuthorize(idChucNang = 12)]
        public JsonResult addimgGVID1(string img)
        {
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
            governmentID up = db.governmentIDs.Where(item => item.userID == p_user.id).FirstOrDefault();
            up.backof_id_card = img;
            db.SaveChanges();
            var userInfor = (db.users.Where(item => item.id == p_user.id).FirstOrDefault());
            Session["user"] = userInfor;
            return Json("true", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [UserAuthorize(idChucNang = 12)]
        public JsonResult addimgGVID2(string img)
        {
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
            governmentID up = db.governmentIDs.Where(item => item.userID == p_user.id).FirstOrDefault();
            up.identity_card = img;
            db.SaveChanges();
            var userInfor = (db.users.Where(item => item.id == p_user.id).FirstOrDefault());
            Session["user"] = userInfor;
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [UserAuthorize(idChucNang = 12)]
        public JsonResult addEmergencyContact(string ec_name, string relationship, string ec_language, string ec_email, string ec_country, string ec_phone_number)
        {
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
            emergency_contact e = db.emergency_contact.Where(item => item.userid == p_user.id).FirstOrDefault();
            if (ec_phone_number.Length > 20 || !Regex.IsMatch(ec_phone_number, @"^\d+$")) ec_phone_number = "";
            if(e != null)
            {
                e.ec_name = ec_name;
                e.phone_number = ec_phone_number;
                e.email = ec_email;
                e.country = ec_country;
                e.relationship = relationship;
                e.ec_language = ec_language;
                db.SaveChanges();
                var userInfor = (db.users.Where(item => item.id == p_user.id).FirstOrDefault());
                Session["user"] = userInfor;
            }
            else
            {
                emergency_contact newe = new emergency_contact()
                {
                    userid = p_user.id,
                    ec_name = ec_name,
                    phone_number = ec_phone_number,
                    relationship = relationship,
                    ec_language = ec_language,
                    email = ec_email,
                    country = ec_country,
                };
                db.emergency_contact.Add(newe);
                db.SaveChanges();
                var userInfor = (db.users.Where(item => item.id == p_user.id).FirstOrDefault());
                Session["user"] = userInfor;
            }
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [UserAuthorize(idChucNang = 12)]
        public JsonResult addAddressCtc(int country_id, int state_id, int city_id)
        {
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
            user_personalInfor tmpprofile = db.user_personalInfor.Where(item => item.userID == p_user.id).FirstOrDefault();
            tmpprofile.country_id = country_id;
            tmpprofile.u_state = state_id;
            tmpprofile.u_city = city_id;
            db.SaveChanges();
            var userInfor = (db.users.Where(item => item.id == p_user.id).FirstOrDefault());
            Session["user"] = userInfor;
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        // Login and Logout and Sign up
        public ActionResult Login()
        {
            user new_user = new user();
            return View(new_user);
        }

        [HttpPost]
        public ActionResult Login(user model , string email_address)
        {
            ViewBag.email_address = email_address;
            ViewBag.signUpOrLogin = 2;
            PPL3Entities entities = new PPL3Entities();
            user_personalInfor find_user = entities.user_personalInfor.Where(item => item.email_address == email_address).FirstOrDefault();
            foreach (var item in entities.properties.ToList())
            {
                if (item.end_date < DateTime.Now)
                {
                    item.availability_type = 0;
                    item.current_pages = 10;
                }
            }
            entities.SaveChanges();
            if (!IsGmailExists(email_address))

            {

                ModelState.AddModelError("checkGmail", "This Gmail does not exist");

                return View(model);

            }
            else if (!IsPasswordExists(model.user_password , (int)find_user.userID))

            {

                ModelState.AddModelError("Password", "The password you entered is incorrect");
                return View(model);

            }

            else
            {
                
                var id_user = entities.user_personalInfor.Where(item => item.email_address == email_address).FirstOrDefault().userID;
                var userInfor = (entities.users.Where(item => item.id ==  id_user).FirstOrDefault());
                userInfor.user_status = 1;
                entities.SaveChanges();
                if(userInfor.user_type != 1) Session["user"] = userInfor;
                else if(userInfor.user_type == 1) Session["admin"] = userInfor;
                TempData["check"] = false;
                if (Session["user"] != null)  return RedirectToAction("Index", "HomeUser", new { area = "User", id = -1, checkID = -1 });
                else
                {
                    return RedirectToAction("Index", "HomeAdmin", new { area = "Admin"});
                }

            }
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
            return RedirectToAction("Index", "home", new { area = "" , checkLogOut = true});
        }

        public ActionResult SignUp()
        {
            user new_user = new user();
            return View(new_user);
           
        }

        [HttpPost]
        public ActionResult SignUp(user model , string email_address)
        {
            ViewBag.email_address = email_address;
            ViewBag.signUpOrLogin = 1;
            user_personalInfor user_PersonalInfor = new user_personalInfor();
            user_notification user_Notification = new user_notification();
            PPL3Entities db = new PPL3Entities();
            foreach (var item in db.properties.ToList())
            {
                if (item.end_date < DateTime.Now)
                {
                    item.availability_type = 0;
                    item.current_pages = 10;
                }
            }
            db.SaveChanges();
            if (IsGmailExists(email_address))

            {

                ModelState.AddModelError("Gmail", "Gmail is already in use");

                return View(model);

            }
            else if(!IsCorrectPassportCode(model.passport_code))
            {
                ModelState.AddModelError("Passport", "Passport Invalid");
                return View(model);
            }
            else
            {
                if (model.user_type == null) model.user_type = 2;
                model.created = DateTime.Now;
                user_Notification.userid = model.id;
                user_Notification.created = DateTime.Now;
                user_Notification.content = "Registration successful! Please complete your profile.";
                user_Notification.un_status = 0;
                user_Notification.un_url = "/user/homeuser/profile?id=" + model.id;
                db.user_notification.Add(user_Notification);
                user_PersonalInfor.email_address = email_address;
                user_PersonalInfor.userID = model.id;
                db.user_personalInfor.Add(user_PersonalInfor);
                db.users.Add(model);
                db.SaveChanges();
                
                var id_user = db.user_personalInfor.Where(item => item.email_address == email_address).FirstOrDefault().userID;
                var userInfor = (db.users.Where(item => item.id == id_user).FirstOrDefault());
                userInfor.user_status = 1;
                db.SaveChanges();
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




        // Detail
        public ActionResult Detail(int id , int bookingId = -1)
        {
            if (Session["user"] != null )
            {
                if (bookingId != -1) ViewBag.bookingId = bookingId;
                PPL3Entities db = new PPL3Entities();
                List<booking> bookings = new List<booking>();
                ViewBag.propertyFind = db.properties.Where(item => item.id == id).FirstOrDefault();
                if (bookingId != -1) bookings = db.bookings.Where(item => item.property_id == id && item.id != bookingId).ToList();
                else bookings = db.bookings.Where(item => item.property_id == id).ToList();
                string date = "";
                foreach (var item in bookings)
                 if(item.pay_status == 1)
                {
                    date = date + item.check_in_date.Value.ToString("dd/MM/yyyy") + "-" + item.check_out_date.Value.ToString("dd/MM/yyyy") + ",";
                }
                if(date.Trim() != "") date = date.Substring(0, date.Length - 1);
                ViewBag.dataJson = JsonConvert.SerializeObject(date);
                return View();
            }
            else
            {
                return RedirectToAction("login", "homeuser", new { area = "user" });
            }
           
        }


        // trip

        public ActionResult Trip()
        {
           
            if (Session["user"] != null)
            {

                PPL3Entities db = new PPL3Entities();

                DateTime now = DateTime.Now;

                List<booking> bookingsToRemove = new List<booking>();

                user p_user = (user)Session["user"];

                foreach (var item in db.bookings.ToList())

                {

                    TimeSpan duration = (TimeSpan)(item.check_in_date - now);

                    int daysDifference = (int)duration.TotalDays;

                    if ((item.check_out_date < now) || (daysDifference <= 2 && daysDifference >= 0 && item.pay_status == 0))

                    {

                        bookingsToRemove.Add(item);

                    }
                    

                }

                foreach (var itemToRemove in bookingsToRemove)

                {

                    db.bookings.Remove(itemToRemove);

                }

                db.SaveChanges();

                var list_booking_hotel = db.bookings.Where(item => item.userId == p_user.id)

                                                    .OrderBy(item => item.check_in_date)

                                                    .ThenBy(item => item.check_out_date)

                                                    .ThenBy(item => item.id)

                                                    .ToList();

                return View(list_booking_hotel);
            }
            else return RedirectToAction("login");
        }

        [HttpPost]
        [UserAuthorize(idChucNang = 11)]
        public JsonResult FixBookingHotel(int bookingId , DateTime check_in_date, DateTime check_out_date, int[] guest_count , int hotelId , decimal price_hotel)
        {
            if (check_in_date > check_out_date) return  Json("error1", JsonRequestBehavior.AllowGet);
          
           
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
            booking find_hotel = db.bookings.Where(item => item.id == bookingId).FirstOrDefault();
            if (check_in_date == null) check_in_date = (DateTime)find_hotel.check_in_date;
            if (check_out_date == null) check_out_date = (DateTime)find_hotel.check_out_date;
            List<booking> list_booking = db.bookings.Where(item => item.id != bookingId).ToList();
            if (list_booking.Any(item => item.pay_status == 1 && ((check_in_date >= item.check_in_date && check_in_date <= item.check_out_date) || (check_out_date >= item.check_in_date && check_out_date <= item.check_out_date) || (check_in_date <= item.check_in_date && check_out_date >= item.check_out_date)) && item.property_id == find_hotel.property_id))
            {
                return Json("error3", JsonRequestBehavior.AllowGet);
            }
            if ((list_booking.Any(item => ((item.check_in_date <= check_in_date && item.check_out_date >= check_in_date) || (item.check_out_date >= check_out_date && item.check_in_date <= check_out_date) ||(check_in_date < item.check_in_date && check_out_date > item.check_out_date)) && item.userId == p_user.id && item.property_id == hotelId && item.pay_status == 1) == false && check_in_date >= DateTime.Now) || (find_hotel.check_in_date == check_in_date && find_hotel.check_out_date == check_out_date)) { 
                find_hotel.check_in_date = check_in_date;
                find_hotel.check_out_date = check_out_date;
                TimeSpan difference = (find_hotel.check_out_date.Value - find_hotel.check_in_date.Value);
                int daysDifference = (int)difference.TotalDays;
                find_hotel.price_per_day = price_hotel / daysDifference;
                find_hotel.amount_paid = price_hotel;
                db.SaveChanges();
                while (db.booking_guests.Any(item => item.booking_id == bookingId))
                {
                    booking_guests find_booking_guest = db.booking_guests.Where(item => item.booking_id == bookingId).FirstOrDefault();
                    db.booking_guests.Remove(find_booking_guest);
                    db.SaveChanges();
                }
                for (int i = 0; i <= guest_count.Length - 1; ++i)
                {
                    booking_guests booking_Guests = new booking_guests();
                    booking_Guests.booking_id = find_hotel.id;
                    booking_Guests.guest_type_id = i + 1;
                    booking_Guests.num_guests = guest_count[i];
                    db.booking_guests.Add(booking_Guests);
                }
                db.SaveChanges();
                var url = UrlPayment(bookingId);

                return Json(new
                {
                    success = "true",
                    redirectUrl = url,
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("error1", JsonRequestBehavior.AllowGet);
            }

        }

        //notification
        public ActionResult notification()
        {
            if (Session["user"] != null)
            {
                user p_user = (user)Session["user"];
                PPL3Entities db = new PPL3Entities();
                foreach(var item in db.user_notification.ToList())
                {
                    item.un_status = 1;
                }
                db.SaveChanges();
                List<user_notification> un = db.user_notification.Where(item => item.userid == p_user.id).ToList();
                return View(un);
            }
            else return RedirectToAction("login");
        }

        [HttpPost]
        public JsonResult DeleteNotification(int id)
        {
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
            user_notification un = db.user_notification.FirstOrDefault(item => item.id == id);
            db.user_notification.Remove(un);
            db.SaveChanges();
            var userInfor = (db.users.Where(item => item.id == p_user.id).FirstOrDefault());
            Session["user"] = userInfor;
            var data = new
            {
                success = "true",
                count = db.user_notification.Where(item => item.userid == p_user.id).Count(),
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //Pay Hotel
        [HttpPost]
        [UserAuthorize(idChucNang = 3)]
        public JsonResult PayHotel(bool checkBook , int id , DateTime checkInDate , DateTime checkOutDate , int[] guest_count , decimal price_hotel)
        {
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
            if (checkInDate > checkOutDate) return Json(new
            {
                success = "error1"
            }, JsonRequestBehavior.AllowGet);
          
            if (db.bookings.Any(item => item.pay_status == 1 && ((checkInDate >= item.check_in_date && checkInDate <= item.check_out_date) || (checkOutDate >= item.check_in_date && checkOutDate <= item.check_out_date) || (checkInDate <= item.check_in_date && checkOutDate >= item.check_out_date)) && item.property_id == id))
            {
                return Json(new
                {
                    success = "error1"
                }, JsonRequestBehavior.AllowGet);
            }
           
            
            DateTime now = DateTime.Now;
            foreach (var item in db.bookings)
            {
                TimeSpan duration = (TimeSpan)(item.check_in_date - now); // Tính toán khoảng cách
                int daysDifference = (int)duration.TotalDays; // Chuyển khoảng cách thành số ngày
                if ((item.check_out_date < now) || (daysDifference <= 2 && daysDifference >= 0 && item.pay_status == 0))
                {
                    db.bookings.Remove(item);
                    db.SaveChanges();
                }
            }
            if (db.bookings.Any(item => ((item.check_in_date <= checkInDate && item.check_out_date >= checkInDate) || (item.check_out_date >= checkOutDate && item.check_in_date <= checkOutDate) || (checkInDate < item.check_in_date && checkOutDate > item.check_out_date)) && item.userId ==  p_user.id && item.property_id == id && item.pay_status == 1) == false && checkInDate > DateTime.Now)
            {
                TempData["checkBook"] = checkBook;
                property findHotel = db.properties.Where(item => item.id == id).FirstOrDefault();
                booking userBooking = new booking();
                userBooking.property_id = id;
                userBooking.userId = p_user.id;
                userBooking.check_in_date = checkInDate;
                userBooking.check_out_date = checkOutDate;
                TimeSpan difference = (userBooking.check_out_date.Value - userBooking.check_in_date.Value);
                int daysDifference = (int)difference.TotalDays;
                userBooking.price_per_day = price_hotel / daysDifference;
              

               
                userBooking.amount_paid = (decimal)price_hotel;
                userBooking.booking_date = DateTime.Now;
                userBooking.is_refund = 1;
                userBooking.booking_status = 1;
                userBooking.created = DateTime.Now;
                userBooking.pay_status = 0;
                db.bookings.Add(userBooking);
                db.SaveChanges();
                for (int i = 0; i <= guest_count.Length - 1; ++i)
                {
                    booking_guests booking_Guests = new booking_guests();
                    booking_Guests.booking_id = userBooking.id;
                    booking_Guests.guest_type_id = i + 1;
                    booking_Guests.num_guests = guest_count[i];
                    db.booking_guests.Add(booking_Guests);
                }
                db.SaveChanges();

                // pay hotel 

                var url = UrlPayment(userBooking.id);

                return Json(new
                {
                    success = "true",
                    redirectUrl = url,
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    success = "error1"
                }, JsonRequestBehavior.AllowGet);

            }
        }




        // Thanh toán Vnpay

        [UserAuthorize(idChucNang = 8)]
        public ActionResult PaymentSuccess()
        {
            ViewBag.Success = TempData["message"];
            return View();
        }

        [UserAuthorize(idChucNang = 8)]
        public ActionResult PaymentFail()
        {
            ViewBag.fail = TempData["message"];
            return View();
        }
    
        public ActionResult VnpayReturn()
        {
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }

                }
                long invoiceId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.QueryString["vnp_BankCode"];
                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        //Thanh toan thanh cong
                        TempData["message"] = "The transaction was performed successfully. Thank you for using the service";
                        var invoiceItem = db.bookings.FirstOrDefault(x => x.id == (int)invoiceId);
                        invoiceItem.pay_status = 1;

                        user_notification user_Notification = new user_notification();
                        user_Notification.userid = p_user.id;
                        user_Notification.created = DateTime.Now;
                        user_Notification.content = "Payment successful!";
                        user_Notification.un_status = 0;
                        user_Notification.un_url = "#";
                        db.user_notification.Add(user_Notification);
                        db.SaveChanges();

                        // kiểm tra coi hóa đơn có tồn tại hay không

                        if(invoiceItem.transaction_id != null)
                        {
                            transaction find_transaction = db.transactions.FirstOrDefault(item => item.id == invoiceItem.transaction_id);
                            find_transaction.transaction_status = 1;
                            find_transaction.transfer_on = DateTime.Now;

                        }
                        else {
                            // thêm hóa đơn
                            transaction newTransaction = new transaction();
                            newTransaction.property_id = invoiceItem.property_id;
                            newTransaction.booking_id = invoiceItem.id;
                            newTransaction.amount = invoiceItem.amount_paid;
                            newTransaction.transfer_on = DateTime.Now;
                            newTransaction.currency_id = 1;
                            newTransaction.transaction_status = 1;
                            newTransaction.receiver_id = invoiceItem.property.userId;
                            newTransaction.payer_id = p_user.id;
                            newTransaction.site_fees = (decimal?)((int)invoiceItem.amount_paid * 0.01);
                            db.transactions.Add(newTransaction);
                            invoiceItem.transaction_id = newTransaction.id;
                           
                        }
                        db.SaveChanges();
                        transaction transaction = db.transactions.FirstOrDefault(item => item.id == invoiceItem.transaction_id);
                        string emailHtml = RenderRazorViewToString("Invoice", transaction);
                        SendEmail("hosithao1622004@gmail.com", "Invoice for Transaction #" + transaction.id , emailHtml);

                      


                        return RedirectToAction("PaymentSuccess", "homeuser", new { area = "User" });
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        TempData["message"] = "An error occurred during processing. Error code: " + vnp_ResponseCode;
                        var invoiceItem = db.bookings.FirstOrDefault(x => x.id == (int)invoiceId);
                        invoiceItem.pay_status = 0;
                        db.SaveChanges();

                        return RedirectToAction("PaymentFail", "homeuser", new { area = "User" });
                        //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                    }
                    //    displayTmnCode.InnerText = "Mã Website (Terminal ID):" + TerminalID;
                    //    displayTxnRef.InnerText = "Mã giao dịch thanh toán:" + orderId.ToString();
                    //    displayVnpayTranNo.InnerText = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();
                    //    displayAmount.InnerText = "Số tiền thanh toán (VND):" + vnp_Amount.ToString();
                    //    displayBankCode.InnerText = "Ngân hàng thanh toán:" + bankCode;
                }
            }
            return View();
        }

        public string UrlPayment(int orderCode)
        {
            PPL3Entities db = new PPL3Entities();
            var urlPayment = "";
            var order = db.bookings.FirstOrDefault(item => item.id == orderCode);
            //Get Config Info
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key
            

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            var Price = (long)order.amount_paid * 2500000;
            Price = (long)(Price + Price * 0.01);
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", Price.ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            //vnpay.AddRequestData("vnp_CreateDate", DateTime.UtcNow.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng :" + order.id);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.id.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            //log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            return urlPayment;
        }

        // Message 
        [UserAuthorize(idChucNang = 13)]
        public ActionResult ChatUser(int openUser = -1)
        {
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
            List<Friendship> list_friends = new List<Friendship>();
          
            foreach (var item in db.Friendships.Where(item => item.UserId1 == p_user.id || item.UserId2 == p_user.id))
            {
                list_friends.Add(item);
            }
            ViewBag.openUser = openUser;
            return View(list_friends);
        }


        [HttpPost]

        public ActionResult SendMessage(int senderId, int receiverId, string content)

        {
            PPL3Entities db = new PPL3Entities();
            // Lưu tin nhắn vào cơ sở dữ liệu

            Message newMessage = new Message

            {

                SenderID = senderId,

                ReceiverID = receiverId,

                MessageContent = content,

                Timestamp = DateTime.Now

            };

            db.Messages.Add(newMessage);

            db.SaveChanges();

            return RedirectToAction("ChatUser");

        }

        public ActionResult GetMessages(int senderId, int receiverId)

        {
            PPL3Entities db = new PPL3Entities();
            // Lấy danh sách tin nhắn giữa sender và receiver

            var messages = db.Messages.Where(m => (m.SenderID == senderId && m.ReceiverID == receiverId) || (m.SenderID == receiverId && m.ReceiverID == senderId))
                          .OrderBy(m => m.ID)
                          .Select(s => new {
                              id = s.ID,
                              SenderID = s.SenderID,
                              ReceiverID = s.ReceiverID,
                              MessageContent = s.MessageContent,
                              Timestamp = s.Timestamp,
                          })
                          .ToList();


            return Json(messages, JsonRequestBehavior.AllowGet);

        }

        public ActionResult BlockUser(int userID) {
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
            while(db.Messages.Any(item => (item.SenderID == p_user.id && item.ReceiverID == userID) || (item.SenderID == userID && item.ReceiverID == p_user.id)))
            {
                Message find_message = db.Messages.FirstOrDefault(item => (item.SenderID == p_user.id && item.ReceiverID == userID) || (item.SenderID == userID && item.ReceiverID == p_user.id));
                db.Messages.Remove(find_message);
                db.SaveChanges();
            }
            db.Friendships.Remove(db.Friendships.FirstOrDefault(item => (item.UserId1 == p_user.id && item.UserId2 == userID) || (item.UserId1 == userID && item.UserId2 == p_user.id)));
            db.SaveChanges();
            return RedirectToAction("ChatUser");
        }


        public JsonResult SearchUser(string content)
        {
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];

            List<Friendship> list_friends = new List<Friendship>();

            foreach (var item in db.Friendships.Where(item => item.UserId1 == p_user.id || item.UserId2 == p_user.id))
            {
                list_friends.Add(item);
            }

            var friendships = from item in list_friends
                              where (item.UserId1 != p_user.id && Unicode(item.user.user_personalInfor.FirstOrDefault().legal_name).Contains(content)) || (item.UserId2 != p_user.id && Unicode(item.user1.user_personalInfor.FirstOrDefault().legal_name).Contains(content))
                              select new
                              {
                                  item.Id,
                                  item.UserId1,
                                  item.UserId2,
                                  item.Status
                              };
            return Json(friendships , JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddFriend(int userID)
        {
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];

            if(!db.Friendships.Any(item => (p_user.id == item.UserId1 && item.UserId2 == userID) ||(item.UserId1 == userID && p_user.id == item.UserId2))) {
                Friendship newFriendship = new Friendship();
                newFriendship.UserId1 = p_user.id;
                newFriendship.UserId2 = userID;
                newFriendship.Status = 1;
                db.Friendships.Add(newFriendship);
                db.SaveChanges();
            }
            return RedirectToAction("ChatUser", "HomeUser", new { area = "User", openUser = userID });
        }
        //hoa don
        [UserAuthorize(idChucNang = 8)]
        public ActionResult invoice(int id)
        {
            PPL3Entities db = new PPL3Entities();
            transaction transaction = db.transactions.FirstOrDefault(item => item.id == id);
            return View(transaction);
        }
        private void SendEmail(string recipientEmail, string subject, string body)
        {
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress("thanhlongtivip@gmail.com"); 
                mailMessage.To.Add(recipientEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                string appPassword = "strm orqd egqw oqrx";
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.Credentials = new NetworkCredential("thanhlongtivip@gmail.com", appPassword);
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(mailMessage);
                }
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

        //Function

        public string Unicode(string originalString)
        {

            // Loại bỏ dấu Unicode
            var normalizedString = originalString.Normalize(NormalizationForm.FormKD);

            // Loại bỏ kí tự không phải kí tự chữ cái hoặc số
            var stringBuilder = new StringBuilder();
            foreach (var c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            // Chuỗi đã loại bỏ dấu Unicode
            return stringBuilder.ToString().ToLower();
        }
        public bool IsGmailExists(string email_address)

        {

            PPL3Entities db = new PPL3Entities();
            return db.user_personalInfor.Any(u => u.email_address== email_address);

        }
        public bool IsPasswordExists(string password , int id)

        {

            PPL3Entities db = new PPL3Entities();

            return db.users.Any(u => u.user_password == password && u.id == id);

        }
        public static bool IsCorrectPassportCode(string passportCode)
        {
            if (string.IsNullOrEmpty(passportCode))
            {
                return false;
            }
            string pattern = @"^[A-Za-z]{1,2}[0-9]{6,9}$";
            return Regex.IsMatch(passportCode, pattern);
        }
        public bool IsImageUrl(string imagePath)
        {
            // Kiểm tra xem đường dẫn có bắt đầu bằng "http://" hoặc "https://"
            if (imagePath.StartsWith("http://") || imagePath.StartsWith("https://"))
            {
                // Kiểm tra đuôi mở rộng của đường dẫn
                string extension = System.IO.Path.GetExtension(imagePath);
                if (!string.IsNullOrEmpty(extension))
                {
                    string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                    return imageExtensions.Contains(extension.ToLower());
                }
            }
            return false;
        }

        

    }
}
