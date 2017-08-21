using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Cooshu.Spider.Core;
using Cooshu.Spider.Model;

namespace Cooshu.Spider.WebFrame
{
    /// <summary>
    /// 中国裁判该书网
    /// </summary>
    public class TreadTestFrame:SiteFrame
    {

        public override string SpiderTaskSchedulerName { get; protected set; } = "Tread_test_Scheduler";

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

        /// <summary>
        /// 创建抓取页面框架
        /// </summary>
        /// <returns></returns>
        public Page CreateFrame()
        {
            //时效性列表
            var timelinessList = new Page(this)
            {
                Name = "法律时效",
                Pattern = @"'''(?<url>[^']*)'''",
                HtmlLoadedHandle = task =>
                {
                    task.ResponseData.HtmlText = @"
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
";
                }
            };
            timelinessList.Pages = new List<Page> { timelinessList };
            
            //首页
            return new Page(this)
            {
                Name = "首页", Pages = new List<Page> { timelinessList } ,
                HtmlLoadedHandle = task =>
                {
                    task.ResponseData.HtmlText = @"
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
'''http://www.cooshu.com/home/test'''
";
                }
            };
            
        }
    }
    
}
