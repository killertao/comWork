using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Crawler.Helper;

namespace Crawler
{
    [ComVisible(true)]
    public partial class FormChild : Form
    {
        public PageOperator Po;
        private bool isLoad = true;
        private string oldUrl;
        public FormChild(string url)
        {
       
            InitializeComponent();
            webBrowser.Navigate(new Uri(url));
            Po = new PageOperator(webBrowser);
            oldUrl = url;
            webBrowser.ObjectForScripting = this;
        }



        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            string title = webBrowser.Document.Title;
            if (string.IsNullOrEmpty(title))
            {//表示dom还没有加载完成
                return;
            }
            string jsroot = "";
            if (title.Contains("导航已取消"))
            {
                webBrowser.Navigate(new Uri(oldUrl));
                return;
                ;
            }
            if (title.Contains("访问验证"))
            {//加入验证的js 然后回到后台填写验证吗

            }
            else if (title.Contains("首页 - 中国裁判文书网"))
            {

            }
            else if (title.Contains("列表页 - 中国裁判文书网"))
            {
                jsroot = "/exten.js";

            }
            if (!string.IsNullOrEmpty(jsroot))
            {
                HtmlElement script = webBrowser.Document.CreateElement("script");
                script.SetAttribute("type", "text/javascript");
                string str = File.ReadAllText(Application.StartupPath + jsroot);
                script.SetAttribute("text", str);
                webBrowser.Document.Body.AppendChild(script);
            }

        }


        public void AddDocId(string text)
        {
            Regex reg = new Regex("\\\"文书ID[^a-z]{5}([a-z0-9-]{36})");
            var matches= reg.Matches(text);
            List<string> docids=new List<string>();
            foreach (var item in matches)
            {
                var match = item as Match;
                docids.Add(match.Groups[1].Value);
            }
            //加入到数据库todo

        }

        public void Condition(bool isCondition)
        {
            if (!isCondition)
            {
                webBrowser.Navigate(new Uri(oldUrl));
                return;

            }
        }
    }
}
