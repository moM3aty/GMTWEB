using GMTWEB.Data;
using GMTWEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GMTWEB.Controllers
{
    [Authorize]
    public class WebsitesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public WebsitesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var websites = from w in _context.Websites select w;
            if (!String.IsNullOrEmpty(searchString))
            {
                websites = websites.Where(s => s.Name.Contains(searchString) || s.NameAr.Contains(searchString));
            }
            return View(await websites.OrderByDescending(w => w.DateAdded).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WebsiteViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageFile == null)
                {
                    ModelState.AddModelError("ImageFile", "Please upload a website image.");
                    return View(model);
                }

                string wwwRootPath = _hostEnvironment.WebRootPath;
                string uploadPath = Path.Combine(wwwRootPath, "uploads/websites");
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

                var website = new Website
                {
                    Name = model.Name,
                    NameAr = model.NameAr,
                    Link = model.Link,
                    Type = model.Type,
                    ImageUrl = "/uploads/websites/" + uniqueFileName
                };

                _context.Add(website);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var website = await _context.Websites.FindAsync(id);
            if (website == null) return NotFound();

            var viewModel = new WebsiteViewModel
            {
                Id = website.Id,
                Name = website.Name,
                NameAr = website.NameAr,
                Link = website.Link,
                Type = website.Type,
                ExistingImageUrl = website.ImageUrl
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WebsiteViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var website = await _context.Websites.FindAsync(id);
                if (website == null) return NotFound();

                if (model.ImageFile != null)
                {
                    if (!string.IsNullOrEmpty(website.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, website.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string uploadPath = Path.Combine(wwwRootPath, "uploads/websites");
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
                    website.ImageUrl = "/uploads/websites/" + uniqueFileName;
                }

                website.Name = model.Name;
                website.NameAr = model.NameAr;
                website.Link = model.Link;
                website.Type = model.Type;

                try
                {
                    _context.Update(website);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Websites.Any(e => e.Id == website.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var website = await _context.Websites.FirstOrDefaultAsync(m => m.Id == id);
            if (website == null) return NotFound();
            return View(website);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var website = await _context.Websites.FindAsync(id);
            if (website != null)
            {
                if (!string.IsNullOrEmpty(website.ImageUrl))
                {
                    var imagePath = Path.Combine(_hostEnvironment.WebRootPath, website.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                _context.Websites.Remove(website);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
