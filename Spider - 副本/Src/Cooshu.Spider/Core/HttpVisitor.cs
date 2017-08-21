using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Windows.Forms;
using CaptchaExtractor;

namespace Cooshu.Spider.Core
{
    public class HttpVisitor
    {
        public HttpVisitor(string url, HttpCookieContainer cookieContainer, Encoding encoding=null, Dictionary<string, string> header = null)
        {
            if (cookieContainer == null)
            {
                throw new NullReferenceException("cookieContainer不能为null");
            }

            Url = url;
            RequestEncoding = encoding?? Encoding.Default;
            Header = header??new Dictionary<string, string>();
            _cookieContainer = cookieContainer;
        }

        public ResponseData GetString(Action<WebRequest> setWebRequest=null)
        {
            var result = new ResponseData();
            GetStream(
                request =>
                {
                    SetHeader((HttpWebRequest) request);
                    setWebRequest?.Invoke(request);
                },
                (webResponse, responseStream) =>
                {
                    //获得响应内容
                    using (var resultStream = new StreamReader(responseStream, RequestEncoding))
                    {
                        result.HtmlText = resultStream.ReadToEnd();
                        result.ResponseUrl = webResponse.ResponseUri.ToString();
                    }
                });
            
            if (string.IsNullOrWhiteSpace(result.HtmlText))
            {
                throw new Exception("服务器没有返回任何数据！");
            }

            return result;
        }

        public ResponseData GetString(Dictionary<string,string> postData)
        {
            var result = new ResponseData();
            var postString = postData == null ? "" : postData.Aggregate("", (current, item) => current + $"&{item.Key}={HttpUtility.UrlEncode(item.Value)}").Substring(1);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "post";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/x-www-form-urlencoded";//"&xiaoli_id=02,0201,0202,0203,0204,0205,0206&
            string postStr = postData == null ? "" : postData.Aggregate("", (current, item) => current + $"&{item.Key}={HttpUtility.UrlEncode(item.Value)}").Substring(1);
            var sss = "searchtype=0&lib=zyfl&chooseNum=010101&firstPage=1&secondPage=1&thirdPage=1&fourthPage=1&fifthPage=1&sixthPage=1&listnum=10";
            byte[] buffer = Encoding.Default.GetBytes(postStr);


            // byte[] buffer = Encoding.Default.GetBytes("searchtype=0&lib=zyfl&chooseNum=010101&firstPage=1&secondPage=1&thirdPage=1&fourthPage=1&fifthPage=1&sixthPage=1&listnum=10");
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                result.HtmlText = reader.ReadToEnd();
            }





            //GetStream(
            //    request =>
            //    {
            //        // SetHeader((HttpWebRequest)request);

            //        request.Method = "POST";
            //        request.ContentLength = postString.Length;
            //        var writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
            //        writer.Write(postString);
            //        writer.Flush();

            //    },
            //    (webResponse, responseStream) =>
            //    {
            //        //获得响应内容
            //        using (var resultStream = new StreamReader(responseStream, RequestEncoding))
            //        {
            //            result.HtmlText = resultStream.ReadToEnd();
            //            result.ResponseUrl = webResponse.ResponseUri.ToString();
            //        }
            //    });

            //if (string.IsNullOrWhiteSpace(result.HtmlText))
            //{
            //    throw new Exception("服务器没有返回任何数据！");
            //}

            return result;
        }

        public ResponseData GetString(string postJson)
        {
            var result = new ResponseData();
            GetStream(
                request =>
                {
                    SetHeader((HttpWebRequest)request);

                    request.Method = "POST";
                    var writer = new StreamWriter(request.GetRequestStream());
                    var a = RequestEncoding.GetBytes(postJson??"");
                    writer.Write(RequestEncoding.GetChars(a));
                    writer.Flush();

                },
                (webResponse, responseStream) =>
                {
                    //获得响应内容
                    using (var resultStream = new StreamReader(responseStream, RequestEncoding))
                    {
                        result.HtmlText = resultStream.ReadToEnd();
                        result.ResponseUrl = webResponse.ResponseUri.ToString();
                    }
                });

            if (string.IsNullOrWhiteSpace(result.HtmlText))
            {
                throw new Exception("服务器没有返回任何数据！");
            }

            return result;
        }

        public Bitmap GetBitmap(Action<WebRequest> setWebRequest = null)
        {
            Bitmap bitmap = null;
            GetStream(setWebRequest, (webResponse, responseStream) =>
            {
                //获得响应内容
                using (var memoryStream = new MemoryStream())
                {
                    var buffSize = 1024;
                    var buff = new byte[buffSize];
                    var readCount = responseStream.Read(buff, 0, buffSize);
                    while (readCount > 0)
                    {
                        memoryStream.Write(buff, 0, readCount);
                        readCount = responseStream.Read(buff, 0, buffSize);
                    }

                    memoryStream.Position = 0;
                    bitmap = new Bitmap(memoryStream);
                }
            });

            return bitmap;
        }

        public string ReadVerificationCode(Action<WebRequest> setWebRequest = null)
        {
            var bitmap = GetBitmap(setWebRequest);
            var ColorOffset = 5;
            var SplitColorNum = 150;

            return Extractor.Run(bitmap);
        }

        private void GetStream(Action<WebRequest> setWebRequest, Action<WebResponse,Stream> successProcess )
        {
            //创建http请求对象
            var webRequestor = WebRequest.Create(Url);
            webRequestor.Proxy = null;
            webRequestor.Timeout = Timeout;
           // webRequestor.Headers.Add("Cookie", _cookieContainer.GetHeader());

            setWebRequest?.Invoke(webRequestor);

            //web异常时,继续获得响应流
            WebResponse response;
            try
            {
                response = webRequestor.GetResponse();
                lock (UserAgent)
                {
                    if (!UserAgent.Contains(webRequestor.Headers["User-Agent"]))
                    {
                        UserAgent.Add(webRequestor.Headers["User-Agent"]);
                    }
                }
            }
            catch (WebException webError)
            {
                response = webError.Response;

                if (webError.Status == WebExceptionStatus.ProtocolError)
                {
                    lock (ErrorUserAgent)
                    {
                        if (!ErrorUserAgent.Contains(webRequestor.Headers["User-Agent"]))
                        {
                            ErrorUserAgent.Add(webRequestor.Headers["User-Agent"]);
                        }
                    }
                }

                if (response == null)
                {
                    throw new Exception("无法获得错误的响应内容!");
                }
            }

            //获得响应对象
            var httpWebResponse = response as HttpWebResponse;
            using (var responseStream = response.GetResponseStream())
            {
                if (httpWebResponse == null || responseStream == null)
                {
                    throw new Exception("服务器无响应或超时");
                }

                //服务响应报错直接返回报错内容
                var statusCode = (int)httpWebResponse.StatusCode;
                if (statusCode < 200 || statusCode >= 400)
                {
                    throw new Exception(httpWebResponse.StatusDescription);
                }
                
                //获得响应内容
                responseStream.ReadTimeout = Timeout;
                successProcess?.Invoke(response, responseStream);
            }

            //获得响应Cookie
            _cookieContainer.Update(httpWebResponse);
        }

        private void SetHeader(HttpWebRequest request)
        {
            request.ContentType = Header.ContainsKey("Content-Type") ? Header["Content-Type"] : "application/x-www-form-urlencoded";
            
            lock (ErrorUserAgent)
            {
                request.UserAgent = GetAgent(20);
            }

            if (Header.ContainsKey("Host"))
            {
                request.Host = Header["Host"];
            }
            if (Header.ContainsKey("Connection"))
            {
                request.Connection = Header["Connection"];
            }
            else
            {
                //request.Connection ="keep-alive";
            }
            if (Header.ContainsKey("Accept-Encoding"))
            {
                request.Accept = Header["Accept-Encoding"];
            }
            if (Header.ContainsKey("Content-Length"))
            {
                request.ContentLength = int.Parse(Header["Content-Length"]);
            }

            request.Accept = "Accept:text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            request.Headers.Add("Cache-Control", "no-cache");
            request.Headers.Add("Pragma", "no-cache");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
           // request.Host = "wenshu.court.gov.cn";
        }

        private string GetAgent(int times)
        {
            var ran = new Random();
            var browsers = new List<string>
            {
                $"Mozilla/{ran.Next(4, 6)}.0 (Macintosh; U; Intel Mac OS X 10_6_8; en-us) AppleWebKit/534.50 (KHTML, like Gecko) Version/5.1 Safari/534.50{ran.Next(100, 9999)}",
                $"Mozilla/{ran.Next(4, 6)}.0 (Windows; U; Windows NT {ran.Next(5, 7)}.1; en-us) AppleWebKit/534.50 (KHTML, like Gecko) Version/5.1 Safari/534.50.{ran.Next(100, 9999)}",
                $"Mozilla/{ran.Next(4, 6)}.0 (compatible; MSIE {ran.Next(7, 12)}.0; Windows NT {ran.Next(5, 7)}.1; Trident/5.0)",
                $"Mozilla/{ran.Next(4, 6)}.0 (compatible; MSIE {ran.Next(7, 12)}.0; Windows NT {ran.Next(5, 7)}.1; Trident/4.0)",
                $"Mozilla/{ran.Next(4, 6)}.0 (compatible; MSIE {ran.Next(7, 12)}.0; Windows NT {ran.Next(5, 7)}.1)",
                $"Mozilla/{ran.Next(4, 6)}.0 (compatible; MSIE {ran.Next(7, 12)}.0; Windows NT {ran.Next(5, 7)}.1)",
                $"Mozilla/{ran.Next(4, 6)}.0 (Macintosh; Intel Mac OS X 10.6; rv:2.0.1) Gecko/20100101 Firefox/4.0.{ran.Next(1, 999)}",
                $"Mozilla/{ran.Next(4, 6)}.0 (Windows NT 6.1; rv:2.0.1) Gecko/20100101 Firefox/4.0.{ran.Next(1, 999)}",
                $"Opera/9.80 (Macintosh; Intel Mac OS X {ran.Next(9, 11)}.{ran.Next(1, 10)}.8; U; en) Presto/2.8.{ran.Next(100, 9999)} Version/11.11",
                $"Opera/9.80 (Windows NT 6.1; U; en) Presto/2.8.131 Version/11.11.{ran.Next(100, 9999)}",
                $"Mozilla/{ran.Next(4, 6)}.0 (Macintosh; Intel Mac OS X 10_7_0) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.56 Safari/535.11.{ran.Next(100, 9999)}",
                $"Mozilla/{ran.Next(4, 6)}.0 (compatible; MSIE {ran.Next(7, 12)}.0; Windows NT {ran.Next(5, 7)}.1; Maxthon 2.0)",
                $"Mozilla/{ran.Next(4, 6)}.0 (compatible; MSIE {ran.Next(7, 12)}.0; Windows NT {ran.Next(5, 7)}.1; TencentTraveler 4.0)",
                $"Mozilla/{ran.Next(4, 6)}.0 (compatible; MSIE {ran.Next(7, 12)}.0; Windows NT {ran.Next(5, 7)}.1)",
                $"Mozilla/{ran.Next(4, 6)}.0 (compatible; MSIE {ran.Next(7, 12)}.0; Windows NT {ran.Next(5, 7)}.1; The World)",
                $"Mozilla/{ran.Next(4, 6)}.0 (compatible; MSIE {ran.Next(7, 12)}.0; Windows NT {ran.Next(5, 7)}.1; Trident/4.0; SE 2.X MetaSr 1.0; SE 2.X MetaSr 1.0; .NET CLR 2.0.50727; SE 2.X MetaSr 1.0)",
                $"Mozilla/{ran.Next(4, 6)}.0 (compatible; MSIE {ran.Next(7, 12)}.0; Windows NT {ran.Next(5, 7)}.1; 360SE)",
                $"Mozilla/{ran.Next(4, 6)}.0 (compatible; MSIE {ran.Next(7, 12)}.0; Windows NT {ran.Next(5, 7)}.1 Avant Browser)",
                $"Mozilla/{ran.Next(4, 6)}.0 (compatible; MSIE {ran.Next(7, 12)}.0; Windows NT {ran.Next(5, 7)}.1)"
            };

            var result = browsers[ran.Next(0, browsers.Count - 1)];

            if (ErrorUserAgent.Contains(result) && times > 1)
            {
                return GetAgent(times-1);
            }

            return result;
        }

        /// <summary>
        /// 超时时间 (单位：毫秒)
        /// </summary>
        public int Timeout { get; set; } = int.Parse(ConfigurationManager.AppSettings["WebVisitorTimeout"]);

        public Encoding RequestEncoding;

        public string Url;

        private HttpCookieContainer _cookieContainer;

        private Dictionary<string, string> Header { get; set; }

        private static readonly List<string> ErrorUserAgent = new List<string>();


        private static readonly List<string> UserAgent = new List<string>();
    }

    public class ResponseData
    {
        /// <summary>
        /// 正文
        /// </summary>
        public string HtmlText { get; set; }

        /// <summary>
        /// 响应Url，如果发生跳转，响应Url与请求URL会不一致
        /// </summary>
        public string ResponseUrl { get; set; }
    }
    
}