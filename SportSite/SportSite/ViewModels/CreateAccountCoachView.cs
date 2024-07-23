using Microsoft.AspNetCore.Mvc;
using SportSite.Models.Db;
using System.ComponentModel.DataAnnotations;

namespace SportSite.ViewModels
{
    public class CreateAccountCoachView
    {
        public Account account { get; set; }
        [Required]
        [Remote(action: "IsCode", controller: "Home", ErrorMessage = "Error, incorrect code")]
        [Display(Name = "Enter a special code")]
        public Guid CreateCode { get; set; }
        [Required]
        public string? Details { get; set; }
        [Required]
        public string? IdService { get; set; }
    }
}
