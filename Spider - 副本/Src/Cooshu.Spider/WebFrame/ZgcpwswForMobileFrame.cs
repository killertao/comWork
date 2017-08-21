using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CaptchaExtractor;
using Cooshu.Spider.Core;
using Cooshu.Spider.Model;
using System.Threading;
using Cooshu.Spider.Modal;
using SpiderContext = Cooshu.Spider.Core.SpiderContext;

namespace Cooshu.Spider.WebFrame
{
    /// <summary>
    /// 中国裁判该书网
    /// </summary>
    public class ZgcpwswForMobileFrame : SiteFrame
    {
        public override string SpiderTaskSchedulerName { get; protected set; } = "ZgcpwswScheduler";

        public ZgcpwswForMobileFrame()
        {
            Header = new Dictionary<string, string>
            {
                {"Content-Type", "application/json"},
                {"User-Agent", "Dalvik/1.6.0 (Linux; U; Android 4.2.2; GT-P5210 Build/JDQ39E)"},
                {"Accept-Encoding", "gzip"}
            };
        }

        /// <summary>
        /// 开始抓取
        /// </summary>
        public override void Start(int threadCount)
        {
            //var a =
            //    "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";
            //var b = DecryptAes(a, "lawyeecourtwensh", "lawyeecourtwensh");
            //var postData = GeneratePostData("0");
            //var c = GetWebResponse("http://wenshuapp.court.gov.cn/MobileServices/GetLawListData", postData);
            //var d = DecryptAes(c.HtmlText.Substring(1, c.HtmlText.Length - 2), "lawyeecourtwensh", "lawyeecourtwensh");
            //return;

            if (Running)
            {
                return;
            }

            var homeTask = SpiderTask.Create(CreateFrame(), "");
            SchedulerInstance.AddTask(homeTask);
            SchedulerInstance.Start(threadCount);
            Running = true;
        }

        private string GeneratePostData(string skip)
        {
            var token = DateTime.Now.ToString("yyyyMMddHHmm") + "lawyeecourtwenshuapp";
            var md5Byte = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(token));
            var md5String = md5Byte.Aggregate("", (current, t) => current + t.ToString("x").PadLeft(2, '0')).ToUpper();
            return new Dictionary<string, string>
            {
                {"limit", "20"},
                {"dicval", "asc"},
                {"dickey", "/CaseInfo/案/@法院层级"},
                {"reqtoken", md5String},
                {"skip", (int.Parse(skip)*20).ToString()},
                {"condition", ""}
            }.Json();
        }

        /// <summary>
        /// 创建抓取页面框架
        /// </summary>
        /// <returns></returns>
        private Page CreateFrame()
        {
            //列表页
            var listPage = new Page(this)
            {
                Name = "列表页",
                Encoding = _encoding,
                Pattern = @"'''(?<page>[^']*)'''",
                TaskCreatedHandle = (spiderTask, parentSpiderTask) =>
                {
                    spiderTask.PostJson = GeneratePostData(spiderTask.Data["page"]);
                    spiderTask.Data["url"] = "http://wenshuapp.court.gov.cn/MobileServices/GetLawListData";
                    spiderTask.Url = "http://wenshuapp.court.gov.cn/MobileServices/GetLawListData";
                },
                HtmlLoadedHandle = task =>
                {
                    var html = task.ResponseData.HtmlText;
                    var list = DecryptAes(html.Substring(1, html.Length - 2), "lawyeecourtwensh", "lawyeecourtwensh").Object<List<ListItem>>();

                    list.ForEach(a=>
                    {
                        if (SourceArticle.Any(b => b.Url == a.文书ID))
                        {
                            return;
                        }
                        new SourceArticle
                        {
                            RawText = "",
                            Url = a.文书ID,
                            Guid = a.文书ID,
                            Number = a.案号,
                            RawHtml = a.Json()
                        }.Add();
                    });

                    var page = int.Parse(task.Data["page"]);
                    var totalPage = 23050000/20;
                    if (page < totalPage)
                    {
                        task.ResponseData.HtmlText = $"//'''{page+1}'''";
                    }

                }
            };
            listPage.Pages = new List<Page> {listPage};

            //首页
            return new Page(this)
            {
                Name = "首页",
                Pages = new List<Page> { listPage },
                HtmlLoadedHandle = task =>
                {
                    task.ResponseData = new ResponseData
                    {
                        //23000000
                        HtmlText = @"
                            //'''0'''"
                    };
                }
            };
        }

        private static string DecryptAes(string content, string key, string iv)
        {

            using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.KeySize = 128;
                aesProvider.BlockSize = 128;
                aesProvider.Key = Encoding.Default.GetBytes(key);
                aesProvider.Mode = CipherMode.CBC;
                aesProvider.Padding = PaddingMode.PKCS7;
                aesProvider.IV = Encoding.Default.GetBytes(iv);
                using (ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor())
                {
                    var inputBuffers = Convert.FromBase64String(content);
                    var results = cryptoTransform.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                    aesProvider.Clear();
                    return Encoding.UTF8.GetString(results);
                }
            }
        }

        /// <summary>
        /// 获得web数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        private ResponseData GetWebResponse(string url, Dictionary<string, string> postData)
        {
            return new HttpVisitor(url, SchedulerInstance?.SpiderContext?.CookieContainer??new HttpCookieContainer(), _encoding, Header).GetString(postData);
        }

        /// <summary>
        /// 获得web数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        private ResponseData GetWebResponse(string url, string postData)
        {
            return new HttpVisitor(url, SchedulerInstance?.SpiderContext?.CookieContainer ?? new HttpCookieContainer(), _encoding, Header).GetString(postData);
        }

        private static readonly object Locker = new object();

        private DateTime _verifyCodingTime = DateTime.Now;

        private const string Domain = "http://wenshu.court.gov.cn";

        private readonly Encoding _encoding = Encoding.UTF8;

        private static readonly List<string> AreaHierarchy = new List<string> { "法院地域", "中级法院", "基层法院" };
        
        private static readonly List<Court> CourtTree = File.ReadAllText(Directory.GetCurrentDirectory() + "/Modal/CourtTree.txt", Encoding.UTF8).Object<List<Court>>();

        //{"案件类型":"1","裁判日期":"2014-12-29","案件名称":"宋皓犯受贿罪刑事决定书","文书ID":"8252121f-8260-4241-b707-018d52d151ca","审判程序":"再审","案号":"（2012）刑监字第182-1号","法院名称":"最高人民法院","关联文书":[{"文书ID":"eff1ed70-b647-11e3-84e9-5cf3fc0c2c18","Type":"2","Mark":"刑事再审","审判程序":"","审理法院":"最高人民法院","案号":"（2012）刑监字第182号","裁判日期":"2013-03-14","结案方式":""}]}

        class ListItem
        {
            public string 案件类型 { get; set; }

            public string 裁判日期 { get; set; }

            public string 案件名称 { get; set; }

            // ReSharper disable once InconsistentNaming
            public string 文书ID { get; set; }

            public string 审判程序 { get; set; }

            public string 案号 { get; set; }

            public string 法院名称 { get; set; }
        }
    }
}