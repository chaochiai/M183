using Google.Authenticator;
using M183.ViewModels;
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

            ViewBag.Message = "<h2>QR-Code:</h2> <br/><br/> " +
                "<img src='" + setupCode.QrCodeSetupImageUrl + "' /> <br/><br/> " +
                "<h2>Token for manual entry</h2> <br/>" +
                setupCode.ManualEntryKey;

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (loginViewModel.Username == "username" && loginViewModel.Password == "password")
            {
                TwoFactorAuthenticator twoFactorAuthenticator = new TwoFactorAuthenticator();
                bool isPinCorrect = twoFactorAuthenticator.ValidateTwoFactorPIN("My_Secret_Key", loginViewModel.TOTPToken);

                if (isPinCorrect)
                {
                    ViewBag.Message = "Log in credentials and token are correct";
                }
                else
                {
                    ViewBag.Message = "The token entered is incorrect";
                }
            }
            else
            {
                ViewBag.Message = "The credentials are incorrect";
            }
            return View();
        }
    }
}