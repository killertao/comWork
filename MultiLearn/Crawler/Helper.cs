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

                {"Param", "案件类型: 刑事案件"},
               //{"Param", "DocID: 'f42dfa1f-b5ca-4a22-a416-a74300f61906'"},
                {"Index", "100"},//页数最大是100
                {"Page", "20"},
                {"Order", "法院层级"},
                {"Direction", "desc"}
            })).Result;
            string rtn = result.Content.ReadAsStringAsync().Result;

            //Task<HttpResponseMessage> response= client.PostAsync(url,null);
      

        }
    }
}
