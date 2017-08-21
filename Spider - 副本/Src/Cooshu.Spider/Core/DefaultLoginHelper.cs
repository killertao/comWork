using System;

namespace Cooshu.Spider.Core
{
    public class DefaultLoginHelper : LoginHelper
    {
        public WebFrame.BdfbLoginHelper Create()
        {
            return new WebFrame.BdfbLoginHelper(SiteFrame);
        }

        public DefaultLoginHelper(SiteFrame siteFrame) : base(siteFrame)
        {
        }

        public override bool IsLogouted(SpiderTask task, ResponseData responseData)
        {
            return false;
        }

        public override void Logout(SpiderTask task, string hit)
        {
        }

        public override bool TryAutoLogin(SpiderTask task)
        {
            throw new NotImplementedException();
        }

        public override bool Login(SpiderTask task)
        {
            throw new NotImplementedException();
        }
    }
}