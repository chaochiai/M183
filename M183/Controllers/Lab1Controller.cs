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
        HomeController homeController = new HomeController();
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
                ViewBag.Message = "You are currently logged in";
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (loginViewModel.Username == "username" && loginViewModel.Password == "password")
            {
                if (loginViewModel.IsStayLoggedin)
                {
                    CreateHttpCookie("UserProfile", loginViewModel.Username + loginViewModel.Password, DateTime.Now.AddDays(14));
                }
                else
                {
                    CreateHttpCookie("UserProfile", "notStayLoggedIn", DateTime.Now.AddDays(14));
                }
                CreateUserProfileSession();
                ViewBag.Message = "Logged in successfully";
            }
            else
            {
                ViewBag.Message = "The entered credentials are incorrect";
            }     

            return View(loginViewModel);
        }
        private void CreateUserProfileSession()
        {
            System.Web.HttpContext.Current.Session["IsLoggedIn"] = true;
        }


        public void CreateHttpCookie(string name, string value, DateTime expiration)
        {
            HttpCookie httpCookie = new HttpCookie(name);
            httpCookie.Value = value;
            httpCookie.Expires = DateTime.Now.AddDays(14);
            Response.Cookies.Add(httpCookie);
        }
    }
}