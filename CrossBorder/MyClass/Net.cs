using System.Net.Http;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

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



    }
}
