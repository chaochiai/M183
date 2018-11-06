using M183.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M183.Controllers
{
    public class Lab1Controller : Controller
    {
        // GET: Lab1
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {

            if (Request.Cookies["UserProfile"] != null && !String.IsNullOrEmpty(Request.Cookies["UserProfile"].Value))
            {
                CreateUserProfileSession();
            }

            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                string cookieName = "UserProfile";
                if (loginViewModel.IsStayLoggedin)
                {
                    CreateHttpCookie(cookieName, loginViewModel.Username + loginViewModel.Password, DateTime.Now.AddDays(14));

                }
                else
                {
                    CreateHttpCookie(cookieName, "notStayLoggedIn", DateTime.Now.AddDays(14));
                }
                CreateUserProfileSession();
            }           

            return View(loginViewModel);
        }

        //logout is implemented on HomeController

        public void CreateHttpCookie(string name, string value, DateTime expiration)
        {
            HttpCookie httpCookie = new HttpCookie(name);
            httpCookie.Value = value;
            httpCookie.Expires = DateTime.Now.AddDays(14);
            Response.Cookies.Add(httpCookie);
        }

        public void CreateHttpCookie(string name, string value)
        {
            HttpCookie httpCookie = new HttpCookie(name);
            httpCookie.Value = value;
            Response.Cookies.Add(httpCookie);
        }

        public void CreateUserProfileSession()
        {
            if (Session["IsLoggedIn"] == null)
            {
                Session["IsLoggedIn"] = true;
            }
        }
    }
}