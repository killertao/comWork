using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CaptchaExtractor;
using Cooshu.Spider.Core;
using Cooshu.Spider.Model;
using System.Threading;
using Cooshu.Spider.Modal;
using SpiderContext = Cooshu.Spider.Core.SpiderContext;
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable UnusedMember.Global

namespace Cooshu.Spider.WebFrame
{
    /// <summary>
    /// 中国裁判该书网
    /// </summary>
    public class ZgcpwswFrame : SiteFrame
    {
        public override string SpiderTaskSchedulerName { get; protected set; } = "ZgcpwswScheduler";

        /// <summary>
        /// 开始抓取
        /// </summary>
        public override void Start(int threadCount)
        {
            if (Running)
            {
                return;
            }

            var homeTask = SpiderTask.Create(CreateFrame(), "");
            SchedulerInstance.AddTask(homeTask);
            SchedulerInstance.Start(threadCount);
            Running = true;
        }

        /// <summary>
        /// 创建抓取页面框架
        /// </summary>
        /// <returns></returns>
        private Page CreateFrame()
        {
            //详情页
            var detail = new Page(this)
            {
                Pattern = @",(?<jsonData>{\\\"".*?\\\""})",
                Name = "详情页",
                Encoding = _encoding,
                Priority = SpiderTaskPriority.First,
                TaskCreatedHandle = (newSpiderTask, parentSpiderTask) =>
                {
                    var item = newSpiderTask.Data["jsonData"].Replace("\\\"", "\"").Object<ListItem>();
                    var url = "http://wenshu.court.gov.cn/content/content?DocID=" + item.文书id;
                    newSpiderTask.Cancel = SourceArticle.Any(a => a.Url == url);
                    newSpiderTask.Data["url"] = url;
                    newSpiderTask.Data["id"] = item.文书id;
                    newSpiderTask.Url = url;

                    if (newSpiderTask.Cancel)
                    {
                        return;
                    }

                    //{\"裁判要旨段原文\":\"本院认为，被第三百五十条第（一）项的规定，裁定如下\",
                    //\"案件类型\":\"1\",
                    //\"裁判日期\":\"2015-09-01\",
                    //\"案件名称\":\"莫志强抢劫死刑复核刑事裁定书\",
                    //\"文书ID\":\"2a6696db-c9a0-4e37-b4bc-c55ea3dc6dc4\",
                    //\"审判程序\":\"复核\",
                    //\"案号\":\"无\",
                    //\"法院名称\":
                    //\"最高人民法院\"}
                    //var newArticle = new SourceArticle
                    //{
                    //    Guid = item.文书ID,
                    //    Title = item.案件名称,
                    //    State = item.审判程序,
                    //    Court = item.法院名称,
                    //    Number = item.案号,
                    //    ExecuteDate = string.IsNullOrWhiteSpace(item.裁判日期) ? (DateTime?) null : DateTime.Parse(item.裁判日期),
                    //    Url = url,
                    //    RawText = item.裁判要旨段原文??"",
                    //    Append = parentSpiderTask.Data["param"]
                    //};

                    newSpiderTask.Data["文书ID"] = item.文书id;
                    newSpiderTask.Data["案件名称"] = item.案件名称;
                    newSpiderTask.Data["审判程序"] = item.审判程序;
                    newSpiderTask.Data["法院名称"] = item.法院名称;
                    newSpiderTask.Data["案号"] = item.案号;
                    newSpiderTask.Data["裁判日期"] = item.裁判日期;
                    newSpiderTask.Data["裁判要旨段原文"] = item.裁判要旨段原文??"";
                    newSpiderTask.Data["Append"] = parentSpiderTask.Data["param"];

                    //先取消操作，不采集详情页数据
                    //newSpiderTask.Cancel = true;
                },

                HtmlLoadedHandle = task =>
                {
                    var docId = task.Data["id"];
                    var url = $"{Domain}/CreateContentJS/CreateContentJS.aspx?DocID={docId}";
                    var contentResult = new HttpVisitor(url,task.SpiderContext.CookieContainer, Encoding.UTF8).GetString();

                    url = $"{Domain}/Content/GetSummary";
                    var summaryResult = GetWebResponse(url, new Dictionary<string, string> { {"docId",docId}});

                    url = $"{Domain}/Content/GetRelateFiles";
                    var relateFilesResult = GetWebResponse(url, new Dictionary<string, string> { { "docId", docId } });
                    
                    new SourceArticle
                    {
                        Type = "案例",
                        Guid = task.Data["文书ID"],
                        Title = task.Data["案件名称"],
                        State = task.Data["审判程序"],
                        Court = task.Data["法院名称"],
                        Number = task.Data["案号"],
                        ExecuteDate = string.IsNullOrWhiteSpace(task.Data["裁判日期"]) ? (DateTime?)null : DateTime.Parse(task.Data["裁判日期"]),
                        PublishDate = string.IsNullOrWhiteSpace(task.Data["上传日期"]) ? (DateTime?)null : DateTime.Parse(task.Data["上传日期"]),
                        Url = task.Url,
                        RawText = task.Data["裁判要旨段原文"] ?? "",
                        Append = task.Data["param"],
                        RawHtml = contentResult.HtmlText,
                        Attachment = summaryResult.HtmlText,
                        ExtendProperty = relateFilesResult.HtmlText
                    }.Add();}
            };

            //下一页
            var nextList = new Page(this)
            {
                Name = "下一页",
                Pages = new List<Page> {detail},
                Encoding = _encoding,
                Pattern = @"```(?<PageIndex>[^`]*)```",
                TaskCreatedHandle = (spiderTask, parentSpiderTask) =>
                {
                    spiderTask.Data["url"] = "http://wenshu.court.gov.cn/List/ListContent";
                    spiderTask.Url = "http://wenshu.court.gov.cn/List/ListContent";
                    var index = int.Parse(spiderTask.Data["PageIndex"]);
                    spiderTask.PostData = new Dictionary<string, string>
                    {
                        {"Param", parentSpiderTask.Data["param"]},
                        {"Index", index.ToString()},
                        {"Page", "20"},
                        {"Order", "裁判日期"},
                        {"Direction", "asc"}
                    };
                },
                HtmlLoadedHandle = task =>
                {
                    if (task.ResponseData.HtmlText == "\"remind\"")
                    {
                        EntryVerifyCode(task.SpiderContext);
                        WinMain.WriteLog("提示输入难码:\r\n");
                        throw new Exception("处理验证码！");
                    }
                }
            };

            //列表页
            var listPage = new Page(this)
            {
                Name = "地区列表页",
                Encoding = _encoding,
                Pattern = @"'''(?<param>[^']*)'''",
                TaskCreatedHandle = (spiderTask, parentSpiderTask) =>
                {
                    var dates = RegexHelper.GetDictionary(spiderTask.Data["param"], "上传日期:(?<publicDate>[\\d-]+) .*裁判日期:(?<refereeDate>[\\d-]+)");
                    var publicDate = DateTime.Parse(dates["publicDate"]);
                    var refereeDate = DateTime.Parse(dates["refereeDate"]);
                    var dailyCount = CountStatistic.FirstOrDefault(a => a.PublicDate == publicDate && a.RefereeDate == refereeDate);
                    if (dailyCount != null 
                    && (dailyCount.Count == 0 || dailyCount.Count <= SourceArticle.Count(a => a.PublishDate == publicDate && a.ExecuteDate == refereeDate)))
                    {
                        spiderTask.Cancel = true;
                    }

                    spiderTask.PostData = new Dictionary<string, string>
                    {
                        {"Param", spiderTask.Data["param"]},
                        {"Index", "1"},
                        {"Page", "20"},
                        {"Order", "裁判日期"},
                        {"Direction", "asc"}
                    };
                    spiderTask.Data["url"] = "http://wenshu.court.gov.cn/List/ListContent";
                    spiderTask.Url = "http://wenshu.court.gov.cn/List/ListContent";
                },
                HtmlLoadedHandle = task =>
                {
                    //判断是否需要模拟输入验证码
                    if (task.ResponseData.HtmlText == "\"remind\"" || task.ResponseData.HtmlText.Contains("<title>访问验证</title>"))
                    {
                        EntryVerifyCode(task.SpiderContext);
                        WinMain.WriteLog("提示输入难码:\r\n");
                        throw new Exception("处理验证码！");
                    }

                    var count = int.Parse(RegexHelper.GetDictionary(task.ResponseData.HtmlText, @"\\\""Count\\\"":\\\""(?<count>\d+)\\\""")["count"]);
                    var dates = RegexHelper.GetDictionary(task.PostData["Param"], "上传日期:(?<publicDate>[\\d-]+) .*裁判日期:(?<refereeDate>[\\d-]+)");
                    var publicDate = DateTime.Parse(dates["publicDate"]);
                    var refereeDate = DateTime.Parse(dates["refereeDate"]);
                    if (task.PostData["Index"] == "1")
                    {
                        CountStatistic.Remove(a => a.PublicDate == publicDate && a.RefereeDate == refereeDate);
                        new CountStatistic
                        {
                            PublicDate = publicDate,
                            RefereeDate = refereeDate,
                            Count = count
                        }.Add();
                    }

                    task.Data["上传日期"] = dates["publicDate"];
                    var totalPage = (count - 1)/20 + 1;
                    for (var i = 2; i <= totalPage; i++)
                    {
                        task.ResponseData.HtmlText += $"```{i}```\r\n";
                    }

                    //var str = CourtTree.Json();
                },
                Pages = new List<Page> {nextList, detail}
            };

            //首页
            return new Page(this)
            {
                Name = "首页",
                Pages = new List<Page> { listPage },
                HtmlLoadedHandle = task =>
                {

                    var publicDateStart = DateTime.Parse("2013-06-28");
                    var publicDateEnd = DateTime.Parse("2013-08-01");
                    var publishDate = publicDateStart;

                    var refereeDateStart = DateTime.Parse("2002-01-01");
                    var refereeDateEnd = DateTime.Parse("2013-08-01");


                    //var publicDateStart = DateTime.Parse("2016-06-28");
                    //var publicDateEnd = DateTime.Parse("2016-06-28");
                    //var publishDate = publicDateStart;

                    //var refereeDateStart = DateTime.Parse("2016-03-01");
                    //var refereeDateEnd = DateTime.Parse("2016-03-01");
                    //var refereeDate = refereeDateStart;

                    var htmlText = new StringBuilder(1000000);
                    var count = 0;
                    while (publishDate <= publicDateEnd)
                    {
                        var refereeDate = refereeDateStart;


                        var tempRefereeDateEnd = DateTime.Parse("2010-01-01");
                        while (refereeDate <= tempRefereeDateEnd)
                        {
                            htmlText.Append($"'''上传日期:{publishDate:yyyy-MM-dd} TO {publishDate:yyyy-MM-dd},裁判日期:{refereeDate:yyyy-MM-dd} TO {refereeDate.AddMonths(1).AddDays(-1):yyyy-MM-dd}'''");
                            refereeDate = refereeDate.AddMonths(1);
                            count++;
                        }

                        while (refereeDate <= refereeDateEnd)
                        {
                            htmlText.Append($"'''上传日期:{publishDate:yyyy-MM-dd} TO {publishDate:yyyy-MM-dd},裁判日期:{refereeDate:yyyy-MM-dd} TO {refereeDate:yyyy-MM-dd}'''");
                            refereeDate = refereeDate.AddDays(1);
                            count ++;
                        }
                        publishDate = publishDate.AddDays(1);
                    }
                    task.ResponseData = new ResponseData
                    {
                        HtmlText = htmlText.ToString()
                    };
                }
            };
        }














        /// <summary>
        /// 模拟输入验证码
        /// </summary>
        /// <param name="spider"></param>
        private void EntryVerifyCode(SpiderContext spider)
        {
            lock (Locker)
            {
                if (DateTime.Now <= _verifyCodingTime.AddSeconds(3))
                {
                    return;
                }

                var responseData = "";
                while (responseData != "1")
                {
                    var verificationCodeBitmap = new HttpVisitor($"{Domain}/User/ValidateCode", spider.CookieContainer).GetBitmap();
                    var verificationCode = Extractor.Run(verificationCodeBitmap, 5, 130, "");
                    var postData = new Dictionary<string, string> {{"ValidateCode", verificationCode.Text}};
                    responseData = new HttpVisitor($"{Domain}/Content/CheckVisitCode", spider.CookieContainer).GetString(postData).HtmlText;
                    Thread.Sleep(100);
                }
                _verifyCodingTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 分解列表为更细粒度类型的列表
        /// </summary>
        /// <param name="task"></param>
        /// <param name="param"></param>
        /// <param name="courts"></param>
        private void SplitListPage(SpiderTask task, string param, IEnumerable<Court> courts)
        {
            foreach (var court in courts)
            {
                task.ResponseData.HtmlText += $"'''{param},法院名称:{court.Name}'''\r\n";

                if (court.Children.Count > 0)
                {
                    SplitListPage(task, param, court.Children);
                }
            }
        }

        private void SplitListPage(SpiderTask task,string param)
        {
            //案件类型
            task.ResponseData.HtmlText += $"'''{param},案件类型:0'''\r\n";
            task.ResponseData.HtmlText += $"'''{param},案件类型:1'''\r\n";
            task.ResponseData.HtmlText += $"'''{param},案件类型:2'''\r\n";
            task.ResponseData.HtmlText += $"'''{param},案件类型:3'''\r\n";
            task.ResponseData.HtmlText += $"'''{param},案件类型:4'''\r\n";
            task.ResponseData.HtmlText += $"'''{param},案件类型:5'''\r\n";
            task.ResponseData.HtmlText += $"'''{param},案件类型:6'''\r\n";

            //审判程序
            task.ResponseData.HtmlText += $"'''{param},审判程序:一审'''\r\n";
            task.ResponseData.HtmlText += $"'''{param},审判程序:二审'''\r\n";
            task.ResponseData.HtmlText += $"'''{param},审判程序:再审'''\r\n";
            task.ResponseData.HtmlText += $"'''{param},审判程序:复合'''\r\n";
            task.ResponseData.HtmlText += $"'''{param},审判程序:刑罚变更'''\r\n";
            task.ResponseData.HtmlText += $"'''{param},审判程序:非诉执行审查'''\r\n";
            task.ResponseData.HtmlText += $"'''{param},审判程序:再审审查与审判监督'''\r\n";
            task.ResponseData.HtmlText += $"'''{param},审判程序:再审审查与审判监督'''\r\n";
            task.ResponseData.HtmlText += $"'''{param},审判程序:其他'''\r\n";

            //案由
            task.ResponseData.HtmlText += $"'''{param},一级案由:刑事案由'''\r\n";
            task.ResponseData.HtmlText += $"'''{param},一级案由:民事案由'''\r\n";
            task.ResponseData.HtmlText += $"'''{param},一级案由:行政案由'''\r\n";
            task.ResponseData.HtmlText += $"'''{param},一级案由:赔偿案由'''\r\n";

            //文书类型:判决书
            task.ResponseData.HtmlText += $"'''{param},文书类型:判决书'''\r\n";
            task.ResponseData.HtmlText += $"'''{param},文书类型:裁定书'''\r\n";
            task.ResponseData.HtmlText += $"'''{param},文书类型:调解书'''\r\n";
            task.ResponseData.HtmlText += $"'''{param},文书类型:决定书'''\r\n";
            task.ResponseData.HtmlText += $"'''{param},文书类型:通知书'''\r\n";
            task.ResponseData.HtmlText += $"'''{param},文书类型:令'''\r\n";
        }

        /// <summary>
        /// 获得法院层级数据
        /// </summary>
        /// <param name="hierarchy"></param>
        /// <param name="baseParam"></param>
        /// <param name="parval"></param>
        /// <returns></returns>
        private IEnumerable<Dictionary<string, string>> GetHierarchyData(int hierarchy, string baseParam, string parval)
        {
            //从服务器抓取数据
            var name = AreaHierarchy[hierarchy];
            var url = hierarchy == 0 ? "http://wenshu.court.gov.cn/List/TreeContent" : "http://wenshu.court.gov.cn/List/CourtTreeContent";
            var param = hierarchy == 0
                ? new Dictionary<string, string> {{"Param", baseParam}}
                : new Dictionary<string, string> {{"Param", baseParam + "," + name + ":" + parval}, {"parval", parval}};
            var hierarchyData = GetWebResponse(url, param).HtmlText;

            //分解数据
            var pattern = $@"Key\\\"":\\\""(?<name>[^\\]+)\\\"",\\\""Value\\\"":\\\""(?<count>\d+)\\\""[^\{{\}}]*\\\""Field\\\"":\\\""{name}\\\""";
            return RegexHelper.GetList(hierarchyData, pattern);
        }

        /// <summary>
        /// 记录无法拆分的大列表页（列表页数超过100页的，并且无法通过法院层级拆分成更小的表列页）
        /// </summary>
        private void RecordLargeList(SpiderTask task)
        {
            //记录日志
            var fullFileName = Directory.GetCurrentDirectory() + "/Log/";
            if (!Directory.Exists(fullFileName))
            {
                Directory.CreateDirectory(fullFileName);
            }
            using (var streamWriter = new StreamWriter(fullFileName + "LargeList.txt", true, _encoding))
            {
                streamWriter.Write($@"
{{
    url:""{task.Url}"",
    postData:""{task.PostData}""
}}");
            }
        }


        /// <summary>
        /// 分解列表为更细粒度类型的列表
        /// </summary>
        /// <param name="task"></param>
        /// <param name="hierarchy"></param>
        /// <param name="parval"></param>
        private void GenerateCourtTree(SpiderTask task, int hierarchy, string parval)
        {
            //获得当前级别的数据，
            var hierarchyData = GetHierarchyData(hierarchy, task.Data["param"], parval);

            //将法院消息记录到CourtTree变量中
            if (hierarchy == 0)
            {
                foreach (var area in hierarchyData)
                {
                    CourtTree.Add(new Court
                    {
                        Province = area["name"],
                        Name = area["name"] + "高级人民法院"
                    });
                }
            }
            else
            {
                foreach (var area in hierarchyData)
                {
                    var parent = hierarchy == 1 ? Get(CourtTree, parval, null) : Get(CourtTree, null, parval);
                    parent.Children.Add(new Court
                    {
                        Province = parent.Province,
                        Name = area["name"]
                    });
                }
            }


            //生成所有下级
            foreach (var area in hierarchyData)
            {
                if (hierarchy == AreaHierarchy.Count - 1)
                {
                    continue;
                }
                
                GenerateCourtTree(task, hierarchy + 1, area["name"]);
            }
        }

        private Court Get(IReadOnlyList<Court> courts, string province, string parval)
        {
            foreach (var court in courts)
            {
                if (court.Name == parval || court.Province == province)
                {
                    return court;
                }

                var childreResult = Get(court.Children, province, parval);
                if (childreResult != null)
                {
                    return childreResult;
                }
            }

            return null;
        }

        /// <summary>
        /// 获得web数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        private ResponseData GetWebResponse(string url, Dictionary<string, string> postData)
        {
            return new HttpVisitor(url, SchedulerInstance.SpiderContext.CookieContainer, _encoding).GetString(postData);
        }

        private static readonly object Locker = new object();

        private DateTime _verifyCodingTime = DateTime.Now;

        private const string Domain = "http://wenshu.court.gov.cn";

        private readonly Encoding _encoding = Encoding.UTF8;

        private static readonly List<string> AreaHierarchy = new List<string> { "法院地域", "中级法院", "基层法院" };

        /// <summary>
        /// 每种列表最大可采集的记录数
        /// </summary>
        private static readonly int MaxCount = 2000;

        private static readonly List<Court> CourtTree = File.ReadAllText(Directory.GetCurrentDirectory() + "/Modal/CourtTree.txt", Encoding.UTF8).Object<List<Court>>();

        class ListItem
        {
            public string 裁判要旨段原文 { get; set; }

            public string 裁判日期 { get; set; }

            public string 案件名称 { get; set; }
            
            public string 文书id { get; set; }

            public string 审判程序 { get; set; }

            public string 案号 { get; set; }

            public string 法院名称 { get; set; }
        }

    }
}