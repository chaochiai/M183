using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M183.Controllers
{
    public class KeyLoggerController : Controller
    {
        // GET: KeyLogger
        /// <summary>
        /// Shows the index page
        /// Example of key logging
        /// the function is implemented on the view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        
    }
}