using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportSite.Models.Db
{
    public class Coach
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public Account Account { get; set; }
        [Required]
        public string? Details { get; set; }
        public Services?  typeSports { get; set; }
    }
}
