using System;
using System.Net;


namespace Crawler.Helper
{
    class IPHelper
    {

        /// <summary>  
        /// 获取外网ip地址  
        /// </summary>  
        public static string GetExtenalIpAddress()
        {
            String url = "http://hijoyusers.joymeng.com:8100/test/getNameByOtherIp";
            string IP = "未获取到外网ip";
            try
            {
                //从网址中获取本机ip数据    
                WebClient client = new WebClient();
                client.Encoding = System.Text.Encoding.Default;
                var str = client.DownloadString(url);
                IP = str == "" ? IP : str;
                client.Dispose();
            }
            catch (Exception) { }
            return IP;
        }
    }
}
