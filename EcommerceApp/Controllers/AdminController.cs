using EcommerceApp.Data;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin() => HttpContext.Session.GetString("UserRole") == "Admin";

        // Dashboard
        public IActionResult Dashboard()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            return View();
        }

        // Product List
        public IActionResult Products()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            var products = _context.Products.Where(p => p.OwnerId == Guid.Parse(HttpContext.Session.GetString("UserId"))).ToList();
            return View(products);
        }

        // Create Product (GET)
        public IActionResult CreateProduct()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            return View();
        }

        // Create Product (POST - AJAX)
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product model)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized." });
            if (!ModelState.IsValid) return Json(new { success = false, message = "Invalid product details." });

            var ownerId = HttpContext.Session.GetString("UserId");
            model.OwnerId = Guid.Parse(ownerId);

            _context.Products.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Product added successfully!" });
        }

        // Edit Product (GET)
        public IActionResult EditProduct(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            var product = _context.Products.Find(id);
            if (product == null || product.OwnerId != Guid.Parse(HttpContext.Session.GetString("UserId"))) return NotFound();
            return View(product);
        }

        // Edit Product (POST - AJAX)
        [HttpPost]
        public async Task<IActionResult> UpdateProduct([FromBody] Product model)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized." });
            var product = _context.Products.Find(model.Id);
            if (product == null || product.OwnerId != Guid.Parse(HttpContext.Session.GetString("UserId"))) return Json(new { success = false, message = "Unauthorized." });

            _context.Products.Update(model);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Product updated successfully!" });
        }

        // Delete Product
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized." });
            var prod = _context.Products.Find(id);
            if (prod == null || prod.OwnerId != Guid.Parse(HttpContext.Session.GetString("UserId"))) return Json(new { success = false, message = "Product not found." });

            _context.Products.Remove(prod);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Product deleted." });
        }
    }
}