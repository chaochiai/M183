using M183.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace M183.Controllers
{
    public class TwoFactorAuthenticationSMSController : Controller
    {
        // GET: TwoFactorAuthentication
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        string token = "secret";

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (loginViewModel.Username == "username" && loginViewModel.Password == "password")
            {
                string to = "+41764174413";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://rest.nexmo.com/sms/json");
                byte[] data = Encoding.ASCII.GetBytes("api_key=cc9e52d5&api_secret=5vDn023j63OYNSk9&to="+ to + "&from=\"NEXMO\"&text=\"" + token + "\"");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                ViewBag.Message = responseString;
            }
            else
            {
                ViewBag.Message = "The credentials are incorrect";
                return View();
            }
            ModelState.Clear();
            return View("TokenLogin");
        }
        public ActionResult TokenLogin()
        {            
            return View();
        }

        [HttpPost]
        public ActionResult TokenLogin(LoginViewModel loginViewModel)
        {
            if (token.Equals(loginViewModel.TOTPToken))
            {
                ViewBag.Message = "The token entered is correct.";
            }
            else
            {
                ViewBag.Message = "The token entered is incorrect.";
            }
            return View();
        }
    }
}