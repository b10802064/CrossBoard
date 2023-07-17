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
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Classification;
using System.Collections;
using Microsoft.CodeAnalysis;
using CrossBorder.MyClass;

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

        public IActionResult ProductSearch(string keyword , int id = 1)
        {
            // 使用 Entity Framework Core 查询语法进行产品搜索
            var searchResults = _context.Products.Where(p => p.ProductName.Contains(keyword) && (keyword != "帽" || !p.ProductName.Contains("連帽"))).ToList();

            int pageRows = 8;   //每頁幾筆資料
            int totalRows = searchResults.Count();   //計算總筆數
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
            if (id < 1)
            {
                id = 1;
            }
            int maxPage = Pages;
            if (id > maxPage)
            {
                id = maxPage;
            }
            int activePage = id; //目前所在頁
            int startRow = (activePage - 1) * pageRows;  //起始記錄Index
            List<Product> productspage = searchResults.Skip(startRow).Take(pageRows).ToList();
            ViewData["Active"] = 1;    //SidebarActive頁碼
            ViewData["ActivePage"] = id;    //Activec分頁碼
            ViewData["Pages"] = Pages;  //頁數
            ViewData["Keyword"] = keyword;  
            // 将搜索结果传递给视图
            return View(productspage);
        }
        public IActionResult ProductList(int id = 1 , string category = null)
        {
            List<Product> products;

            if (!string.IsNullOrEmpty(category))
            {
                var type  =  _context.Types.FirstOrDefault(p => p.TypeName.Trim() == category.Trim());
                var clsssf  =  _context.Classifieds.Where(p => p.TypeId.Trim() == type.TypeId.Trim()).ToList();
                var productIds = clsssf.Select(c => c.ProductId).Distinct().ToList();

                products = _context.Products
                    .Where(p => productIds.Contains(p.ProductId))
                    .ToList();
            }
            else
            {
                products = _context.Products.ToList();
            }
            int pageRows = 8;   //每頁幾筆資料
            int totalRows = products.Count();   //計算總筆數
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
            if (id < 1)
            {
                id = 1;
            }
            int maxPage = Pages;
            if (id > maxPage)
            {
                id = maxPage;
            }
            int activePage = id; //目前所在頁
            int startRow = (activePage - 1) * pageRows;  //起始記錄Index
            List<Product> productspage = products.Skip(startRow).Take(pageRows).ToList();
            ViewData["Active"] = 1;    //SidebarActive頁碼
            ViewData["ActivePage"] = id;    //Activec分頁碼
            ViewData["Pages"] = Pages;  //頁數
            ViewData["Category"] = category;
            return View(productspage);
        }

        public IActionResult ProductDetail(string? id)
        {

            //Product existingcountry = _context.Products.FirstOrDefault(u => u.ProductId.Trim() == id.Trim());
            //if (existingcountry != null)
            //{
            //    existingcountry.Prefix = twtojp.ToString();
            //    existingcountry2.Prefix = twtocn.ToString();
            //    _context.SaveChanges();
            //}
            if (id == null)
            {
                return BadRequest();
            }
            Product product = _context.Products.Find(id);


            Sale existingcountry = _context.Sales.FirstOrDefault(u => u.ProductId.Trim() == id.Trim() && u.CountryId == "JP");
            List<string> resultList = Net.Getjpdata(product.ProductJP);
            string jpprice = resultList[1];
            string jpcode = resultList[0];

            if (existingcountry != null)
            {
                existingcountry.Price = Convert.ToDecimal(jpprice);
                existingcountry.ProductCountrycode = jpcode;
                _context.SaveChanges();
            }

            if (product == null)
            {
                return NotFound();
            }
            var sales = _context.Sales.Where(item => item.ProductId == product.ProductId).ToList();

            var pdVMs = sales.Select(group => new CurrencyViewModel
            {
                Price = group.Price/ Convert.ToDecimal((from data in _context.Countries
                                                        where data.CountryId == @group.CountryId
                                                        select data.Prefix).FirstOrDefault()),
                Photo = (from data in _context.Countries
                         where data.CountryId == @group.CountryId
                         select data.CountryName).FirstOrDefault(),
                code = group.ProductCountrycode

            }).OrderBy(vm => vm.Price).ToList();

            var mixViewModel = new mixProductCountriesViewModel
            {
                Products = product,
                currencyVMs = pdVMs,
            };
            return View(mixViewModel);
        }


        [HttpGet]
        public IActionResult AddProductlist(string? id)
        {
            bool chack1 = true;
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    string username = User.Identity.Name;
                    var customer = (from data in _context.Customers
                             where data.CusdtomerName.Trim() == username.Trim()
                             select data).FirstOrDefault();
                    var product = (from data in _context.Products
                                  where data.ProductId.Trim() == id.Trim()
                                  select data).FirstOrDefault();

                    foreach (var i in _context.Shoppinglists)
                    {
                        if (customer.CustomerId == i.CustomerId && product.ProductId == i.ProductId)
                        {
                            chack1 = false;
                        }
                    }

                    if (chack1)
                    {
                        Shoppinglist shoppinglist = new Shoppinglist
                        {
                            ProductId = product.ProductId,
                            CustomerId = customer.CustomerId,
                            Amount = 1
                        };
                        _context.Shoppinglists.Add(shoppinglist);
                        _context.SaveChanges();
                        ViewData["Title"] = "新增成功";
                        ViewData["Message"] = "新增產品成功!";
                        return View("~/Views/Shared/ResultMessage_true.cshtml");
                    }
                    else
                    {
                        ViewData["Title"] = "新增失敗";
                        ViewData["Message"] = "已有此項目";
                        return View("~/Views/Shared/ResultMessage_true.cshtml");
                    }
                }
            }
            else
            {
                return LocalRedirect("~/Account/Login");
            }
            return View();
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
                    dynamic pricedata = JsonConvert.DeserializeObject(responseResult);
                    dynamic namedata = JsonConvert.DeserializeObject(responseResult2);
                    //dynamic namedata = JsonConvert.DeserializeObject(responseResult);
                    string price = pricedata.resp[0].summary.minPrice;
                    string productname = namedata.summary.name;
                    string type = namedata.summary.gDeptValue;
                    //日本區處理
                    string jpprice="0";
                    string jptruecoode = "";
                    string productphoto = "https://www.uniqlo.com/tw/hmall/test/" + AddPVM.ProductId + "/main/first/1000/1.jpg";
                    string productphoto2 = "https://www.uniqlo.com/tw/hmall/test/" + AddPVM.ProductId + "/main/other2/1000/3.jpg";
                    string jpurlcode = namedata.summary.code;


                    List<string> resultList = Net.Getjpdata(jpurlcode);
                    jptruecoode = resultList[0];
                    jpprice = resultList[1];

                    if (jptruecoode == "")
                    {
                        ViewData["Title"] = "新增失敗";
                        ViewData["Message"] = "日本無此商品!";
                        return View("~/Views/Shared/ResultMessage_true.cshtml");
                    }


                    //string searchjp = "https://www.uniqlo.com/jp/api/commerce/v5/ja/products?q=" + jpurlcode + "&queryRelaxationFlag=true&offset=0&limit=36&httpFailure=true";
                    //var responseMessagejps = httpClient.GetAsync(searchjp).Result;
                    //if (responseMessagejps.StatusCode == System.Net.HttpStatusCode.OK)
                    //{
                    //    string responseResultjps = responseMessagejps.Content.ReadAsStringAsync().Result;
                    //    dynamic jpdatas = JsonConvert.DeserializeObject(responseResultjps);
                    //    if(jpdatas.result.items.Count <= 0)
                    //    {
                    //        ViewData["Title"] = "新增失敗";
                    //        ViewData["Message"] = "日本無此商品!";
                    //        return View("~/Views/Shared/ResultMessage_true.cshtml");
                    //    }
                    //    else
                    //    {
                    //        jptruecoode = jpdatas.result.items[0].productId;
                    //    }  
                    //}

                    //string urljp = "https://www.uniqlo.com/jp/api/commerce/v5/ja/products/" + jptruecoode + "/price-groups/00/l2s?withPrices=true&withStocks=true&httpFailure=true";
                    //var responseMessagejp = httpClient.GetAsync(urljp).Result;
                    //if (responseMessagejp.StatusCode == System.Net.HttpStatusCode.OK)
                    //{
                    //    string responseResultjp = responseMessagejp.Content.ReadAsStringAsync().Result;
                    //    dynamic jpdata = JsonConvert.DeserializeObject(responseResultjp);
                    //    string jpcoode = jpdata.result.l2s[0].l2Id;
                    //    jpprice = jpdata.result.prices[jpcoode]["base"].value;

                    //}


                    //中國區處理
                    string cnprice = "0";
                    var chinesers = Net.SendRequest(jpurlcode);
                    dynamic chinesedata = JsonConvert.DeserializeObject(chinesers);

                    if(chinesedata.resp[1].Count == 0)
                    {
                        ViewData["Title"] = "新增失敗";
                        ViewData["Message"] = "中國無此商品!";
                        return View("~/Views/Shared/ResultMessage_true.cshtml");
                    }
                    string cncoode = chinesedata.resp[1][0].productCode;
                    cnprice = chinesedata.resp[1][0].prices[0];
                    if (type == "女童")
                    {
                        type = "童裝";
                    }
                    var typeid = _context.Types.FirstOrDefault(m => m.TypeName == type).TypeId;
                    if (ModelState.IsValid)
                    {
                        Product product = new Product
                        {
                            ProductId = AddPVM.ProductId.Trim(),
                            ProductJP = jptruecoode,
                            ProductCN = cncoode,
                            ProductName = productname,
                            Description = price,
                            Photo = productphoto,
                            Photo2 = productphoto2
                        };

                        Classified classified = new Classified
                        {
                            ProductId = AddPVM.ProductId.Trim(),
                            TypeId = typeid
                        };

                        Sale salejp = new Sale
                        {
                            ProductId = AddPVM.ProductId.Trim(),
                            CountryId = "JP",
                            Price = Convert.ToDecimal(jpprice),
                            ProductCountrycode = jptruecoode.Trim()
                        };
                        Sale saletw = new Sale
                        {
                            ProductId = AddPVM.ProductId.Trim(),
                            CountryId = "TW",
                            Price = Convert.ToDecimal(price),
                            ProductCountrycode = AddPVM.ProductId.Trim()
                        };
                        Sale salecn = new Sale
                        {
                            ProductId = AddPVM.ProductId.Trim(),
                            CountryId = "CN",
                            Price = Convert.ToDecimal(cnprice),
                            ProductCountrycode = cncoode.Trim()
                        };

                        _context.Products.Add(product);
                        _context.Classifieds.Add(classified);
                        _context.Sales.Add(salejp);
                        _context.Sales.Add(saletw);
                        _context.Sales.Add(salecn);
                        _context.SaveChanges();
                        ViewData["Title"] = "新增成功";
                        ViewData["Message"] = "新增產品成功!";

                        return View("~/Views/Shared/ResultMessage_true.cshtml");

                    }
                }
                ModelState.AddModelError(string.Empty, "產品id重複");
                return View(AddPVM);
            }
            ModelState.AddModelError(string.Empty, "產品id有錯");
            return View(AddPVM);
        }





        public IActionResult CustomerProductList()
        {
            //string username = User.Identity.Name;
            //var customer = (from data in _context.Customers
            //                where data.CusdtomerName.Trim() == username.Trim()
            //                select data).FirstOrDefault();
            //var cplist = (from data in _context.Shoppinglists
            //              where data.CustomerId.Trim() == customer.CustomerId.Trim()
            //              select data.ProductId).ToList(); 舊程式碼

            string username = User.Identity.Name;
            var customer = _context.Customers.FirstOrDefault(data => data.CusdtomerName.Trim() == username.Trim());
            var cplist = _context.Shoppinglists
                .Where(data => data.CustomerId.Trim() == customer.CustomerId.Trim())
                .Select(data => data.ProductId)
                .ToList();

            var filteredList = _context.Products.Where(item => cplist.Contains(item.ProductId)).ToList();
            var filteredList2 = _context.Sales.Where(item => cplist.Contains(item.ProductId)).ToList();
            var result = filteredList2.GroupBy((x) => x.CountryId);


            // 優化 currencyVMs 查詢
            var countriesPrefix = _context.Countries.ToDictionary(data => data.CountryId, data => data.Prefix);
            var countriesName = _context.Countries.ToDictionary(data => data.CountryId, data => data.CountryName);
            var currencyVMs = result.Select(group =>
            {
                decimal prefix = countriesPrefix.TryGetValue(group.Key, out var p) ? decimal.Parse(p) : 1m;

                decimal totalPrice = group.Sum(item =>
                {
                    var shoppinglist = _context.Shoppinglists.FirstOrDefault(sl => sl.ProductId == item.ProductId);
                    if (shoppinglist != null)
                    {
                        return item.Price * shoppinglist.Amount;
                    }
                    return 0;
                });
                return new CurrencyViewModel
                {
                    Price = totalPrice / Convert.ToDecimal(prefix),
                    Photo = countriesName.TryGetValue(group.Key, out var name) ? name : null
                };
            }).OrderBy(vm => vm.Price).ToList();


            //var currencyVMs = result.Select(group => new CurrencyViewModel 舊程式
            //{
            //    Price = group.Sum(item =>
            //    {
            //        var shoppinglist = _context.Shoppinglists
            //            .FirstOrDefault(sl => sl.ProductId == item.ProductId);
            //        if (shoppinglist != null)
            //        {
            //            return item.Price * shoppinglist.Amount;
            //        }
            //        return 0;
            //    }) / Convert.ToDecimal((from data in _context.Countries
            //                            where data.CountryId == @group.Key
            //                            select data.Prefix).FirstOrDefault()),

            //    Photo = (from data in _context.Countries
            //             where data.CountryId == @group.Key
            //             select data.CountryName).FirstOrDefault()
            //}).ToList();



            var productinfoVMs = filteredList.Select(group => new ProductinfoViewModel
            {
                Products = group,
                Amount = (from data in _context.Shoppinglists
                          where data.ProductId == @group.ProductId
                          select data.Amount).FirstOrDefault(),

                Price = (from data in _context.Sales
                         where data.ProductId == @group.ProductId && data.CountryId == "TW"
                         select data.Price * (
                         from amount in _context.Shoppinglists
                         where amount.ProductId == @group.ProductId
                         select amount.Amount
                         ).FirstOrDefault()).FirstOrDefault()
            }).ToList();




            var mixViewModel = new mixCurrencyViewModel
            {
                Products = productinfoVMs,
                currencyVMs = currencyVMs
            };
            return View(mixViewModel);
        }

        public IActionResult DeleteCProduct(string? id)
        {
            var productToDelete = (from data in _context.Shoppinglists
                                   where data.ProductId.Trim() == id.Trim()
                                   select data).FirstOrDefault();

            if (productToDelete != null)
            {
                _context.Shoppinglists.Remove(productToDelete);
                _context.SaveChanges();
                ViewData["Title"] = "刪除成功";
                ViewData["Message"] = "刪除產品成功!";
                return View("~/Views/Shared/ResultMessage_true.cshtml");
            }
            return View();
        }




        [HttpGet]
        public IActionResult ProductAmount()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult ProductAmount(string? id, int amount)
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
            string username = User.Identity.Name;
            var customer = (from data in _context.Customers
                            where data.CusdtomerName.Trim() == username.Trim()
                            select data).FirstOrDefault();
            Shoppinglist shoppingList = _context.Shoppinglists.FirstOrDefault(sl => sl.CustomerId == customer.CustomerId && sl.ProductId == id);

            if (shoppingList != null)
            {
                // 更新购物清单中的Amount值
                shoppingList.Amount = amount;
                _context.SaveChanges();

                ViewData["Title"] = "更新數量";
                ViewData["Message"] = "更新數量成功!";
                return View("~/Views/Shared/ResultMessage_true.cshtml");
            }

            return View();

        }








    }
}
