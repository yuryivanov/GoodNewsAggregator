using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Models.ViewModels.Account
{
    public class LoginViewModel
    {
        [Display(Name="Введите Email")] 
        //It's better than using <label>, cause replacing name here causes replacing name for all such fields
        [Required(ErrorMessage = "Введите Email")]      
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
