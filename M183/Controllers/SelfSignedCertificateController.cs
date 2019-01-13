using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M183.Controllers
{
    public class SelfSignedCertificateController : Controller
    {
        // GET: SelfSignedCertificate
        /// <summary>
        /// Show the index view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //the cerficate is located on the view folder
            return View();
        }
    }
}