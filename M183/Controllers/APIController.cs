using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M183.Controllers
{
    public class APIController : Controller
    {
        // GET: API
        public ActionResult Index()
        {
            return View();
        }
        public void CollectKeyLogging(string text)
        {

        }
        public void CollectUsernamePassword(string username, string password)
        {

        }
    }
}