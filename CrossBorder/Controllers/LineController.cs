using CrossBorder.Models;
using CrossBorder.MyClass;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CrossBorder.Controllers
{
    public class LineController : Controller
    {
        private readonly Cross_BorderContext _context;
        public LineController(Cross_BorderContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult LineMessage(string userId, string userMessage)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Json(new { success = false, message = "userId 參數為必填" });
            }
            if (string.IsNullOrWhiteSpace(userMessage))
            {
                return Json(new { success = false, message = "userMessage 參數為必填" });
            }
            var getMsg = "?userID="+ userId;
            var userlist= _context.Customers.Select(c=> c.lineid).ToList();
            if(userlist.Contains(userId)) 
            {
                List<string> stringList = userMessage.Select(c => c.ToString()).ToList();
                var productwomanc = _context.Classifieds.Where(data => data.TypeId.Trim() == "T001").Select(data => data.ProductId).ToList();
                var filteredListwoman = _context.Products.Where(item => productwomanc.Contains(item.ProductId)).ToList();
                var randomproductswoman = Net.RandomProduct(filteredListwoman, 2);
                var customer = _context.Customers.Where(item => item.lineid ==userId).FirstOrDefault();

                


                if (stringList.Contains("女"))
                {
                    getMsg = "?userID=" + userId;
                    getMsg += "&text=敬愛的" + customer.CusdtomerName.Trim() + "會員。\n目前推薦的女裝商品如下:\n";
                    Net.sendLineMessage(getMsg);

                    foreach(var item in randomproductswoman)
                    {
                        getMsg = "?userID=" + userId;
                        getMsg += "&text="+item.ProductName.Trim();
                        getMsg += "&imageUrl=" + item.Photo.Trim();
                        getMsg += "&previewImageUrl=" + item.Photo.Trim();
                        Net.sendLineMessage(getMsg);
                    }

                }
                else
                {
                    getMsg = "?userID=" + userId;
                    getMsg += "&text=敬愛的" + customer.CusdtomerName.Trim() + "會員。\n請以更精準的字元搜索\n";
                    Net.sendLineMessage(getMsg);
                }
            }
            else
            {
                getMsg = "?userID=" + userId;
                getMsg += "&text= 請到以下網址註冊 : https://crossborder20230717162124.azurewebsites.net/Account/Register?userid="+userId;
                Net.sendLineMessage(getMsg);
            }

            //
            
            // 在這裡執行你的邏輯處理
            //getMsg += "&text=第10組跨境電商";
            //// ...
            //Net.sendLineMessage(getMsg);



            return Json(new { success = true, message = "API 呼叫成功" });
        }
    }
}
