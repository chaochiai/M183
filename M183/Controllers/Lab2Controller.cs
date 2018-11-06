using M183.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace M183.Controllers
{
    public class Lab2Controller : Controller
    {
        // GET: Lab2
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            var token = new Random().Next(9999999);
            Session["token"] = token;
            ViewBag.token = token;
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (loginViewModel.token == null || !loginViewModel.token.Equals(Session["token"]))
                throw new Exception("Wrong token");

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
            httpCookie.HttpOnly = true;
            httpCookie.Secure = true;
            httpCookie.Path = "localhost:49854";
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

        [HttpPost]
        public ActionResult XSSAttack()
        {
            XSSAttackViewModel xSSAttackViewModel = new XSSAttackViewModel() { Token = "randomToken" };
            return View(xSSAttackViewModel);
        }

        //[ValidateAntiForgeryToken]
        public ActionResult XSSAttack(string cookie)
        {
            
            return View();
        }

    }
}