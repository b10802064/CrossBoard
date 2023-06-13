using CrossBorder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CrossBorder.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Productmain()
        {
            return View(cards);
        }





        public List<Product> cards { get; } = new List<Product>
        {
            new Product {ProductId = "001", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "002", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "003", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "004", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "005", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "006", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "011", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "012", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "013", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "014", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "015", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "016", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "021", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "022", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "023", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "024", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "025", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "026", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "031", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "032", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "033", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "034", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "035", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "036", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" }
        };
    }
}
