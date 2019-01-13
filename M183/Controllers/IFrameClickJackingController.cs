using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M183.Controllers
{
    public class IFrameClickJackingController : Controller
    {
        // GET: IFrameClickJacking
        /// <summary>
        /// Shows the index page
        /// Example of ui redress attack
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}