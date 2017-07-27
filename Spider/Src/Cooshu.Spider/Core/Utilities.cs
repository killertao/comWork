using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cooshu.Spider.Core
{
    public static class Utilities
    {
        /// <summary>
        /// 生成完成Url,因为有些url是../或./开始的
        /// </summary>
        /// <param name="targetUrl"></param>
        /// <param name="currentPageUrl1"></param>
        /// <returns></returns>
        public static string GenerateFullUrl(string targetUrl, string currentPageUrl1)
        {
            //如果Url是完整的Url可以直接使用
            if (targetUrl.StartsWith("http://") || targetUrl.StartsWith("https://"))
            {
                return targetUrl;
            }

            if (string.IsNullOrWhiteSpace(currentPageUrl1))
            {
                return null;
            }
            
            //如果是"根目录"格式的URL
            //获得当前域名
            var domain = RegexHelper.GetDictionary(currentPageUrl1, @"(?<domain>http(s)?:[/\\]{2}[^/\\]*[/\\]).*")["domain"];
            if (targetUrl.StartsWith("/"))
            {
                return domain + targetUrl.Substring(1);
            }

            //获得当前路径
            var currentDirectory = Regex.Replace(currentPageUrl1, @"\?.*", "").Replace("\\", "/");
            var lastIndex = currentDirectory.LastIndexOf("/");
            if (lastIndex > 8)
            {
                currentDirectory = currentDirectory.Substring(0, lastIndex + 1);
            }
            if (!currentDirectory.EndsWith("/"))
            {
                currentDirectory += "/";
            }

            //如果是"相对目录"格式的URL
            targetUrl = currentDirectory + targetUrl;
            while (targetUrl.IndexOf("../") > -1)
            {
                targetUrl = Regex.Replace(targetUrl, @"[^/\\]*[/\\]\.\./", "");
            }

            return targetUrl.Replace("./", "");
        }

        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
            {
                return value;
            }
            else
            {
                return default(TValue);
            }
        }
    }
}
