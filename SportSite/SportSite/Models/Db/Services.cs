using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportSite.Models.Db
{
    public class Services
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Details { get; set; }
        public List<Coach>? coaches { get; set; } = new ();
        public string? Image { get; set; }
        public bool IsTypeSport { get; set; } = false;
    }
}
