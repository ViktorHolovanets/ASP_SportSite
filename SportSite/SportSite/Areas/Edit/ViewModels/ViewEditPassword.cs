using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SportSite.Areas.Edit.ViewModels
{
    public class ViewEditPassword
    {
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        [Remote(action: "IncorrectPassword", controller: "Home", ErrorMessage = "Incorrect password")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }
    }
}
