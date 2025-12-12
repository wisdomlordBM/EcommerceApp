using EcommerceApp.Data;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        private bool IsLoggedIn() => HttpContext.Session.GetString("UserId") != null;

        // User Home (Product Index with search by category)
        public async Task<IActionResult> Home(string category = null)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            var productsQuery = _context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(category))
                productsQuery = productsQuery.Where(p => p.Category == category);

            var products = await productsQuery.ToListAsync();
            ViewBag.Categories = await _context.Products.Select(p => p.Category).Distinct().ToListAsync();
            return View(products);
        }
    }
}