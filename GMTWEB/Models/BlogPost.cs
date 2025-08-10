using System.ComponentModel.DataAnnotations;

namespace GMTWEB.Models
{
    public class BlogPost
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(200)]
        public string TitleAr { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string ContentAr { get; set; }

        public string? ImageUrl { get; set; } 

        public string? Author { get; set; }

        public DateTime DatePosted { get; set; } = DateTime.UtcNow;
    }
}
