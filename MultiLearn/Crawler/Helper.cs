using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;


namespace Crawler
{
    using System.Net;
    class Helper
    {

        public static void GetData()
        {

            string url = "http://wenshu.court.gov.cn/List/ListContent";



            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            ///
            var result = client.PostAsync(url, new FormUrlEncodedContent(new Dictionary<string, string>()
            {

                {"Param", "案件类型: 刑事案件, 案件名称: 奚某某、张某某重婚罪一审刑事裁定书"},
                {"Index", "1"},
                {"Page", "5"},
                {"Order", "法院层级"},
                {"Direction", "asc"}
            })).Result;
            string rtn = result.Content.ReadAsStringAsync().Result;

            //Task<HttpResponseMessage> response= client.PostAsync(url,null);
      

        }
    }
}
