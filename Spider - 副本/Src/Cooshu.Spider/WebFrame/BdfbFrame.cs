using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Cooshu.Spider.Core;
using Cooshu.Spider.Model;
using SpiderContext = Cooshu.Spider.Model.SpiderContext;

namespace Cooshu.Spider.WebFrame
{
    /// <summary>
    /// 中国裁判该书网
    /// </summary>
    public class BdfbFrame:SiteFrame
    {

        public override string SpiderTaskSchedulerName { get; protected set; } = "Bdfb_flfg_Scheduler";

        /// <summary>
        /// 开始抓取
        /// </summary>
        public override void Start(int threadCount)
        {
            if (Running)
            {
                return;
            }
            
            SchedulerInstance.AddTask(SpiderTask.Create(CreateFrame(), ""));
            SchedulerInstance.Start(threadCount);
            Running = true;
        }

        private readonly static object locker = new object();
        private int currentId = -100;
        private int maxId = 1262796;

        private readonly Dictionary<string, string>  _states = new Dictionary<string, string> { { "现行有效", "1" }, { "尚未生效", "2" }, { "已被修改", "3" }, { "部分失效", "4" }, { "失效", "5" } };

        private readonly Dictionary<string, KeyValuePair<string, int>> _levels = new Dictionary<string, KeyValuePair<string, int>>
        {
            {"xm0701", new KeyValuePair<string, int>("其他参考文件",7)},
            {"xm0702", new KeyValuePair<string, int>("其他参考文件",7)},
            {"xm0703", new KeyValuePair<string, int>("其他参考文件",7)},
            {"xm0704", new KeyValuePair<string, int>("其他参考文件",7)},
            {"xo08", new KeyValuePair<string, int>("其他参考文件",7)},
            {"xp08", new KeyValuePair<string, int>("其他参考文件",7)},
            {"部门规范性文件", new KeyValuePair<string, int>("国务院各部门工作文件",8)},
            {"部门规章", new KeyValuePair<string, int>("部门规章",9)},
            {"地方规范性文件", new KeyValuePair<string, int>("地方规范性文件",10)},
            {"地方司法文件", new KeyValuePair<string, int>("地方司法文件",11)},
            {"地方政府规章", new KeyValuePair<string, int>("地方政府规章",12)},
            {"法律", new KeyValuePair<string, int>("法律",13)},
            {"法律解释", new KeyValuePair<string, int>("法律解释",14)},
            {"工作答复", new KeyValuePair<string, int>("全国人大常委会法制工作委员会工作文件",15)},
            {"工作文件", new KeyValuePair<string, int>("全国人大及其常委会工作文件",16)},
            {"国务院规范性文件", new KeyValuePair<string, int>("国务院工作文件",17)},
            {"较大市地方性法规", new KeyValuePair<string, int>("较大市地方性法规",18)},
            {"经济特区法规", new KeyValuePair<string, int>("经济特区法规",19)},
            {"军事法规", new KeyValuePair<string, int>("军事法规",20)},
            {"军事规范性文件", new KeyValuePair<string, int>("军事规范性文件",21)},
            {"军事规章", new KeyValuePair<string, int>("军事规章",22)},
            {"两高工作文件", new KeyValuePair<string, int>("两高工作文件",23)},
            {"任免", new KeyValuePair<string, int>("任免",24)},
            {"省级地方性法规", new KeyValuePair<string, int>("省、自治区、直辖市地方性法规",25)},
            {"司法解释", new KeyValuePair<string, int>("司法解释",26)},
            {"司法解释性质文件", new KeyValuePair<string, int>("司法文件",27)},
            {"团体规定", new KeyValuePair<string, int>("政党文件",28)},
            {"行业规定", new KeyValuePair<string, int>("行业文件",29)},
            {"行政法规", new KeyValuePair<string, int>("行政法规",30)},
            {"行政法规解释", new KeyValuePair<string, int>("国务院法制办公室工作文件",31)},
            {"有关法律问题的决定", new KeyValuePair<string, int>("全国人大及其常委会决定、决议",32)},
            {"自治条例和单行条例", new KeyValuePair<string, int>("自治条例和单行条例",33)},
            {"社团文件", new KeyValuePair<string, int>("社团文件",1003)},
            {"附件", new KeyValuePair<string, int>("附件",1004)}
        };
        

        public override void AppendProcess()
        {
            for (var i = 0; i < 1; i++)
            {
                var threadTask = new Task(GenerateRawText);

                threadTask.Start();
            }
        }

        private void GenerateRawText()
        {
            int id;
            lock (locker)
            {
                currentId+=100;
                id = currentId;
            }
            if (id > maxId)
            {
                WinMain.WriteLog($"完成");
                return;
            }
            WinMain.WriteLog($"当前ID“{currentId}\r\n");


            var articles = SourceArticle.Where(a => a.Id >= id && a.Id < id + 100 && a.RawText =="");

            //var articles = SourceArticle.Where(a => a.Succee == null);
            var ids = new List<int>()
            {
                172975,134537,100,200,400,500,138174,600,60340
            };
            articles = SourceArticle.Where(a => ids.Contains(a.Id));

            foreach (var article in articles)
            {
                try
                {
                    var text = ExtractArticle(article.RawHtml);
                    //var state = _states.ContainsKey(article.State) ? _states[article.State] : "";
                    //var level = _levels.ContainsKey(article.PotencyLevel) ? _levels[article.PotencyLevel] : new KeyValuePair<string, int>("其他参考文件", 7);
                    //var properties =
                    //    $"[{{id:\"ExtendProperty8\",value:\"\",text:\"\"}}," +
                    //    $"{{id:\"ExtendProperty7\",value:\"{state}\",text:\"{article.State}\"}}," +
                    //    $"{{id:\"ExtendProperty6\",value:\"{level.Value}\",text:\"{level.Key}\"}}," +
                    //    $"{{id:\"ExtendProperty5\",value:\"{article.ExecuteDate?.ToString("yyyy-MM-dd")}\",text:\"{article.ExecuteDate?.ToString("yyyy-MM-dd")}\"}}," +
                    //    $"{{id:\"ExtendProperty4\",value:\"{article.PublishDate?.ToString("yyyy-MM-dd")}\",text:\"{article.PublishDate?.ToString("yyyy-MM-dd")}\"}}," +
                    //    $"{{id:\"ExtendProperty3\",value:\"{article.Number}\",text:\"{article.Number}\"}}," +
                    //    $"{{id:\"ExtendProperty2\",value:\" {article.PublishInstitution}\",text:\" {article.PublishInstitution}\"}}]";
                    article.RawText = text;
                    //article.ExtendProperty = properties;
                    //article.PotencyLevelType = level.Value;
                    //article.Succee = !Regex.IsMatch(article.RawText, "^目[　 ]*录$");

                    if (article.Succee != true)
                    {

                    }

                    SourceArticle.Update(a=>a.Id==article.Id, sourceArticle => new SourceArticle {RawText = text});
                    //WinMain.WriteLog($"Id:{article.Id}已经处理完成\r\n");
                }
                catch
                {
                    // ignored
                }
            }

            var threadTask = new Task(GenerateRawText);
            threadTask.Start();
        }

        public static string ExtractArticle(string htmlArticle)
        {
            //提取正文HTML内容
            var contentPattern = @"<div[^>]*id=""?div_content""?[^>]*>(?<article>[\s\S]*)</div>[^<]*</td>[^<]*</tr>[^<]*<tr[^>]*>[^<]*</tr>[^<]*<tr>[^<]*<td[^>]*><div class=""?fulltext_guanggao""?>";
            var contentText = RegexHelper.GetDictionary(htmlArticle, contentPattern)["article"];

            //清除法宝联想内容
            contentText = Regex.Replace(contentText, @"（<font color=""red"" >法宝联想</font>[\s\S]*?&nbsp;）","");

            //清除【全文】、【法宝引证码】内容
            contentText = Regex.Replace(contentText, @"<table[\s\S]*?【全文】[\s\S]*?法宝引证码[\s\S]*?</table>", "");

            //清除【本法变迁史】内容
            contentText = Regex.Replace(contentText, @"<font class='TiaoYin'>.*【本法变迁史】</font>(<br>[^<]*<a href=javascript:SLC\([^\(]*\) class=alink>[^<]*</a>)*", "");

            //处理表格

            //摄取表格内容
            var matchedGroups = Regex.Matches(
                contentText, 
                @"<table[^>]*>((?!</?table.*?>)[\s\S])*(((?'Open'<table[^>]*>)((?!</?table.*?>)[\s\S])*)+((?'-Open'</table>)((?!</?table.*?>)[\s\S])*)+)*(?(Open)(?!))</table>", 
                RegexOptions.IgnoreCase);
            var ignores = new List<string>();
            var index = 0;
            for (var i = matchedGroups.Count - 1; i > -1; i--)
            {
                ignores.Add(matchedGroups[i].Value);
                contentText = contentText.Remove(matchedGroups[i].Index, matchedGroups[i].Length);
                contentText = contentText.Insert(matchedGroups[i].Index, "@{忽略内容点位符" + index + "}");
                index++;
            }


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

            //还原忽略的内容
            index = 0;
            foreach (var item in ignores)
            {
                contentText = contentText.Replace("@{忽略内容点位符" + index + "}", item);
                index++;
            }

            return contentText;
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
                Pattern =@"<a[^>]*href=""(?<url>fulltext_form\.aspx[^""]*)""[^>]*>(?<title>[^>]*)</a>",
                LoginHelper = new BdfbLoginHelper(this),
                Priority = SpiderTaskPriority.First,
                TaskCreatedHandle = (newSpiderTask, parentSpiderTask) =>
                {
                    newSpiderTask.Cancel = SourceArticle.Any(a => a.Url == newSpiderTask.Url);
                },
                HtmlLoadedHandle = task =>
                {
                    var institution = RegexHelper.GetDictionary(task.ResponseData.HtmlText, @"<td[^>]*>((?!</td>)[\s\S])*【发布部门】((?!</td>)[\s\S])*<a[^>]*>(?<institution>[^>]*)</a>((?!</td>)[\s\S])*</td>").GetValue("institution");
                    var number = RegexHelper.GetDictionary(task.ResponseData.HtmlText, @"<td[^>]*>((?!</td>)[\s\S])*【发文字号】((?!</td>)[\s\S])*((?!</td>)[\s\S])* (?<number>[^<]*)</td>").GetValue("number");
                    var publishDate = RegexHelper.GetDictionary(task.ResponseData.HtmlText, @"<td[^>]*>((?!</td>)[\s\S])*【发布日期】((?!</td>)[\s\S])*((?!</td>)[\s\S])*(?<publishDate>\d{4}.\d+.\d+)</td>").GetValue("publishDate");
                    var executeDate = RegexHelper.GetDictionary(task.ResponseData.HtmlText, @"<td[^>]*>((?!</td>)[\s\S])*【实施日期】((?!</td>)[\s\S])*((?!</td>)[\s\S])*(?<executeDate>\d{4}.\d+.\d+)</td>").GetValue("executeDate");
                    var state = RegexHelper.GetDictionary(task.ResponseData.HtmlText, @"<td[^>]*>((?!</td>)[\s\S])*【时效性】((?!</td>)[\s\S])*<a[^>]*>(?<state>[^>]*)</a>((?!</td>)[\s\S])*</td>").GetValue("state");
                    var subtype = RegexHelper.GetDictionary(task.ResponseData.HtmlText, @"<td[^>]*>((?!</td>)[\s\S])*【法规类别】((?!</td>)[\s\S])*<a[^>]*>(?<subtype>[^>]*)</a>((?!</td>)[\s\S])*</td>").GetValue("subtype");
                    var type = RegexHelper.GetDictionary(task.ResponseData.HtmlText, @"<a[^>]*>(?<type>[^>]*)</a>[^<]*<SPAN style=""COLOR: ?#778899"">正文阅览</SPAN>").GetValue("type");
                    var level = RegexHelper.GetDictionary(task.ResponseData.HtmlText, @"<td[^>]*>((?!</td>)[\s\S])*【效力级别】((?!</td>)[\s\S])*<a[^>]*>(?<level>[^>]*)</a>((?!</td>)[\s\S])*</td>").GetValue("level");

                    //通过递归抓取Div中的文章内容
                    //var contentPattern = @"<div[^>]*id=""?div_content""?[^>]*>(?<article>((?!</?div.*?>)[\s\S])*(((?'Open'<div[^>]*>)((?!</?div.*?>)[\s\S])*)+((?'-Open'</div>)((?!</?div.*?>)[\s\S])*)+)*(?(Open)(?!)))</div>";
                    var contentText = ExtractArticle(task.ResponseData.HtmlText);

                    new SourceArticle
                    {
                        Title = task.Data["title"],
                        PublishInstitution = institution,
                        Number = number,
                        PublishDate = string.IsNullOrWhiteSpace(publishDate) ? (DateTime?)null : DateTime.Parse(publishDate.Replace(".","-")),
                        ExecuteDate = string.IsNullOrWhiteSpace(executeDate) ? (DateTime?)null : DateTime.Parse(executeDate.Replace(".", "-")),
                        State = state,
                        Url = task.Url,
                        RawText = contentText,
                        RawHtml = task.ResponseData.HtmlText,
                        Type = type,
                        SubType = subtype,
                        PotencyLevel = level
                    }.Add();
                }
            };

            //时效性列表
            var timelinessList = new Page(this)
            {
                Name = "法律时效",
                Pattern = @"'''(?<url>[^']*)'''",
                HtmlLoadedHandle = task =>
                {
                    //获得页码信息
                    var pager = RegexHelper.GetDictionary(task.ResponseData.HtmlText, @"第 *(?<page>\d+) *页/共 *(?<totalPage>\d+) *页");
                    var page = int.Parse(pager["page"])-1;
                    var totalPage = int.Parse(pager["totalPage"]);

                    //判断是不是能整除100,因为每页列表页希望成生成5页子列表页=1&page_count=5430
                    const int listPageCount = 20;
                    if (page % listPageCount != 0)
                    {
                        return;
                    }

                    //附加列表页内容到html中以便生成任务
                    var baseUrl = Regex.Replace(task.Url, @"&?aim_page=\d+", "", RegexOptions.IgnoreCase);
                    baseUrl = Regex.Replace(baseUrl, @"&?page_count=\d+", "", RegexOptions.IgnoreCase);
                    baseUrl = Regex.Replace(baseUrl, @"&?hidtrsWhere=.*?&", "", RegexOptions.IgnoreCase);
                    var hidtrsWhere = RegexHelper.GetDictionary(task.ResponseData.HtmlText, "value=\"(?<hidtrsWhere>.*?)\"").GetValue("hidtrsWhere");
                    var appendHtml = "";
                    for (var i = page+1; i <= page + listPageCount && i < totalPage; i ++)
                    {
                        appendHtml += $"'''{baseUrl}&aim_page={i}&page_count={totalPage}&hidtrsWhere={hidtrsWhere}'''<br/>\r\n";
                    }
                    task.ResponseData.HtmlText += appendHtml;
                }
            };
            timelinessList.Pages = new List<Page> { detail, timelinessList };
            
            //首页
            return new Page(this)
            {
                Name = "首页", Pages = new List<Page> { timelinessList } ,
                HtmlLoadedHandle = task =>
                {
                    task.ResponseData = new ResponseData
                    {
                        HtmlText = @"
                    //'''http://fjlx.pkulaw.cn/doSearch.ashx?Db=chl&clusterwhere=%2525e6%252597%2525b6%2525e6%252595%252588%2525e6%252580%2525a7%25253d01&Search_Mode=accurate&range=name&menu_item=law'''
                    //'''http://fjlx.pkulaw.cn/doSearch.ashx?Db=chl&clusterwhere=%2525e6%252597%2525b6%2525e6%252595%252588%2525e6%252580%2525a7%25253d02&Search_Mode=accurate&range=name&menu_item=law'''
                    //'''http://fjlx.pkulaw.cn/doSearch.ashx?Db=chl&clusterwhere=%2525e6%252597%2525b6%2525e6%252595%252588%2525e6%252580%2525a7%25253d03&Search_Mode=accurate&range=name&menu_item=law'''
                    //'''http://fjlx.pkulaw.cn/doSearch.ashx?Db=chl&clusterwhere=%2525e6%252597%2525b6%2525e6%252595%252588%2525e6%252580%2525a7%25253d04&Search_Mode=accurate&range=name&menu_item=law'''
                    //'''http://fjlx.pkulaw.cn/doSearch.ashx?Db=chl&clusterwhere=%2525e6%252597%2525b6%2525e6%252595%252588%2525e6%252580%2525a7%25253d05&Search_Mode=accurate&range=name&menu_item=law'''"
                    };

                    //task.HtmlText = @"
                    //'''http://fjlx.pkulaw.cn/doSearch.ashx?Db=chl&clusterwhere=%2525e6%252597%2525b6%2525e6%252595%252588%2525e6%252580%2525a7%25253d01&Search_Mode=accurate&range=name&menu_item=law'''
                    //'''http://fjlx.pkulaw.cn/doSearch.ashx?Db=chl&clusterwhere=%2525e6%252597%2525b6%2525e6%252595%252588%2525e6%252580%2525a7%25253d02&Search_Mode=accurate&range=name&menu_item=law'''
                    //'''http://fjlx.pkulaw.cn/doSearch.ashx?Db=chl&clusterwhere=%2525e6%252597%2525b6%2525e6%252595%252588%2525e6%252580%2525a7%25253d03&Search_Mode=accurate&range=name&menu_item=law'''
                    //'''http://fjlx.pkulaw.cn/doSearch.ashx?Db=chl&clusterwhere=%2525e6%252597%2525b6%2525e6%252595%252588%2525e6%252580%2525a7%25253d04&Search_Mode=accurate&range=name&menu_item=law'''
                    //'''http://fjlx.pkulaw.cn/doSearch.ashx?Db=chl&clusterwhere=%2525e6%252597%2525b6%2525e6%252595%252588%2525e6%252580%2525a7%25253d05&Search_Mode=accurate&range=name&menu_item=law'''
                    //'''http://fjlx.pkulaw.cn/doSearch.ashx?Db=lar&clusterwhere=%2525e6%252595%252588%2525e5%25258a%25259b%2525e7%2525ba%2525a7%2525e5%252588%2525ab%25253dXM07&clust_db=lar&Search_Mode=accurate&range=name&menu_item=law'''
                    //'''http://fjlx.pkulaw.cn/doSearch.ashx?Db=lar&clusterwhere=%2525e6%252595%252588%2525e5%25258a%25259b%2525e7%2525ba%2525a7%2525e5%252588%2525ab%25253dXO08&clust_db=lar&Search_Mode=accurate&range=name&menu_item=law'''
                    //'''http://fjlx.pkulaw.cn/doSearch.ashx?Db=lar&clusterwhere=%2525e6%252595%252588%2525e5%25258a%25259b%2525e7%2525ba%2525a7%2525e5%252588%2525ab%25253dXP08&clust_db=lar&Search_Mode=accurate&range=name&menu_item=law'''
                    //'''http://fjlx.pkulaw.cn/doSearch.ashx?Db=lar&clusterwhere=%2525e6%252595%252588%2525e5%25258a%25259b%2525e7%2525ba%2525a7%2525e5%252588%2525ab%25253dXP09&clust_db=lar&Search_Mode=accurate&range=name&menu_item=law'''";

                    //task.HtmlText = @"
                    //'''http://fjlx.pkulaw.cn/doSearch.ashx?Db=chl&clusterwhere=%2525e6%252597%2525b6%2525e6%252595%252588%2525e6%252580%2525a7%25253d04&Search_Mode=accurate&range=name&menu_item=law'''";
                }
            };
            
        }
    }

    public partial class BdfbLoginHelper : LoginHelper
    {
        public BdfbLoginHelper(SiteFrame siteFrame) : base(siteFrame)
        {
        }

        public override bool IsLogouted(SpiderTask task, ResponseData responseData)
        {
            //北大法宝每个用户每天只可以看200篇文章，我们只用100篇，给用户只活口
            task.SpiderContext.CheckLoginCount++;
            if (task.SpiderContext.CheckLoginCount > 100)
            {
                Logout(task, $"{Thread.CurrentThread.ManagedThreadId},{task.SpiderContext.Number}“{task.LoginUser}”账号退出(使用超过100次)\r\n");
                task.SpiderContext.CheckLoginCount = 0;
                return true;
            }

            if (responseData.HtmlText.IndexOf("id=login_prompt") > -1)
            {
                Logout(task, $"{Thread.CurrentThread.ManagedThreadId},{task.SpiderContext.Number}“{task.LoginUser}”账号退出(已经退出状态)\r\n");
                return true;
            }
            else if (responseData.HtmlText.IndexOf("屏蔽恶意访问用户提示") > -1)
            {
                var newResult =
                    new HttpVisitor(task.Url, task.SpiderContext.CookieContainer, task.Page.Encoding, task.SpiderContext.SiteFrame.Header).GetString();
                //网站bug，有时多请求一次就没有访问限制了
                if (newResult.HtmlText.IndexOf("屏蔽恶意访问用户提示") > -1)
                {
                    Logout(task, $"{Thread.CurrentThread.ManagedThreadId},{task.SpiderContext.Number}“{task.LoginUser}”账号退出（访问受限）\r\n");
                    return true;
                }
                responseData.HtmlText = newResult.HtmlText;
                responseData.ResponseUrl = newResult.ResponseUrl;
                return false;
            }
            return false;
        }

        public override void Logout(SpiderTask task, string hit = "账号退出\r\n")
        {
            lock (locker)
            {
                if (hit == null)
                {
                    hit = $"{Thread.CurrentThread.ManagedThreadId}“{task.LoginUser}”账号退出\r\n";
                }
                if (task.LoginUser == task.SpiderContext.CurrentLoginUser)
                {
                    WinMain.WriteLog(hit);
                    new HttpVisitor("http://fjlx.pkulaw.cn/vip_login/CheckLogin.ashx?t=2&n=1445440847006&menu_item=law", task.SpiderContext.CookieContainer)
                        .GetString();
                }
            }
        }


        public override bool TryAutoLogin(SpiderTask task)
        {
            if (task.LoginUser != task.SpiderContext.CurrentLoginUser)
            {
                return true;
            }
            lock (locker)
            {
                if (task.LoginUser != task.SpiderContext.CurrentLoginUser)
                {
                    return true;
                }

                string lsNumber;
                while (QueueAccount.Count > 0 && QueueAccount.TryDequeue(out lsNumber))
                {
                    if (QueueAccount.Count == 0)
                    {
                        ValidAccount.ForEach(QueueAccount.Enqueue);
                        ValidAccount.Clear();
                    }

                    task.SpiderContext.CookieContainer = new HttpCookieContainer();
                    var password = lsNumber.Substring(lsNumber.Length - 6);
                    var account = "fj" + password;
                    var loginUrl = $"http://fjlx.pkulaw.cn/vip_login/CheckLogin.ashx?t=1&u={account}&p={password}";
                    var result = new HttpVisitor(loginUrl, task.SpiderContext.CookieContainer).GetString();
                    var resultValue= result.HtmlText.StartsWith("p_LoginUser");

                    if (resultValue)
                    {
                        WinMain.WriteLog($"{Thread.CurrentThread.ManagedThreadId},{task.SpiderContext.Number}“{account}”登录成功\r\n");
                        ValidAccount.Add(lsNumber);
                        task.SpiderContext.CurrentLoginUser = account;
                        return true;
                    }
                    else
                    {
                        WinMain.WriteLog($"{Thread.CurrentThread.ManagedThreadId},{task.SpiderContext.Number}“{account}”登录失败\r\n");
                        InvalidAccount.Add(lsNumber);
                    }
                }

                WinMain.WriteLog($"可登录账号已经全部用完\r\n");
                return false;
            }
        }

        public override bool Login(SpiderTask task)
        {
            throw new System.NotImplementedException();
        }

        public List<string> ValidAccount = new List<string>();
        
        public List<string> InvalidAccount = new List<string>();

        public ConcurrentQueue<string> QueueAccount=new ConcurrentQueue<string>(Account);
        
        public readonly object locker = new object();
    }
}


