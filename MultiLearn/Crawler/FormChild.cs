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
        private string url;

        public FormChild(string url)
        {
            this.url = url;
            InitializeComponent();
            webBrowser.Navigate(new Uri(url));
            Po = new PageOperator(webBrowser);
            webBrowser.ObjectForScripting = this;
        }



        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
        
            var htmlDocument = webBrowser.Document;
            var arrjs = htmlDocument.GetElementsByTagName("script").Cast<HtmlElement>();
            if (arrjs.Count() > 14)
            {
                HtmlElement script = webBrowser.Document.CreateElement("script");
                script.SetAttribute("type", "text/javascript");
                string str = File.ReadAllText(Application.StartupPath + "/exten.js");
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
    }










}
