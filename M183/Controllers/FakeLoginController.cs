using M183.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M183.Controllers
{
    public class FakeLoginController : Controller
    {
        // GET: FakeLogin
        /// <summary>
        /// Shows the index page
        /// Example of fake log in
        /// the function is implemented on the view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}