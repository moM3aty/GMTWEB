using GMTWEB.Data;
using GMTWEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GMTWEB.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // --- Website Data ---
            var monthlyWebsiteData = new int[12];
            var websitesThisYear = await _context.Websites
                .Where(w => w.DateAdded.Year == DateTime.Now.Year)
                .ToListAsync();
            var websiteGroups = websitesThisYear.GroupBy(w => w.DateAdded.Month).ToDictionary(g => g.Key, g => g.Count());
            for (int i = 0; i < 12; i++)
            {
                monthlyWebsiteData[i] = websiteGroups.ContainsKey(i + 1) ? websiteGroups[i + 1] : 0;
            }

            // --- Blog Post Data ---
            var monthlyBlogPostData = new int[12];
            var blogPostsThisYear = await _context.BlogPosts
                .Where(b => b.DatePosted.Year == DateTime.Now.Year)
                .ToListAsync();
            var blogPostGroups = blogPostsThisYear.GroupBy(b => b.DatePosted.Month).ToDictionary(g => g.Key, g => g.Count());
            for (int i = 0; i < 12; i++)
            {
                monthlyBlogPostData[i] = blogPostGroups.ContainsKey(i + 1) ? blogPostGroups[i + 1] : 0;
            }

            // --- Project Type Data (New Dynamic Logic) ---
            var projectTypeGroups = await _context.Websites
                .GroupBy(w => w.Type)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToListAsync();

            var projectTypeLabels = projectTypeGroups.Select(g => g.Type.ToString()).ToList();
            var projectTypeData = projectTypeGroups.Select(g => g.Count).ToList();


            // --- ViewModel ---
            var viewModel = new DashboardViewModel
            {
                WebsitesCount = await _context.Websites.CountAsync(),
                UsersCount = await _userManager.Users.CountAsync(),
                BlogPostsCount = await _context.BlogPosts.CountAsync(),
                MonthlyWebsiteData = monthlyWebsiteData.ToList(),
                MonthlyBlogPostData = monthlyBlogPostData.ToList(),
                ProjectTypeLabels = projectTypeLabels, 
                ProjectTypeData = projectTypeData      
            };

            return View(viewModel);
        }
    }
}
