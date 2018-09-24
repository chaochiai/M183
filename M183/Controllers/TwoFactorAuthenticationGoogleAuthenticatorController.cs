using Google.Authenticator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M183.Controllers
{
    public class TwoFactorAuthenticationGoogleAuthenticatorController : Controller
    {
        // GET: TwoFactorAuthenticationGoogleAuthenticator
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SetupAuthentication()
        {
            TwoFactorAuthenticator twoFactorAuthenticator = new TwoFactorAuthenticator();
            SetupCode setupCode = twoFactorAuthenticator.GenerateSetupCode("M183", "chantalochiaiit@gmail.com", "My_Secret_Key", 300, 300);

            string qrCodeImageUrl = setupCode.QrCodeSetupImageUrl;
            string manualEntrySetupCode = setupCode.ManualEntryKey;

            ViewBag.Message = "<h2>QR-Code:</h2> <br/><br/> <img src=''" + qrCodeImageUrl + " /> <br/><br/> <h2>Token for manual entry</h2> <br/>" + manualEntrySetupCode;

            return View();
        }

        [HttpPost]
        public ActionResult Login()
        {
            string username = Request["username"];
            string password = Request["password"];
            string token = Request["token"];

            if (username == "chaochiai" && password == "test")
            {
                TwoFactorAuthenticator twoFactorAuthenticator = new TwoFactorAuthenticator();
                bool isPinCorrect = twoFactorAuthenticator.ValidateTwoFactorPIN("My_Secret_Key", token);

                if (isPinCorrect)
                {
                    ViewBag.Message = "Log in credentials and token are correct";
                }
                else
                {
                    ViewBag.Message = "Log in credentials and token are wrong";
                }

            }
            else
            {
                ViewBag.Message = "Wrong credentials";
            }
            return View();
        }
    }
}