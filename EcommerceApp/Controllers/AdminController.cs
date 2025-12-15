using EcommerceApp.Data;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace EcommerceApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AdminController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        private bool IsAdmin() => HttpContext.Session.GetString("UserRole") == "Admin";

        public IActionResult Dashboard()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            return View();
        }

        public IActionResult Products()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            var products = _context.Products.Where(p => p.OwnerId == Guid.Parse(HttpContext.Session.GetString("UserId") ?? string.Empty)).ToList();
            return View(products);
        }

        public IActionResult CreateProduct()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product model, IFormFile? imageFile)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized." });
            //if (!ModelState.IsValid) return Json(new { success = false, message = "Invalid product details." });

            var ownerId = HttpContext.Session.GetString("UserId");
            if (ownerId == null) return Json(new { success = false, message = "Admin not logged in." });
            model.OwnerId = Guid.Parse(ownerId);

            if (imageFile != null && imageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var ext = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(ext))
                    return Json(new { success = false, message = "Invalid image type. Use JPG, PNG, or GIF." });

                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "products");
                Directory.CreateDirectory(uploadsFolder); // Create if not exists

                var uniqueFileName = Guid.NewGuid().ToString() + ext;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                model.ImageUrl = "/images/products/" + uniqueFileName;
            }
            else
            {
                return Json(new { success = false, message = "Image is required." });
            }

            _context.Products.Add(model);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Product added successfully!" });
        }

        public IActionResult EditProduct(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            var product = _context.Products.Find(id);
            if (product == null || product.OwnerId != Guid.Parse(HttpContext.Session.GetString("UserId") ?? string.Empty)) return NotFound();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(Product model, IFormFile? imageFile, string? existingImageUrl)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized." });
            var product = await _context.Products.FindAsync(model.Id);
            if (product == null || product.OwnerId != Guid.Parse(HttpContext.Session.GetString("UserId") ?? string.Empty)) return Json(new { success = false, message = "Unauthorized." });

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.Stock = model.Stock;
            product.Category = model.Category;

            if (imageFile != null && imageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var ext = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(ext))
                    return Json(new { success = false, message = "Invalid image type. Use JPG, PNG, or GIF." });

                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "products");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + ext;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Delete old image
                if (!string.IsNullOrEmpty(existingImageUrl))
                {
                    var oldPath = Path.Combine(_environment.WebRootPath, existingImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }

                product.ImageUrl = "/images/products/" + uniqueFileName;
            }
            else if (!string.IsNullOrEmpty(existingImageUrl))
            {
                product.ImageUrl = existingImageUrl;
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Product updated successfully!" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized." });
            var prod = await _context.Products.FindAsync(id);
            if (prod == null || prod.OwnerId != Guid.Parse(HttpContext.Session.GetString("UserId") ?? string.Empty)) return Json(new { success = false, message = "Product not found." });

            // Delete image
            if (!string.IsNullOrEmpty(prod.ImageUrl))
            {
                var filePath = Path.Combine(_environment.WebRootPath, prod.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
            }

            _context.Products.Remove(prod);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Product deleted." });
        }
    }
}