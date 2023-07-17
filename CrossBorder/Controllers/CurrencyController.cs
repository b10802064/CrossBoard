using AngleSharp;
using cross_border.ViewModels;
using CrossBorder.Models;
using CrossBorder.MyClass;
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
            //var httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36");
            //var config = Configuration.Default.WithDefaultLoader();
            //var context = BrowsingContext.New(config);
            //string urlJT = "https://zt.coinmill.com/JPY_TWD.html";
            //string urlCN = "https://zt.coinmill.com/CNY_TWD.html";
            //var responseMessageJT = httpClient.GetAsync(urlJT).Result;
            //var responseMessageCN = httpClient.GetAsync(urlCN).Result;
            //if (responseMessageJT.StatusCode == System.Net.HttpStatusCode.OK && responseMessageCN.StatusCode == System.Net.HttpStatusCode.OK)
            //{
            //string responseResultJT = responseMessageJT.Content.ReadAsStringAsync().Result;
            //var document = context.OpenAsync(res => res.Content(responseResultJT)).Result;
            //var jps = document.QuerySelector("table.conversionchart tr:nth-child(16) td:first-child");
            //var tws = document.QuerySelector("table.conversionchart tr:nth-child(16) td:last-child");
            //decimal tw = Convert.ToDecimal(tws.TextContent);
            //decimal jp = Convert.ToDecimal(jps.TextContent);
            //var twtojp = jp / tw;

            //string responseResultCN = responseMessageCN.Content.ReadAsStringAsync().Result;
            //var documentC = context.OpenAsync(res => res.Content(responseResultCN)).Result;
            //var cns = documentC.QuerySelector("table.conversionchart tr:nth-child(16) td:first-child");
            //var twC = documentC.QuerySelector("table.conversionchart tr:nth-child(16) td:last-child");
            //decimal twcn = Convert.ToDecimal(twC.TextContent);
            //decimal cn = Convert.ToDecimal(cns.TextContent);
            //var twtocn = cn / twcn;
            decimal twtojp = Net.GetConversionRate("https://zt.coinmill.com/JPY_TWD.html", "table.conversionchart tr:nth-child(16) td:first-child", "table.conversionchart tr:nth-child(16) td:last-child");
            decimal twtocn = Net.GetConversionRate("https://zt.coinmill.com/CNY_TWD.html", "table.conversionchart tr:nth-child(16) td:first-child", "table.conversionchart tr:nth-child(16) td:last-child");

            if (ModelState.IsValid)
                    {
                    Country existingcountry = _context.Countries.FirstOrDefault(u => u.CountryId == "JP");
                    Country existingcountry2 = _context.Countries.FirstOrDefault(u => u.CountryId == "CN");
                    if (existingcountry != null)
                    {
                        existingcountry.Prefix = twtojp.ToString();
                        existingcountry2.Prefix = twtocn.ToString();
                        _context.SaveChanges();
                        ViewData["Title"] = "更新成功";
                        ViewData["Message"] = "更新匯率成功!";
                        return View("~/Views/Shared/ResultMessage_true.cshtml");
                    }

                //Country country = new Country
                //{
                //    CountryId = "CN",
                //    CountryName = "https://www.gov.cn/guoqing/site1/20100928/001aa04acfdf0e0bfb6401.gif",
                //    Prefix = twtojp.ToString()
                //};
                //_context.Countries.Add(country);
                //_context.SaveChanges();
                //ViewData["Title"] = "新增成功";
                //ViewData["Message"] = "新增國家成功!";
                //return View("~/Views/Shared/ResultMessage_true.cshtml");
            }
        //}
            ModelState.AddModelError(string.Empty, "產品id有錯");
            return View();
        }




    }
}
