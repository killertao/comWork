using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Cooshu.Spider.Core
{
    public class Page
    {
        public Page(SiteFrame siteFrame)
        {
            SiteFrame = siteFrame;
            LoginHelper = new DefaultLoginHelper(SiteFrame);
        }

        /// <summary>
        /// 页面名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 任务结果完成前执行的自定义处理
        /// </summary>
        public Action<SpiderTask> HtmlLoadedHandle;

        /// <summary>
        /// 任务创建完成后执行的自定义处理
        /// </summary>
        public Action<SpiderTask,SpiderTask> TaskCreatedHandle;

        /// <summary>
        /// 登录帮助类
        /// </summary>
        public LoginHelper LoginHelper;

        /// <summary>
        /// 子页
        /// </summary>
        public List<Page> Pages { get; set; }

        /// <summary>
        /// 正则表达式
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// 块表达式,提取特定区别内容的表达式,Pattern表达式将从块表达式的结果中提取数据
        /// </summary>
        public string BlockPattern { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public Encoding Encoding { get; set; }= Encoding.Default;

        /// <summary>
        /// 所属的SiteForm
        /// </summary>
        public SiteFrame SiteFrame { get; private set; }

        public SpiderTaskPriority Priority { get; set; } = SpiderTaskPriority.Second;
    }
}
