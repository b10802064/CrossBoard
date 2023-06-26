using AngleSharp;
using cross_border.ViewModels;
using CrossBorder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Net;

namespace CrossBorder.Controllers
{
    public class ProductsController : Controller
    {
        private static int totalRows = -1;
        private readonly Cross_BorderContext _context;
        public ProductsController(Cross_BorderContext cross_Border) 
        {
            _context = cross_Border;
            if (totalRows == -1)
            {
                totalRows = _context.Products.Count();   //計算總筆數
            }
        }
        public IActionResult Index()
        {
            return View();

        }
        public IActionResult ProductList(int id = 1)
        {


            int activePage = id; //目前所在頁
            int pageRows = 8;   //每頁幾筆資料
            //int totalRows = _ctx.Clothing.Count();   //計算總筆數

            //計算Page頁數
            int Pages = 0;
            if (totalRows % pageRows == 0)
            {
                Pages = totalRows / pageRows;
            }
            else
            {
                Pages = (totalRows / pageRows) + 1;
            }

            int startRow = (activePage - 1) * pageRows;  //起始記錄Index
            List<Product> products = _context.Products.Skip(startRow).Take(pageRows).ToList();


            ViewData["Active"] = 1;    //SidebarActive頁碼
            ViewData["ActivePage"] = id;    //Activec分頁碼
            ViewData["Pages"] = Pages;  //頁數
            return View(products);
        }
        public IActionResult ProductDetail(string? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Product product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }



        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(AddProductViewModel AddPVM)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36");
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            string url = "https://d.uniqlo.com/tw/p/product/i/product/spu/pc/query/"+AddPVM.ProductId+"/zh_TW";
            string url2 = "https://www.uniqlo.com/tw/data/products/spu/zh_TW/"+AddPVM.ProductId+".json";
            
            var responseMessage = httpClient.GetAsync(url).Result;
            var responseMessage2 = httpClient.GetAsync(url2).Result;
            var products = from num in _context.Products select num.ProductId;
            var chack1 = true;
            foreach  (var i in products)
            {
                if(AddPVM.ProductId.Trim() == i.Trim())
                {
                    chack1 = false;
                }
            }
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK && responseMessage2.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if(chack1 == true)
                {
                    //讀取Content內容
                    string responseResult = responseMessage.Content.ReadAsStringAsync().Result;
                    string responseResult2 = responseMessage2.Content.ReadAsStringAsync().Result;
                    //var document = context.OpenAsync(res => res.Content(responseResult)).Result;
                    //var head = document.QuerySelectorAll(".leading-0 .article-intro ul:nth-child(4) li");
                    //foreach (var c in head)
                    //{
                    //    //取得每個元素的TextContent

                    //    ViewData["head"] += c.TextContent;
                    //}
                    dynamic pricedata = JsonConvert.DeserializeObject(responseResult);
                    dynamic namedata = JsonConvert.DeserializeObject(responseResult2);
                    //dynamic namedata = JsonConvert.DeserializeObject(responseResult);

                    string price = pricedata.resp[0].summary.minPrice;
                    string productname = namedata.summary.name;
                    //
                    string productphoto = "https://www.uniqlo.com/tw/hmall/test/" + AddPVM.ProductId + "/main/first/1000/1.jpg";

                    string jpurlcode = namedata.summary.code;
                    string urljp = "https://www.uniqlo.com/jp/api/commerce/v5/ja/products/E" + jpurlcode + "-000/price-groups/00/l2s?withPrices=true&withStocks=true&httpFailure=true";
                    var responseMessagejp = httpClient.GetAsync(urljp).Result;
                    if (responseMessagejp.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string responseResultjp = responseMessagejp.Content.ReadAsStringAsync().Result;
                        dynamic jpdata = JsonConvert.DeserializeObject(responseResultjp);
                        string jpcoode = jpdata.result.l2s[0].l2Id;
                        string jpprice = jpdata.result.prices[jpcoode]["base"].value;
                    }

                    if (ModelState.IsValid)
                    {
                        Product product = new Product
                        {
                            ProductId = AddPVM.ProductId.Trim(),
                            ProductName = productname,
                            Description = price,
                            Photo = productphoto,
                        };


                        _context.Products.Add(product);
                        _context.SaveChanges();
                        ViewData["Title"] = "新增成功";
                        ViewData["Message"] = "新增產品成功!";

                        return View("~/Views/Shared/ResultMessage.cshtml");

                    }


                }
                ModelState.AddModelError(string.Empty, "產品id重複");
                return View(AddPVM);
            }
            ModelState.AddModelError(string.Empty, "產品id有錯");
            return View(AddPVM);
        }




    }
}
