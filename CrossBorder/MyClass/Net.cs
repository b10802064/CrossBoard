using System.Net.Http;
using System.Threading.Tasks;
using System;

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
    }
}
