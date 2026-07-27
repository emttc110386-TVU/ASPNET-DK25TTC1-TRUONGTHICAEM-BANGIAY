using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TrasuaMON.Models;

namespace TrasuaMON.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        private List<CartItem> GetCart()
        {
            var session = HttpContext.Session.GetString("CART");
            if (!string.IsNullOrEmpty(session))
            {
                return JsonSerializer.Deserialize<List<CartItem>>(session) ?? new List<CartItem>();
            }
            return new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString("CART", JsonSerializer.Serialize(cart));
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        public IActionResult AddToCart(int productId)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                var cart = GetCart();
                var item = cart.FirstOrDefault(p => p.ProductId == productId);
                if (item != null)
                {
                    item.Quantity++;
                }
                else
                {
                    cart.Add(new CartItem
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Price = product.Price,
                        ImageUrl = product.ImageUrl,
                        Quantity = 1
                    });
                }
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int productId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(p => p.ProductId == productId);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Checkout(string customerName, string phone, string address)
        {
            var cart = GetCart();
            if (!cart.Any()) return RedirectToAction("Index");

            ViewBag.CustomerName = customerName;
            ViewBag.Phone = phone;
            ViewBag.Address = address;
            ViewBag.OrderDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            var orderCart = new List<CartItem>(cart);
            HttpContext.Session.Remove("CART");

            return View("Bill", orderCart);
        }
    }
}