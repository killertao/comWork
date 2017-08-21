using System.Linq;
using System.Net;
using System.Reflection;

namespace Cooshu.Spider.Core
{
    public class HttpCookieContainer
    {
        public string GetHeader()
        {
            return _cookies.Count > 0 ? _cookies.Cast<object>().Aggregate("", (current, cookie) => current + ($" ;{cookie}")).Substring(2) : "";
        }

        public void Update(HttpWebResponse httpWebResponse)
        {
            var cookieCutterFunInfo = typeof (CookieContainer).GetMethod("CookieCutter", BindingFlags.NonPublic | BindingFlags.Instance);
            if (httpWebResponse == null)
            {
                return;
            }
            
            var cookieContainer = new CookieContainer();
            foreach (var headerKey in new[] { "Set-Cookie" , "Set-Cookie2" })
            {
                try
                {
                    var setCookie = httpWebResponse.Headers[headerKey];
                    if (!(setCookie?.Length > 0))
                    {
                        continue;
                    }

                    var invokeParams = new object[] { httpWebResponse.ResponseUri, headerKey, setCookie, false };
                    var cookieCollection = (CookieCollection)cookieCutterFunInfo.Invoke(cookieContainer, invokeParams);
                    _cookies.Add(cookieCollection);
                }
                catch
                {
                    // ignored
                }
            }
            
        }

        private readonly CookieCollection _cookies = new CookieCollection();
    }
}