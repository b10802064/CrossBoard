using CrossBorder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CrossBorder.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Cross_BorderContext _context;

        public HomeController(ILogger<HomeController> logger, Cross_BorderContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            var random = new Random();
            var randomIndexes = Enumerable.Range(0, products.Count()).OrderBy(x => random.Next()).Take(6);
            var randomProducts = randomIndexes.Select(index => products[index]).ToList();
            return View(randomProducts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public List<Product> cards { get; } = new List<Product>
        {
            new Product {ProductId = "001", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "002", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "003", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "004", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "005", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" },
            new Product {ProductId = "006", ProductName = "衣服名稱", Description = "2000", Photo="0.jpg" }
        };

    }
}
