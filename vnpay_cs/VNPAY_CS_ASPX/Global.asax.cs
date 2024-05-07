using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace VNPAY_CS_ASPX
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            ConfigLogging();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
        public void ConfigLogging()
        {
          //  string sLogFile = HttpRuntime.AppDomainAppPath + "log4net.config";
            string sLogFile = HttpContext.Current.Server.MapPath( "~/log4net.config");
            if ((System.IO.File.Exists(sLogFile)))
            {
                log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(sLogFile));
            }
        }
    }
}