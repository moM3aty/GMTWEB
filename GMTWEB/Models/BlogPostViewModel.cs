using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GMTWEB.Models
{
    public class BlogPostViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "English Title")]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "English Content")]
        public string Content { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Arabic Title")]
        public string TitleAr { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Arabic Content")]
        public string ContentAr { get; set; }

        [Display(Name = "Blog Image")]
        public IFormFile? ImageFile { get; set; } 

        public string? ExistingImageUrl { get; set; } 
    }
}
