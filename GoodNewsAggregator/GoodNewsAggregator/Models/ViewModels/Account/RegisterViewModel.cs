using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Models.ViewModels.Account
{
    public class RegisterViewModel
    {
        [EmailAddress(ErrorMessage = "Некорректный Email")]
        [Required(ErrorMessage = "Введите Email")]
        [DataType(DataType.EmailAddress)] // __@{}.{}
        [Remote("CheckEmail", "Account", ErrorMessage = "Аккаунт уже существует с таким email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите Имя")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)] //min 8 symbols
                                      //at leaset 1 lowercase letter, 1 uppercase, 1 digit, 1 specSymbol        
        public string Password { get; set; }

        [Required(ErrorMessage = "Повторите пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirmation { get; set; }
    }
}
