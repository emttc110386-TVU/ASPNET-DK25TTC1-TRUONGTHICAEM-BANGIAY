using Microsoft.AspNetCore.Mvc;
using BangiayCAEM.Models;
using Microsoft.EntityFrameworkCore;

namespace BangiayCAEM.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. THÊM SẢN PHẨM
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            var category = await _context.Categories.FirstOrDefaultAsync();
            if (category == null)
            {
                category = new Category { Name = "Giày Sneaker" };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }

            product.CategoryId = category.Id;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        // 2. SỬA SẢN PHẨM
        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            var existingProduct = await _context.Products.FindAsync(product.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.ImageUrl = product.ImageUrl;

                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }

        // 3. XÓA SẢN PHẨM
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
