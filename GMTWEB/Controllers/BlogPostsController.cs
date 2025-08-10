using GMTWEB.Data;
using GMTWEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GMTWEB.Controllers
{
    [Authorize]
    public class BlogPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment; // <-- للتعامل مع الملفات

        public BlogPostsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment; // <-- حقن الخدمة
        }

        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var posts = from p in _context.BlogPosts select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                posts = posts.Where(s => s.Title.Contains(searchString) || s.TitleAr.Contains(searchString));
            }
            return View(await posts.OrderByDescending(b => b.DatePosted).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogPostViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.ImageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string uploadPath = Path.Combine(wwwRootPath, "uploads/blogs");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                    string extension = Path.GetExtension(model.ImageFile.FileName);
                    uniqueFileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(uploadPath, uniqueFileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }
                    uniqueFileName = "/uploads/blogs/" + uniqueFileName;
                }

                var user = await _userManager.GetUserAsync(User);
                var blogPost = new BlogPost
                {
                    Title = model.Title,
                    Content = model.Content,
                    TitleAr = model.TitleAr,
                    ContentAr = model.ContentAr,
                    ImageUrl = uniqueFileName, // <-- حفظ مسار الصورة
                    Author = user.UserName,
                    DatePosted = DateTime.UtcNow
                };
                _context.Add(blogPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null) return NotFound();

            var viewModel = new BlogPostViewModel
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Content = blogPost.Content,
                TitleAr = blogPost.TitleAr,
                ContentAr = blogPost.ContentAr,
                ExistingImageUrl = blogPost.ImageUrl // <-- إرسال مسار الصورة الحالية
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogPostViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var blogPost = await _context.BlogPosts.FindAsync(id);
                    if (blogPost == null) return NotFound();

                    if (model.ImageFile != null)
                    {
                        if (!string.IsNullOrEmpty(blogPost.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, blogPost.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string uploadPath = Path.Combine(wwwRootPath, "uploads/blogs");
                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }
                        string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                        string extension = Path.GetExtension(model.ImageFile.FileName);
                        string uniqueFileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(uploadPath, uniqueFileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(fileStream);
                        }
                        blogPost.ImageUrl = "/uploads/blogs/" + uniqueFileName;
                    }

                    blogPost.Title = model.Title;
                    blogPost.Content = model.Content;
                    blogPost.TitleAr = model.TitleAr;
                    blogPost.ContentAr = model.ContentAr;

                    _context.Update(blogPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.BlogPosts.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var blogPost = await _context.BlogPosts.FirstOrDefaultAsync(m => m.Id == id);
            if (blogPost == null) return NotFound();
            return View(blogPost);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost != null)
            {
                if (!string.IsNullOrEmpty(blogPost.ImageUrl))
                {
                    var imagePath = Path.Combine(_hostEnvironment.WebRootPath, blogPost.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                _context.BlogPosts.Remove(blogPost);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
