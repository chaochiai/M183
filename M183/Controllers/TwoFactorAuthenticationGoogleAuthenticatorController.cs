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
        /// <summary>
        /// shows the index page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// shows the set up authentication
        /// qr code
        /// and token
        /// </summary>
        /// <returns></returns>
        public ActionResult SetupAuthentication()
        {
            //generate token and qr code
            TwoFactorAuthenticator twoFactorAuthenticator = new TwoFactorAuthenticator();
            SetupCode setupCode = twoFactorAuthenticator.GenerateSetupCode("M183", "chantalochiaiit@gmail.com", "My_Secret_Key", 300, 300);

            //print
            ViewBag.Message = "<h2>QR-Code:</h2> <br/><br/> " +
                "<img src='" + setupCode.QrCodeSetupImageUrl + "' /> <br/><br/> " +
                "<h2>Token for manual entry</h2> <br/>" +
                setupCode.ManualEntryKey;

            return View();
        }

        /// <summary>
        /// show the log in view
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// checks if the credentials and the token are correct
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            //checks if the credentials are valid
            if (loginViewModel.Username == "username" && loginViewModel.Password == "password")
            {
                //validate the token entered
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