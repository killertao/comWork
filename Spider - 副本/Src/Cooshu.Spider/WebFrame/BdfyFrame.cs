using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Cooshu.Spider.Core;
using Cooshu.Spider.Model;

namespace Cooshu.Spider.WebFrame
{
    /// <summary>
    /// 中国裁判该书网
    /// </summary>
    public class BdfyFrame : SiteFrame
    {
        public override string SpiderTaskSchedulerName { get; protected set; } = "BdfyScheduler";

        /// <summary>
        /// 开始抓取
        /// </summary>
        public override void Start(int threadCount)
        {
            if (Running)
            {
                return;
            }

            SchedulerInstance.AddTask(SpiderTask.Create(CreateFrame(), "http://www.lawyee.net/webservice/actsearch.asp?query=&Sort=Pub_Date%20desc,ClassID%20asc&start=0"));
            SchedulerInstance.Start(threadCount);
            Running = true;
        }

        /// <summary>
        /// 创建抓取页面框架
        /// </summary>
        /// <returns></returns>
        public Page CreateFrame()
        {
            //详情页
            var detail = new Page(this)
            {
                Name = "详情页",
                Pattern =@"<ActID>(?<articleId>\d+)</ActID>",
                Encoding = Encoding.Default,
                TaskCreatedHandle = (newSpiderTask, parentSpiderTask) =>
                {
                    newSpiderTask.Url = Utilities.GenerateFullUrl("/Act/Act_Display.asp?ChannelID=1010100&KeyWord=&rid="+ newSpiderTask.Data["articleId"],parentSpiderTask.Url);
                    newSpiderTask.Data["url"] = newSpiderTask.Url;
                    newSpiderTask.Cancel = SourceArticle.Any(a=>a.Url== newSpiderTask.Url);
                },
                LoginHelper = new BdfyLoginHelper(this),
                HtmlLoadedHandle = task =>
                {
                    var title = RegexHelper.GetDictionary(task.ResponseData.HtmlText, @"<td[^>]*>[^<>]*法规名称[^<>]*</td>[^<]*<td[^>]*>(?<title>[^<>]*)</td>").GetValue("title");
                    var institution = RegexHelper.GetDictionary(task.ResponseData.HtmlText, @"<td[^>]*>[^<>]*颁布机构[^<>]*</td>[^<]*<td[^>]*>(?<institution>[^<>]*)</td>").GetValue("institution");
                    var number = RegexHelper.GetDictionary(task.ResponseData.HtmlText, @"<td[^>]*>[^<>]*发 文 号[^<>]*</td>[^<]*<td[^>]*>(?<number>[^<>]*)</td>").GetValue("number");
                    var publishDate = RegexHelper.GetDictionary(task.ResponseData.HtmlText, @"<td[^>]*>[^<>]*颁布时间[^<>]*</td>[^<]*<td[^>]*>(?<publishDate>[^<>]*)</td>").GetValue("publishDate");
                    var executeDate = RegexHelper.GetDictionary(task.ResponseData.HtmlText, @"<td[^>]*>[^<>]*实施时间[^<>]*</td>[^<]*<td[^>]*>(?<executeDate>[^<>]*)</td>").GetValue("executeDate");
                    var state = RegexHelper.GetDictionary(task.ResponseData.HtmlText, @"<td[^>]*>[^<>]*效力属性[^<>]*</td>[^<]*<td[^>]*>(?<state>[^<>]*)</td>").GetValue("state");
                    
                    var contentPattern = @"<td id=""?Matter""?[^>]*>(?<article>((?!</?td.*?>)[\s\S])*(((?'Open'<td[^>]*>)((?!</?td.*?>)[\s\S])*)+((?'-Open'</td>)((?!</?td.*?>)[\s\S])*)+)*(?(Open)(?!)))</td>";
                    var contentText = RegexHelper.GetDictionary(task.ResponseData.HtmlText, contentPattern)["article"];

                    new SourceArticle
                    {
                        Title = title,
                        PublishInstitution = institution,
                        Number = number,
                        PublishDate = string.IsNullOrWhiteSpace(publishDate)?(DateTime?)null:DateTime.Parse(publishDate),
                        ExecuteDate = string.IsNullOrWhiteSpace(executeDate) ? (DateTime?)null : DateTime.Parse(executeDate),
                        State = state,
                        Url = task.Url,
                        RawText = contentText,
                        RawHtml = task.ResponseData.HtmlText,
                        Type = "法律法规"
                    }.Add();
                }
            };
            
            //民事案件
            var list = new Page(this)
            {
                Name = "列表页",
                Pattern = @"'''(?<url>[^']*)'''",
                Encoding = Encoding.UTF8,
                HtmlLoadedHandle = task =>
                {
                    //获得当前开始记录数
                    var currentStart = int.Parse(RegexHelper.GetDictionary(task.Url, @"start=(?<start>\d+)")["start"]);

                    //获得总页数
                    var totalCount = int.Parse(RegexHelper.GetDictionary(task.ResponseData.HtmlText, @"<NumFound>(?<total>\d+)</NumFound>")["total"]);

                    //判断是不是能整除100,因为每页列表页希望成生成5页子列表页
                    const int listPageCount = 5;
                    const int pageSize = 20;
                    var baseUrl = Regex.Replace(task.Url, @"start=\d+","");
                    if (currentStart%(listPageCount*pageSize) != 0)
                    {
                        return;
                    }

                    //附加列表页内容到html中以便生成任务
                    var appendHtml = "";
                    for (var i = currentStart + pageSize; i <= currentStart+(listPageCount * pageSize) && i<totalCount; i += 20)
                    {
                        appendHtml += $"<br>\r\n'''{baseUrl}start={i}'''";
                    }
                    task.ResponseData.HtmlText += appendHtml;
                }

            };
            list.Pages=new List<Page> { detail, list };
            
            //首页
            return list;
        }

        public class BdfyLoginHelper : LoginHelper
        {
            public BdfyLoginHelper(SiteFrame siteFrame) : base(siteFrame)
            {
            }

            public override bool IsLogouted(SpiderTask task, ResponseData responseData)
            {
                return responseData.HtmlText.IndexOf("<td class=\"Matter\"><div class='R1'>")>-1;
            }

            public override void Logout(SpiderTask task, string hit = "账号退出")
            {
            }

            public override bool TryAutoLogin(SpiderTask task)
            {
                const string loginUrl = "http://www.lawyee.net/user/check_login.asp?atype=2&Login_Type=Account&UserName=stulib&Password=stulib";
                var result = new HttpVisitor(loginUrl, task.SpiderContext.CookieContainer).GetString();
                return result.HtmlText.IndexOf("登录成功")>-1;
            }

            public override bool Login(SpiderTask task)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}