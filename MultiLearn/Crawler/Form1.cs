using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using Crawler.Helper;

namespace Crawler
{
    public partial class Form1 : Form
    {
        private HtmlElement body;
        public Form1()
        {
            InitializeComponent();
            webBrowser1.Url =
                new Uri(@"http://wenshu.court.gov.cn/list/list/?sorttype=1&conditions=searchWord+++2017-07-31%20TO%202017-08-01+%E4%B8%8A%E4%BC%A0%E6%97%A5%E6%9C%9F:2017-07-31%20TO%202017-08-01&conditions=searchWord+%E5%88%91%E4%BA%8B%E6%A1%88%E7%94%B1+++%E4%B8%80%E7%BA%A7%E6%A1%88%E7%94%B1:%E5%88%91%E4%BA%8B%E6%A1%88%E7%94%B1&conditions=searchWord+%E9%9D%9E%E6%B3%95%E5%8D%A0%E6%9C%89+++%E5%85%B3%E9%94%AE%E8%AF%8D:%E9%9D%9E%E6%B3%95%E5%8D%A0%E6%9C%89");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            TaskPool tp = new TaskPool();
            var at = new AssignTask(tp);
            at.Begin();
            tp.EndReStart();

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            body = webBrowser1.Document.Body;
            // webBrowser1.DocumentStream.Flush();

            var arrjs = webBrowser1.Document.GetElementsByTagName("script").Cast<HtmlElement>();

            var listhtm = arrjs.FirstOrDefault(ele => ele.GetAttribute("src") == "/Assets/js/Lawyee.CPWSW.List.js");


            if (listhtm != null)
            {
                // Thread.Sleep(10000);
                //  webBrowser1.DocumentStream.Flush();
                //string html = webBrowser1.DocumentText;
                //string script = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Lawyee.CPWSW.List.js");
                //html = html.Replace("<script src=\"/Assets/js/Lawyee.CPWSW.List.js\" type=\"text/javascript\"></script>",  "<script>"+ script + "</script>");
                //webBrowser1.DocumentText = html;
            }
        }


        private void webBrowser1_FileDownload(object sender, EventArgs e)
        {
            var s = 1;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            var elements = body.GetElementsByTagName("a").Cast<HtmlElement>();
            var nextBtn = elements.FirstOrDefault(ele => ele.InnerHtml == "下一页");
            if (nextBtn == null)
            {
                MessageBox.Show("没有下一页了");
                return;
            }
            nextBtn.InvokeMember("click");
        }


        //调成每页20条数据
        private void btnPageSize_Click(object sender, EventArgs e)
        {
            var s = body.GetElementsByTagName("input").Cast<HtmlElement>()
                .First(ele => ele.GetAttribute("value") == "5");
            s.InvokeMember("click");
            Thread.Sleep(200);
            var option = body.GetElementsByTagName("li").Cast<HtmlElement>()
                .First(ele => ele.InnerText == "20" && ele.GetAttribute("id").Contains("input_20"));
            option.InvokeMember("click");

        }

        //获取分类总数据
        private void btnGetCount_Click(object sender, EventArgs e)
        {

            MessageBox.Show(GetCount());
        }


        private string GetCount()
        {
            return webBrowser1.Document.GetElementById("span_datacount").InnerText;
        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri(txtAddress.Text);
        }

        private void btnGetUrl_Click(object sender, EventArgs e)
        {
            MessageBox.Show(webBrowser1.Url.AbsoluteUri);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CustomWB xCustomWb = new CustomWB();
            xCustomWb.CallBack = () =>
            {
                MessageBox.Show("加载腕表");
            };
            xCustomWb.Url = new Uri("http://wenshu.court.gov.cn/List/List?sorttype=1&conditions=searchWord%201%20AJLX%20%20%E6%A1%88%E4%BB%B6%E7%B1%BB%E5%9E%8B%3A%E5%88%91%E4%BA%8B%E6%A1%88%E4%BB%B6");
            




        }

        private void button2_Click(object sender, EventArgs e)
        {
            Regex reg = new Regex(@"/content/content\?DocID=[\w\d-]+", RegexOptions.Multiline);
            var s = reg.Matches(webBrowser1.Document.Body.InnerHtml);

        }
    }
}
