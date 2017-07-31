using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Cooshu.Spider.Core
{
    public class SpiderTask
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        public void Execute(SpiderTaskScheduler scheduler)
        {
            //通过Url抓取页面数据
            if (!string.IsNullOrWhiteSpace(Url))
            {
                 // LoginUser = SpiderContext.CurrentLoginUser;
            
                  //  ? new HttpVisitor(Url, SpiderContext.CookieContainer, Page.Encoding, SpiderContext.SiteFrame.Header).GetString(PostData)
                  //  : new HttpVisitor(Url, SpiderContext.CookieContainer, Page.Encoding, SpiderContext.SiteFrame.Header).GetString(PostJson);

                //判断是否登录的，直接退出
      


              //  ResponseData = result;
            }
            Page.HtmlLoadedHandle?.Invoke(this);

            if (Page.Pages == null)
            {
                return;
            }

            //生成子页任务
            for (var i = Page.Pages.Count-1; i >= 0; i--)
            {
                var childPage = Page.Pages[i];
                List<Dictionary<string, string>> matchDatas;

                //如果BlockPattern有正则表达式,从指定区域抓取数据,否则从所有内容中抓取数据
                if (string.IsNullOrEmpty(childPage.BlockPattern))
                {
                    matchDatas = RegexHelper.GetList(ResponseData.HtmlText, childPage.Pattern);
                }
                else
                {
                    var blockData = RegexHelper.GetDictionary(ResponseData.HtmlText, childPage.BlockPattern);
                    if (blockData.Count == 0)
                    {
                        continue;
                    }
                    matchDatas = RegexHelper.GetList(blockData.First().Value, childPage.Pattern);
                }

                //通过匹配的数据创建子页任务
                matchDatas.Reverse();
                foreach (var newSpiderTask in matchDatas.Select(matchData => Create(childPage, matchData, this)))
                {
                    if (!newSpiderTask.Cancel)
                    {
                        scheduler.AddTask(newSpiderTask);
                    }
                    //TODO:测试时只用一条,所以这些直接结束循环
                    //break;
                }
            }
        }

        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="page">任务对应的页面</param>
        /// <param name="data">初始任务数据</param>
        /// <param name="parentSpiderTask">父任务,即创建此任务的任务</param>
        /// <returns></returns>
        public static SpiderTask Create(Page page, Dictionary<string, string> data, SpiderTask parentSpiderTask)
        {
            var newSpiderTask = new SpiderTask
            {
                Data = data,
                Page = page,
                FailedTimes = 0,
                Priority = page.Priority
            };
            
            FixUrl(newSpiderTask, parentSpiderTask?.ResponseData.ResponseUrl);
            page.TaskCreatedHandle?.Invoke(newSpiderTask, parentSpiderTask);

            if (parentSpiderTask?.Data?.Count > 0)
            {
                foreach (var keyValuePair in parentSpiderTask.Data.Where(keyValuePair => !newSpiderTask.Data.ContainsKey(keyValuePair.Key)))
                {
                    newSpiderTask.Data.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            return newSpiderTask;
        }

        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="page">任务对应的页面</param>
        /// <param name="url">任务Url</param>
        /// <returns></returns>
        public static SpiderTask Create(Page page, string url)
        {
            return Create(page, new Dictionary<string, string> {{"url", url}}, null);

        }

        /// <summary>
        /// Url地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 提交的数据
        /// </summary>
        public Dictionary<string,string> PostData { get; set; }



        /// <summary>
        /// 提交的Json数据
        /// </summary>
        public string PostJson { get; set; }

        /// <summary>
        /// 请求结果
        /// </summary>
        public ResponseData ResponseData { get; set; }

        /// <summary>
        /// 生成此任务的Page
        /// </summary>
        public Page Page { get; set; }

        /// <summary>
        /// 通过HtmlText分析的数据结果
        /// </summary>
        public Dictionary<string,string> Data { get; set; }

        /// <summary>
        /// 失败次数
        /// </summary>
        public int FailedTimes { get; set; }

        /// <summary>
        /// 任务是否已经被取消
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string LoginUser { get; set; }

        /// <summary>
        /// 任务优先级
        /// </summary>
        public SpiderTaskPriority Priority { get; set; } = SpiderTaskPriority.Second;

        /// <summary>
        /// 生成URL
        /// </summary>
        /// <param name="spiderTask"></param>
        /// <param name="parentPageUrl"></param>
        private static void FixUrl(SpiderTask spiderTask, string parentPageUrl)
        {
            if (spiderTask.Data.ContainsKey("url"))
            {
                spiderTask.Url = Utilities.GenerateFullUrl(spiderTask.Data["url"], parentPageUrl);
                spiderTask.Data["url"] = spiderTask.Url;
            }
        }

        public SpiderContext SpiderContext { get; set; }


        public WebBrowser WebBrowser { get; }
    }

    /// <summary>
    /// 任务优先级
    /// </summary>
    public enum SpiderTaskPriority
    {
        First,

        Second,

        Third
    }
}