using System.Net.Http;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using AngleSharp;
using System.Collections.Generic;
using CrossBorder.Models;
using System.Linq;

namespace CrossBorder.MyClass
{
    public class Net
    {
        public static async Task LineNotify(string _massage, string _token = "5yWBGD90N8XKig2FVHakgg1UGHiWkl18zwceUMLsR1s")
        {
            string url = "http://125.229.21.105/Line/LineAPI.php?Token=" + _token + "&Message=%F0%9F%94%94" + _massage;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    await client.GetAsync(url);  // 发送 HTTP 请求并等待完成
                }
            }
            catch (Exception ex)
            {
            }
        }
        
        //public static async Task sendLineMessage(string _id = "Uc120036fb20e6a930c198cb843aa75a6", string _message = "第10組跨境電商")
        //{
        //    string url = "https://script.google.com/macros/s/AKfycbzAyp4HE-Ki_PCGfzbcXrS6PSTSl7qarrEbPN_qhh-5O5qTus0Qi73ATCn6QQzj30-C/exec?userID=" + _id + "&text=" + _message + "&imageUrl=https://images.chinatimes.com/newsphoto/2021-11-17/1024/20211117004544.jpg&previewImageUrl=https://images.chinatimes.com/newsphoto/2021-11-17/1024/20211117004544.jpg";
        //    try
        //    {
        //        using (HttpClient client = new HttpClient())
        //        {
        //            await client.GetAsync(url);  // 发送 HTTP 请求并等待完成
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        public static async Task sendLineMessage(string GettMsg )
        {
            string url = "https://script.google.com/macros/s/AKfycbxu6AA55bCDsBqTmBsXyUnZcrte5-pb_MPWwpsaV134U24XPOGR6wVMslNXoNi5yfPY/exec" +  GettMsg;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    await client.GetAsync(url);  // 发送 HTTP 请求并等待完成
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static string SendRequest(string _productid)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36");
                var requestData = new
                {
                    url = "/search.html?description=" + _productid + "&searchType=1",
                    pageInfo = new { page = 1, pageSize = 24, withSideBar = "Y" },
                    belongTo = "pc",
                    rank = "overall",
                    priceRange = new { low = 0, high = 0 },
                    color = new string[] { },
                    size = new string[] { },
                    season = new string[] { },
                    material = new string[] { },
                    sex = new string[] { },
                    identity = new string[] { },
                    insiteDescription = "",
                    exist = new string[] { },
                    searchFlag = true,
                    description = _productid
                };
                var jsonRequest = JsonConvert.SerializeObject(requestData);
                var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var response = client.PostAsync("https://d.uniqlo.cn/p/hmall-sc-service/search/searchWithDescriptionAndConditions/zh_CN", httpContent).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    return jsonResponse;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static decimal GetConversionRate(string url, string selector ,string selector2)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36");

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            var responseMessage = httpClient.GetAsync(url).Result;
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string responseResult = responseMessage.Content.ReadAsStringAsync().Result;
                var document = context.OpenAsync(res => res.Content(responseResult)).Result;
                var element = document.QuerySelector(selector);
                var element2 = document.QuerySelector(selector2);

                decimal value = Convert.ToDecimal(element.TextContent)/Convert.ToDecimal(element2.TextContent);
                return value;
            }

            return 0;
        }
        //public static (string, string) Getjpdata(string _productid)
        //{

        //    string jptruecoode = "";
        //    string jpprice = "0";

        //    var httpClient = new HttpClient();
        //    httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36");
        //    var config = Configuration.Default.WithDefaultLoader();
        //    var context = BrowsingContext.New(config);

        //    string searchjp = "https://www.uniqlo.com/jp/api/commerce/v5/ja/products?q=" + _productid + "&queryRelaxationFlag=true&offset=0&limit=36&httpFailure=true";
        //    var responseMessagejps = httpClient.GetAsync(searchjp).Result;
        //    if (responseMessagejps.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        string responseResultjps = responseMessagejps.Content.ReadAsStringAsync().Result;
        //        dynamic jpdatas = JsonConvert.DeserializeObject(responseResultjps);
        //        if (jpdatas.result.items.Count <= 0)
        //        {
        //            return ("0","");
        //        }
        //        else
        //        {
        //            jptruecoode = jpdatas.result.items[0].productId;
        //        }
        //    }
        //    string urljp = "https://www.uniqlo.com/jp/api/commerce/v5/ja/products/" + jptruecoode + "/price-groups/00/l2s?withPrices=true&withStocks=true&httpFailure=true";
        //    var responseMessagejp = httpClient.GetAsync(urljp).Result;
        //    if (responseMessagejp.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        string responseResultjp = responseMessagejp.Content.ReadAsStringAsync().Result;
        //        dynamic jpdata = JsonConvert.DeserializeObject(responseResultjp);
        //        string jpcoode = jpdata.result.l2s[0].l2Id;
        //        jpprice = jpdata.result.prices[jpcoode]["base"].value;
        //    }
        //    return (jptruecoode,jpprice);
        //}
        public static List<string> Getjpdata(string _productid)
        {
            List<string> resultList = new List<string>();

            string jptruecoode = "";
            string jpprice = "0";

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36");
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            string searchjp = "https://www.uniqlo.com/jp/api/commerce/v5/ja/products?q=" + _productid + "&queryRelaxationFlag=true&offset=0&limit=36&httpFailure=true";
            var responseMessagejps = httpClient.GetAsync(searchjp).Result;
            if (responseMessagejps.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string responseResultjps = responseMessagejps.Content.ReadAsStringAsync().Result;
                dynamic jpdatas = JsonConvert.DeserializeObject(responseResultjps);
                if (jpdatas.result.items.Count <= 0)
                {
                    resultList.Add("");
                    resultList.Add("0");
                    return resultList;
                }
                else
                {
                    jptruecoode = jpdatas.result.items[0].productId;
                }
            }

            string urljp = "https://www.uniqlo.com/jp/api/commerce/v5/ja/products/" + jptruecoode + "/price-groups/00/l2s?withPrices=true&withStocks=true&httpFailure=true";
            var responseMessagejp = httpClient.GetAsync(urljp).Result;
            if (responseMessagejp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string responseResultjp = responseMessagejp.Content.ReadAsStringAsync().Result;
                dynamic jpdata = JsonConvert.DeserializeObject(responseResultjp);
                string jpcoode = jpdata.result.l2s[0].l2Id;
                jpprice = jpdata.result.prices[jpcoode]["base"].value;
            }

            resultList.Add(jptruecoode);
            resultList.Add(jpprice);
            return resultList;
        }



        public static List<Product> RandomProduct(List<Product> products,int amount)
        {
            var random = new Random();
            var randomIndexes = Enumerable.Range(0, products.Count()).OrderBy(x => random.Next()).Take(amount);
            var randomProducts = randomIndexes.Select(index => products[index]).ToList();
            return randomProducts;
        }




    }
}
