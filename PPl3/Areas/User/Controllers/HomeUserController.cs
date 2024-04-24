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
                db.favourites.Add(fa);
                db.SaveChanges();
            }
            return Json("true", JsonRequestBehavior.AllowGet);
            
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
            return Json("true", JsonRequestBehavior.AllowGet);
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
            string physicalPath = ControllerContext.HttpContext.Server.MapPath(imagePath);
            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
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
        public ActionResult userPersonalProfile()
        {
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
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
            return View();
        }

        [HttpPost]
        public ActionResult Login(user model , string email_address)
        {
            PPL3Entities entities = new PPL3Entities();
            user_personalInfor find_user = entities.user_personalInfor.Where(item => item.email_address == email_address).FirstOrDefault();
            if (!IsGmailExists(email_address))

            {

                ModelState.AddModelError("checkGmail", "Gmail này không tồn tại");

                return View(model);

            }
            else if (!IsPasswordExists(model.user_password , (int)find_user.userID))

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
                if (Session["user"] != null)  return RedirectToAction("Index", "HomeUser", new { area = "User", id = -1, checkID = -1 });
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
            PPL3Entities db = new PPL3Entities();
            
            if (IsGmailExists(email_address))

            {

                ModelState.AddModelError("Gmail", "Gmail đã được sử dụng");

                return View(model);

            }
            else
            {
                if (model.user_type == null) model.user_type = 2;
                model.created = DateTime.Now;
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


        // Detail
        [UserAuthorize(idChucNang = 12)]
        public ActionResult Detail(int id , int bookingId = -1)
        {
                if (bookingId != -1) ViewBag.bookingId = bookingId;
                PPL3Entities db = new PPL3Entities();
                List<booking> bookings = new List<booking>();
                ViewBag.propertyFind = db.properties.Where(item => item.id == id).FirstOrDefault();
                if (bookingId != -1) bookings = db.bookings.Where(item => item.property_id == id && item.id != bookingId).ToList();
                else bookings = db.bookings.Where(item => item.property_id == id).ToList();
                string date = "";
                foreach (var item in bookings)
                {
                    date = date + item.check_in_date.Value.ToString("dd/MM/yyyy") + "-" + item.check_out_date.Value.ToString("dd/MM/yyyy") + ",";
                }
                date = date.Substring(0, date.Length - 1);
                ViewBag.dataJson = JsonConvert.SerializeObject(date);
                return View();
           
        }


        // trip

        public ActionResult Trip()
        {
           
            if (Session["user"] != null)
            {
                user p_user = (user)Session["user"];
                PPL3Entities db = new PPL3Entities();
                while(db.bookings.Any(item => item.check_out_date < DateTime.Now))
                {
                    booking find_booking = db.bookings.FirstOrDefault(item => item.check_out_date < DateTime.Now);
                    db.bookings.Remove(find_booking);
                    db.SaveChanges();
                }
                var list_booking_hotel = db.bookings.Where(item => item.userId == p_user.id).OrderBy(item => item.check_in_date).ThenBy(item => item.check_out_date).ThenBy(item => item.id).ToList();
                return View(list_booking_hotel);
            }
            else return RedirectToAction("login");
        }

        [HttpPost]
        [UserAuthorize(idChucNang = 11)]
        public JsonResult FixBookingHotel(int bookingId , DateTime check_in_date, DateTime check_out_date, int[] guest_count , int hotelId)
        {
            if (check_in_date > check_out_date) return  Json("error1", JsonRequestBehavior.AllowGet);
            if ((check_out_date.Month - check_in_date.Month) >= 2) return Json("error2", JsonRequestBehavior.AllowGet);
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
            booking find_hotel = db.bookings.Where(item => item.id == bookingId).FirstOrDefault();
            List<booking> list_booking = db.bookings.Where(item => item.id != bookingId).ToList();
            if ((list_booking.Any(item => ((item.check_in_date <= check_in_date && item.check_out_date >= check_in_date) || (item.check_out_date >= check_out_date && item.check_in_date <= check_out_date) ||(check_in_date < item.check_in_date && check_out_date > item.check_out_date)) && item.userId == p_user.id && item.property_id == hotelId) == false && check_in_date >= DateTime.Now) || (find_hotel.check_in_date == check_in_date && find_hotel.check_out_date == check_out_date)) { 
                find_hotel.check_in_date = check_in_date;
                find_hotel.check_out_date = check_out_date;
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
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("error1", JsonRequestBehavior.AllowGet);
            }

        }


       

        //Pay Hotel
        [HttpPost]
        [UserAuthorize(idChucNang = 3)]
        public JsonResult PayHotel(bool checkBook , int id , DateTime checkInDate , DateTime checkOutDate , int[] guest_count)
        {

            if (checkInDate > checkOutDate) return Json(new
            {
                success = "error1"
            }, JsonRequestBehavior.AllowGet);
            if ((checkOutDate.Month - checkInDate.Month) >= 2) return Json(new { success = "error2" }, JsonRequestBehavior.AllowGet);
            PPL3Entities db = new PPL3Entities();
            user p_user = (user)Session["user"];
            while (db.bookings.Any(item => item.check_out_date < DateTime.Now))
            {
                booking find_booking = db.bookings.FirstOrDefault(item => item.check_out_date < DateTime.Now);
                db.bookings.Remove(find_booking);
                db.SaveChanges();
            }
            if (db.bookings.Any(item => ((item.check_in_date <= checkInDate && item.check_out_date >= checkInDate) || (item.check_out_date >= checkOutDate && item.check_in_date <= checkOutDate) || (checkInDate < item.check_in_date && checkOutDate > item.check_out_date)) && item.userId ==  p_user.id && item.property_id == id) == false && checkInDate >= DateTime.Now)
            {
                TempData["checkBook"] = checkBook;
                property findHotel = db.properties.Where(item => item.id == id).FirstOrDefault();
                booking userBooking = new booking();
                userBooking.property_id = id;
                userBooking.userId = p_user.id;
                userBooking.check_in_date = checkInDate;
                userBooking.check_out_date = checkOutDate;
                userBooking.price_per_day = findHotel.price;
                TimeSpan difference = (userBooking.check_out_date.Value - userBooking.check_in_date.Value);
                int daysDifference = (int)difference.TotalDays;

                double sum = daysDifference * (double)userBooking.price_per_day;
                userBooking.amount_paid = (decimal)sum;
                userBooking.booking_date = DateTime.Now;
                userBooking.is_refund = 1;
                userBooking.booking_status = 1;
                db.bookings.Add(userBooking);
                for (int i = 0; i <= guest_count.Length - 1; ++i)
                {
                    booking_guests booking_Guests = new booking_guests();
                    booking_Guests.booking_id = userBooking.id;
                    booking_Guests.guest_type_id = i + 1;
                    booking_Guests.num_guests = guest_count[i];
                    db.booking_guests.Add(booking_Guests);
                }
                db.SaveChanges();
                return Json(new
                {
                    success = "true",
                    redirectUrl = Url.Action("Index", "Homeuser", new { area = "user" })
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


        //Function
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

    }
}
