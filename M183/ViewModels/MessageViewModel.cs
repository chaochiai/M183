using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace M183.ViewModels
{
    public class MessageViewModel
    {
        [Required(ErrorMessage = "Please enter your message.")]
        public string Message { get; set; }
    }
}