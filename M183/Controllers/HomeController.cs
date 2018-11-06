using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M183.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logout()
        {
            DeleteHttpCookies();
            DeleteSession();

            return View("Index");
        }

        public void DeleteHttpCookies()
        { 
            //this doesnt work
            Response.Cookies.Remove("UserProfile");

            //only this
            Response.Cookies["UserProfile"].Expires = DateTime.Now.AddDays(-1);
        }
        public void DeleteSession()
        {
            Session["IsLoggedIn"] = null;            
        }

    }
}