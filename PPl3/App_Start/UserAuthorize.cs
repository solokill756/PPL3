using PPl3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PPl3.App_Start
{
    public class UserAuthorize : AuthorizeAttribute
    {
        public int idChucNang {  get; set; }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //1. Check Session : Đã đăng nhập => cho thực hiện Filter
            // Ngược lại cho trở lại trang đăng nhập
            user userSession = (user)HttpContext.Current.Session["user"];
            if(userSession != null)
            {
                PPL3Entities3 db = new PPL3Entities3();
                var count = db.user_type_user_role.Count(m => m.user_type_id == userSession.user_type && m.user_role_id == idChucNang && m.user_type_id == 2);
                if(count != 0)
                {
                    return;
                }
                else
                {
                    var returnUrl = filterContext.RequestContext.HttpContext.Request.RawUrl;
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        Controller = "Home",
                        aciton = "Error",
                        area = "",
                        returnUrl = returnUrl.ToString()

                    }));
                }
            }
            else
            {
                var returnUrl = filterContext.RequestContext.HttpContext.Request.RawUrl;
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    Controller = "HonmeUser",
                    aciton = "Login",
                    area = "User",
                    returnUrl = returnUrl.ToString()

                }));
            }

        }
    }
}