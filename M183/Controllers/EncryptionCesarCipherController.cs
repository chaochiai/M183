using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M183.Controllers
{
    public class EncryptionCesarCipherController : Controller
    {
        // GET: EncryptionCesarCipher
        /// <summary>
        /// Shows the index view
        /// the function is implemented on the view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}