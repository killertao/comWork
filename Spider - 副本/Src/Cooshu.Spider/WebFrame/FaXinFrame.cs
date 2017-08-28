using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Cooshu.Spider.Core;
using Cooshu.Spider.Model;


namespace Cooshu.Spider.WebFrame
{
    public class FaXinFrame : SiteFrame
    {
        public override string SpiderTaskSchedulerName { get; protected set; } = "FaXinScheduler";

        public FaXinFrame()
        {
            Header=new Dictionary<string, string>
            {
                { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.101 Safari/537.36" },
            };
        }


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



        private Page CreateFrame()
        {

            //列表页
            var listPage = new Page(this)
            {
                Name = "国家法律列表",
                Encoding = Encoding.UTF8,
                // Pages = new List<Page> { nextList},
                TaskCreatedHandle = (spiderTask, parentSpiderTask) =>
                {
                    // spiderTask.Data["url"] = "http://www.faxin.cn/lib/zyfl/ZyflLibrary.aspx?libid=010101";
                    spiderTask.Url = "http://www.faxin.cn/lib/zyfl/GetZyflData.ashx";
                    spiderTask.PostData = new Dictionary<string, string>
                    {
                        { "searchtype","0"},
                        { "lib","zyfl"},
                        { "chooseNum","010101"},
                        { "firstPage","1"},
                        { "secondPage","1"},
                        { "thirdPage","1"},
                        { "fourthPage","1"},
                        { "fifthPage","1"},
                        { "sixthPage","1"},
                        { "listnum","50"},
                    };
                    //"xiaoli_id=02,0201,0202,0203,0204,0205,0206lib=zyfl&chooseNum=010101&firstPage=1&secondPage=1&thirdPage=1&fourthPage=1&fifthPage=1&sixthPage=1listnum=10
                },
                HtmlLoadedHandle = NextLoadCb
            };
            return listPage;
        }

        Page CreateNextPage(SiteFrame sf)
        {
            return new Page(sf)
            {
                Name = "下一页",
                Encoding = Encoding.UTF8,
                TaskCreatedHandle = (spiderTask, parentSpiderTask) =>
                {
                    spiderTask.Url = "http://www.faxin.cn/lib/zyfl/GetZyflData.ashx";
                    spiderTask.PostData = parentSpiderTask.PostData;
                    spiderTask.PostData["firstPage"] = spiderTask.PostData["secondPage"] =
                    spiderTask.PostData["thirdPage"] = spiderTask.PostData["fourthPage"] = spiderTask.PostData["fifthPage"] = spiderTask.PostData["sixthPage"] = (Convert.ToInt32(parentSpiderTask.PostData["sixthPage"]) + 1).ToString();
                },
                HtmlLoadedHandle = NextLoadCb
            };
        }

        Page CreateDetailPage(SiteFrame sf, string url)
        {

            return new Page(sf)
            {
                Name = "法律详细页",
                AjaxMethod = Page.EAjaxMethod.Get,
                Encoding = Encoding.UTF8,
                LoginHelper = new FaXInLoginHelper(this),
                TaskCreatedHandle = (spiderTask, parentSpiderTask) =>
                {
                    spiderTask.Url = url;
                },
                Pages =new List<Page>(),
                HtmlLoadedHandle = ts =>
                 {//插入数据
                     
                     string content = ts.ResponseData.HtmlText;
                     var titleReg = new Regex("<div class=\"law-title\">([\\s\\S]+?)<a>");
                     string title = titleReg.Match(content).Groups[1].Value;
                     if (title.Contains("您的访问频率过快"))
                     {
                         Thread.Sleep(200);
                         SchedulerInstance._spiderTaskStack.Push(ts);
                         return;
                        

                        
                     }
                     var spaceReg = new Regex("\\s");
                     title= spaceReg.Replace(title, "");
                     var  allreg = new Regex("<div class=\"nop-t1\">([\\s\\S]+?)</div>\\s+<div class=\"nop-t2\">([\\s\\S]+?)</div>");
                     var matches = RegexHelper.GetListMatch(content, allreg);
                     string number = matches.Single(a => a.Groups[1].Value.Contains("文号")).Groups[2].Value;
                     if (SourceArticle.Any(a => number.Contains(a.Number)))
                     {
                         return;
                     }
                     string institution =new Regex(@"[^\u4e00-\u9fa5]+").Replace(matches.Single(a => a.Groups[1].Value.Contains("制定机关")).Groups[2].Value,"");
                     //string number = matches.Single(a => a.Groups[1].Value.Contains("文号")).Groups[2].Value;
                     string publishDate = matches.Single(a => a.Groups[1].Value.Contains("公布日期")).Groups[2].Value;
                     string executeDate = matches.Single(a => a.Groups[1].Value.Contains("施行日期")).Groups[2].Value;
                     //string court = matches.Single(a => a.Groups[1].Value.Contains("文号")).Groups[2].Value;
                     string level =new Regex(@"[^\u4e00-\u9fa5]+").Replace(matches.Single(a => a.Groups[1].Value.Contains("效力等级")).Groups[2].Value,"");
                     new SourceArticle
                     {
                         Title = title,
                         PublishInstitution =spaceReg.Replace(institution,""),
                         Number =spaceReg.Replace(number,""),
                         PublishDate = string.IsNullOrWhiteSpace(publishDate) ? (DateTime?)null : DateTime.Parse(publishDate.Replace(".", "-")),
                         ExecuteDate = string.IsNullOrWhiteSpace(executeDate) ? (DateTime?)null : DateTime.Parse(executeDate.Replace(".", "-")),
                         //Court =court,
                         Url = ts.Url,
                         RawText = ExtractArticle(content),
                         RawHtml = content,
                         PotencyLevel = level,
                         CreateDate = DateTime.Now
                     }.Add();
                 }
            };
        }


        public void NextLoadCb(SpiderTask ts)
        {
            //1.抓取数据填充到数据库创立子详细页面任务
            Regex reg1 = new Regex(@"ZyflContent.aspx\?gid=[^\s]+&libid=010101");
            var matchs = reg1.Matches(ts.ResponseData.HtmlText);
            ts.Page.Pages = new List<Page>();
            foreach (Match match in matchs)
            {
                var url = "http://www.faxin.cn/lib/zyfl/" + match.Value;
                ts.Page.Pages.Add(CreateDetailPage(this, url));
            }
            //2. 创建下一页任务
            ts.Page.Pages.Add(CreateNextPage(this));
        }


        public static string ExtractArticle(string htmlArticle)
        {
            //提取正文HTML内容
            var contentPattern = "<div class=\"content\">[\\s\\S]+</div>[\\s\\S]+<span id=\"omitSpan\"";
            var contentText =new Regex(contentPattern).Match(htmlArticle).Value.Replace("<span id=\"omitSpan\"","");
          

            //删除Html标签
            contentText = Regex.Replace(contentText, @"</?span[^>]*>", "", RegexOptions.IgnoreCase);
            contentText = Regex.Replace(contentText, @"<(title|style|script)[^>]*>((?!</(title|style|script)>)[\s\S])*</(title|style|script)[^>]*>", "", RegexOptions.IgnoreCase);
            contentText = Regex.Replace(contentText, @"</?div[^>]*>", "\n", RegexOptions.IgnoreCase);
            contentText = Regex.Replace(contentText, @"</?p[^>]*>", "\n", RegexOptions.IgnoreCase);
            contentText = Regex.Replace(contentText, @"<br[^>]*>", "\n", RegexOptions.IgnoreCase);
            contentText = Regex.Replace(contentText, @"<[^>]*>", "", RegexOptions.IgnoreCase);
            contentText = Regex.Replace(contentText.Replace("&nbsp;", " "), @"\n[ 　		\t]+", "\n", RegexOptions.IgnoreCase);
            contentText = Regex.Replace(contentText, @"[ 　		\t]+\n", "\n", RegexOptions.IgnoreCase);
            contentText = Regex.Replace(contentText, @"\n+", "\n", RegexOptions.IgnoreCase).Trim('\n');

            //清除目录
            contentText = Regex.Replace(contentText, @"(目[　 ]*录[　 ]*)?[\r\n]*(第一章[　 ]*.*)([\r\n]*((?!第[1１一]+章)第[\d０１２３４５６７８９零一二三四五六七八九十]+章)[　 ]*.*)+", "");
            contentText = contentText.Replace("\n", "<br>");

            return contentText;
        }

   

    }
    public partial class FaXInLoginHelper : LoginHelper
    {
        public FaXInLoginHelper(SiteFrame siteFrame) : base(siteFrame)
        {
      
         
        }

        //是否退出没有登陆
        public override bool IsLogouted(SpiderTask task, ResponseData responseData)
        {
            if (responseData.HtmlText.Contains("user_nav"))
            {
                return false;
            }
            return true;
        }

        public override bool Login(SpiderTask task)
        {
            task.SpiderContext.CookieContainer = new HttpCookieContainer();
            var loginUrl = "http://www.faxin.cn/login.aspx";
            var result = new HttpVisitor(loginUrl, task.SpiderContext.CookieContainer).GetString("user_name=codekiller&user_password=happytao123");
             
            return true;
        }

        public override void Logout(SpiderTask task, string hit = "账号退出\r\n")
        {
            var logoutUrl = "http://www.faxin.cn/LogOut.aspx";
            var result=new HttpVisitor(logoutUrl, task.SpiderContext.CookieContainer).GetString();
        }

        public override bool TryAutoLogin(SpiderTask task)
        {
            task.SpiderContext.CookieContainer = new HttpCookieContainer();
            var loginUrl = "http://www.faxin.cn/login.aspx";
            var headers = new Dictionary<string, string>
            {
                {"Referer", loginUrl},
                {"User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.101 Safari/537.36"},
                {"Origin", "http://www.faxin.cn"},
                {"Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8"}
            };
           var hv  =new HttpVisitor(loginUrl, task.SpiderContext.CookieContainer,Encoding.UTF8,headers);
            var result = hv.GetString((request) =>
            {
                request.Method = "POST";
                ((HttpWebRequest) request).AllowAutoRedirect = false;
                // byte[] buffer = Encoding.Default.GetBytes("user_name=qwerdf123&user_password=123456");
                byte[] buffer = Encoding.Default.GetBytes("user_name=codekiller&user_password=happytao123");
                request.GetRequestStream().Write(buffer,0,buffer.Length);
            });
            return  hv.Response.Headers["Set-Cookie"].Contains("lawapp_web");
        }
   
    }

}
