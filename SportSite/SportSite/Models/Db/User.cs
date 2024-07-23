using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportSite.Models.Db
{
    public enum Gender
    {
        Male,
        Female,
        Other
    }

    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        [Phone(ErrorMessage = "Incorrect phone")]
        public string Tel { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Incorrect address")]
        public string Email { get; set; }
        [Required]
        public Gender Gender { get; set; }
    }
}
