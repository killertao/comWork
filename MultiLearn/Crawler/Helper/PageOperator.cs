﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crawler.Helper
{

    class PageOperator
    {

        private const string validate = "http://wenshu.court.gov.cn/Html_Pages/VisitRemind.html";
        private WebBrowser webBrowser { get; set; }


        public PageOperator(WebBrowser wb)
        {
            webBrowser = wb;
        }

        //将页面切换到每页20条
        public void SwitchPageSize()
        {
            var s = webBrowser.Document.Body.GetElementsByTagName("input").Cast<HtmlElement>()
                .First(ele => ele.GetAttribute("value") == "5");
            s.InvokeMember("click");
            Thread.Sleep(200);
            var option = webBrowser.Document.Body.GetElementsByTagName("li").Cast<HtmlElement>()
                .First(ele => ele.InnerText == "20" && ele.GetAttribute("id").Contains("input_20"));
            option.InvokeMember("click");
        }

        //点击下一页   
        public bool NextPage()
        {
            var elements = webBrowser.Document.Body.GetElementsByTagName("a").Cast<HtmlElement>();
            var nextBtn = elements.FirstOrDefault(ele => ele.InnerHtml == "下一页");
            if (nextBtn == null)
            {
                return false;
            }
            else
            {
                nextBtn.InvokeMember("click");
                return true;
            }



        }

        //获取当前文章总数
        public int GetSumDoc()
        {
           
              return Int32.Parse(webBrowser.Document.GetElementById("span_datacount").InnerText);
             
       
        }

        public List<String> GetDocIds()
        {
            Regex reg=new Regex(@"/content/content\?DocID=[\w\d-]+");
            var mathes = reg.Matches(webBrowser.Document.Body.InnerHtml);
            List<string> listDocId=new List<string>();
            foreach (var item in mathes)
            {
                   var match= item as Match;
                   listDocId.Add(match.Value.Replace("/content/content?DocID=",""));
            }
            return listDocId;
        }

        public List<TypeContent> GetTypeByWord(keyType keyType)
        {
          
            string s = "按" + keyType + "筛选";
            var key = webBrowser.Document.GetElementsByTagName("span").Cast<HtmlElement>()
                .FirstOrDefault(ele => ele.InnerText == "按关键词筛选");
            var ul = key.Parent.NextSibling.FirstChild;
            var childs = ul.Children.Cast<HtmlElement>().Where(ele => ele.TagName == "LI");
            List<TypeContent> list = new List<TypeContent>();
            foreach (var li in childs)
            {
                string[] arrcontent = li.InnerText.Split('(', ')');
                var tc = new TypeContent()
                {
                    count = Int32.Parse(arrcontent[1]),
                    content = arrcontent[0],
                    keytype = keyType
                };
                list.Add(tc);
            }
            return new List<TypeContent>();
        }

        public class TypeContent
        {
            //关键字
            public string content { get; set; }

            //有多少条
            public int count { get; set; }

            public keyType keytype { get; set; }

        }


        public enum keyType
        {
            关键词,
            一级案由,
            法院层级,
            审判程序,
            文书类型,
            裁判年份,
        }

        //判断是否是验证码页面
        public bool IsValidatePage()
        {
            return webBrowser.Document.Body.InnerHtml.Contains("您的访问频次超出正常访问范围，为保障网站稳定运行，请输入验证码后继续查看！");
        }

        //填写验证码验证  并且返回原来的页面
        public void Validate()
        { //这里唯一的问题就是 页面验证完成之后页面不一定跳到之前的页面   展示不管这个） 还要控制所有的id不能重复加入。。。。

        }
        public  void  









    }
}