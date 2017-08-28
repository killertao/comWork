using System;
using System.Collections.Generic;

namespace Cooshu.Spider.Core
{
    public abstract class SiteFrame
    {

        protected SiteFrame()
        {
           
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            SchedulerInstance = SpiderTaskScheduler.Instance(SpiderTaskSchedulerName,this);//初始化调度对象

        }

        /// <summary>
        /// 开始抓取
        /// </summary>
        public abstract void Start(int threadCount);
        
        public virtual void Pause(Action afterCancel=null)
        {
            Running = false;
            SchedulerInstance.Pause(afterCancel);

        }

        public virtual void Stop(Action afterCancel=null)
        {
            SchedulerInstance.Stop(afterCancel);
            Running = false;
        }

        public virtual void Continue(int threadCount)
        {
            SchedulerInstance.Start(threadCount);
        }
        public virtual void AppendProcess()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract string SpiderTaskSchedulerName { get; protected set; } //调度进程的名称

        /// <summary>
        /// 是否在运行中
        /// </summary>
        protected bool Running;  

        /// <summary>
        /// 
        /// </summary>
        public SpiderTaskScheduler SchedulerInstance; //调度

        public Dictionary<string, string> Header;
    }

    /// <summary>
    /// 爬虫内容类  
    /// </summary>
    public class SpiderContext
    {
        public SpiderContext(int number, SiteFrame siteFrame )
        {
            Number = number;
            SiteFrame = siteFrame;

            if (SiteFrame.Header == null)
            {
                SiteFrame.Header = new Dictionary<string, string>();
            }
        }

        public HttpCookieContainer CookieContainer = new HttpCookieContainer();
        
        public string CurrentLoginUser;

        public SiteFrame SiteFrame;

        /// <summary>
        /// 验证登录的次数
        /// </summary>
        public int CheckLoginCount;

        public int Number;
    }
}