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
    public class SingleSignOnController : Controller
    {
        
        // GET: SingleSignOn
        /// <summary>
        /// shows the index page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// shows the log in page
        /// the log in function is implemented on the shared view
        /// log out is implemented on the shared view as well
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// verify id token via google api
        /// </summary>
        /// <returns></returns>
        public JsonResult SSOTokenSignIn()
        {
            string tokenID = Request["tokenid"];
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/oauth2/v3/tokeninfo?id_token=" + tokenID);
            byte[] data = Encoding.ASCII.GetBytes("id_token" + tokenID);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.ContentLength = data.Length;
            using (Stream stream = httpWebRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string responseStream = new StreamReader(httpWebResponse.GetResponseStream()).ReadToEnd();
            return Json(responseStream);
        }
    }
}