using System.ComponentModel.DataAnnotations;

namespace GMTWEB.Models
{
    public class WebsiteViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "English Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Arabic Name")]
        public string NameAr { get; set; }

        [Required]
        [Url]
        public string Link { get; set; }

        [Required]
        [Display(Name = "Project Type")]
        public ProjectType Type { get; set; } 

        public IFormFile? ImageFile { get; set; }

        public string? ExistingImageUrl { get; set; }
    }
}
