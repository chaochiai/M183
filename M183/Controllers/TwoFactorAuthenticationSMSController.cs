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

        [HttpPost]
        public ActionResult Login()
        {
            string username = Request["username"];
            string password = Request["password"];

            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://rest.nexmo.com/sms/json");

                string secret = "secret";

                string postData = "api_key=cc9e52d5";
                postData += "&api_secret=5vDn023j63OYNSk9";
                postData += "&to=+41764174413";
                postData += "&from=\"NEXMO\"";
                postData += "&text=\"" + secret + "\"";

                byte[] data = Encoding.ASCII.GetBytes(postData);

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
                ViewBag.Message = "!!";
            }



            return View();
        }

        [HttpPost]
        public void TokenLogin()
        {
            string token = Request["token"];



        }
    }
}