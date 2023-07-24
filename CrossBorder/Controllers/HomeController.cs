using cross_border.ViewModels;
using CrossBorder.Models;
using CrossBorder.MyClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
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
            var randomProducts = Net.RandomProduct(products,6);


            var productwomanc = _context.Classifieds.Where(data => data.TypeId.Trim() == "T001").Select(data => data.ProductId).ToList();
            var filteredListwoman = _context.Products.Where(item => productwomanc.Contains(item.ProductId)).ToList();
            var randomproductswoman = Net.RandomProduct(filteredListwoman, 6);

            var productmanc = _context.Classifieds.Where(data => data.TypeId.Trim() == "T002").Select(data => data.ProductId).ToList();
            var filteredListman = _context.Products.Where(item => productmanc.Contains(item.ProductId)).ToList();
            var randomproductsman = Net.RandomProduct(filteredListman, 6);

            var productkidc = _context.Classifieds.Where(data => data.TypeId.Trim() == "T003").Select(data => data.ProductId).ToList();
            var filteredListkid = _context.Products.Where(item => productkidc.Contains(item.ProductId)).ToList();
            var randomproductskid = Net.RandomProduct(filteredListkid   , 6);

            var productbabyc = _context.Classifieds.Where(data => data.TypeId.Trim() == "T004").Select(data => data.ProductId).ToList();
            var filteredListbaby = _context.Products.Where(item => productbabyc.Contains(item.ProductId)).ToList();
            var randomproductsbaby = Net.RandomProduct(filteredListbaby, 6);





            var indexViewModel = new IndexViewModel
            {
                Products = randomProducts,
                Productswoman = randomproductswoman,
                Productsman = randomproductsman,
                Productskid = randomproductskid,
                Productsbaby = randomproductsbaby,
            };


            return View(indexViewModel);
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


    }
}
