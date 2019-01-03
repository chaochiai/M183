using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace M183.ViewModels
{
    public class LoginViewModel
    {        
        [Required(ErrorMessage="Please enter your username.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please enter your password.")]
        public string Password { get; set; }
        public bool IsStayLoggedin { get; set; }
        public int? token { get; set; }
        [Display(Name ="Token")]
        public string TOTPToken { get; set; }
    }
}
