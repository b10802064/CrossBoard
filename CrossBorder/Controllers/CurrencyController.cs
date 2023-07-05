using AngleSharp;
using cross_border.ViewModels;
using CrossBorder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;

namespace CrossBorder.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly Cross_BorderContext _context;
        public CurrencyController(Cross_BorderContext cross_Border)
        {
            _context = cross_Border;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Addtype()
        {

            if (ModelState.IsValid)
            {

                Models.Type type = new Models.Type
                {
                    TypeId = "T001",
                    TypeName = "女裝"
                };
                Models.Type type2 = new Models.Type
                {
                    TypeId = "T002",
                    TypeName = "男裝"
                };
                Models.Type type3 = new Models.Type
                {
                    TypeId = "T003",
                    TypeName = "童裝"
                };
                Models.Type type4 = new Models.Type
                {
                    TypeId = "T004",
                    TypeName = "新生兒/嬰幼兒"
                };


                _context.Types.Add(type);
                _context.Types.Add(type2);
                _context.Types.Add(type3);
                _context.Types.Add(type4);
                _context.SaveChanges();
                ViewData["Title"] = "新增成功";
                ViewData["Message"] = "新增類別成功!";
                return View("~/Views/Shared/ResultMessage_true.cshtml");
            }

            ModelState.AddModelError(string.Empty, "產品id有錯");
            return View();
        }
        public IActionResult AddcCurrency()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36");
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            string urlJT = "https://zt.coinmill.com/JPY_TWD.html";
            var responseMessageJT = httpClient.GetAsync(urlJT).Result;
            if (responseMessageJT.StatusCode == System.Net.HttpStatusCode.OK )
            {
                string responseResultJT = responseMessageJT.Content.ReadAsStringAsync().Result;
                var document = context.OpenAsync(res => res.Content(responseResultJT)).Result;
                var jps = document.QuerySelector("table.conversionchart tr:nth-child(19) td:first-child");
                var tws = document.QuerySelector("table.conversionchart tr:nth-child(19) td:last-child");
                decimal tw = Convert.ToDecimal(tws.TextContent);
                decimal jp = Convert.ToDecimal(jps.TextContent);
                var twtojp = jp / tw;

                if (ModelState.IsValid)
                    {
                    Country existingcountry = _context.Countries.FirstOrDefault(u => u.CountryId == "JP");
                    if (existingcountry != null)
                    {
                        existingcountry.Prefix = twtojp.ToString();
                        _context.SaveChanges();
                        ViewData["Title"] = "更新成功";
                        ViewData["Message"] = "更新匯率成功!";
                        return View("~/Views/Shared/ResultMessage_true.cshtml");
                    }

                    //Country country = new Country
                    //    {
                    //        CountryId = "JP",
                    //        CountryName = "https://i03piccdn.sogoucdn.com/fcb1054db90918ef",
                    //        Prefix = twtojp.ToString() 
                    //    };
                    //    _context.Countries.Add(country);
                    //    _context.SaveChanges();
                    //    ViewData["Title"] = "新增成功";
                    //    ViewData["Message"] = "新增國家成功!";
                    //    return View("~/Views/Shared/ResultMessage_true.cshtml");
                }
            }
            ModelState.AddModelError(string.Empty, "產品id有錯");
            return View();
        }




    }
}
