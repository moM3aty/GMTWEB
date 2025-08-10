using System.ComponentModel.DataAnnotations;

namespace GMTWEB.Models
{
    public class Website
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string NameAr { get; set; } 

        [Required]
        [Url]
        public string Link { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    }
}
