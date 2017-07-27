namespace Cooshu.Spider.Core
{
    public abstract class LoginHelper
    {
        public LoginHelper(SiteFrame siteFrame)
        {
            SiteFrame = siteFrame;
        }

        /// <summary>
        /// 判断页面响应的内容是否是被退出登录
        /// </summary>
        /// <returns></returns>
        public abstract bool IsLogouted(SpiderTask task, ResponseData responseData);

        public abstract void Logout(SpiderTask task, string hit = "账号退出\r\n");

        /// <summary>
        /// 尝试自动登录
        /// </summary>
        public abstract bool TryAutoLogin(SpiderTask task);

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns>登录是否成功</returns>
        public abstract bool Login(SpiderTask task);

        public SiteFrame SiteFrame { get; private set; }
    }
}