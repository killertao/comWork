using System;
using System.Linq;
using System.Security.Policy;
using System.Threading;
using System.Windows.Forms;
using Crawler.Helper;
using WeifenLuo.WinFormsUI.Docking;
namespace Crawler
{
    public partial class FormChild : DockContent
    {
        private PageOperator po;

        public FormChild(string url)
        {
            InitializeComponent();

            webBrowser.Navigate(new Uri(url));
            //webBrowser.Navigated += Navigated;
            po = new PageOperator(webBrowser);
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            var htmlDocument = webBrowser.Document;
            if (htmlDocument != null)
            {
                var arrjs = htmlDocument.GetElementsByTagName("script").Cast<HtmlElement>();
                if (arrjs.Count() > 14)
                {

                    po.SwitchPageSize();
                }
              //  po.SwitchPageSize();
            }
        }




        private void Delay(int Millisecond) //延迟系统时间，但系统又能同时能执行其它任务；  
        {
            DateTime current = DateTime.Now;
            while (current.AddMilliseconds(Millisecond) > DateTime.Now)
            {
                Application.DoEvents(); //转让控制权              
            }

        }

        private bool WaitWebPageLoad()
        {
            int i = 0;
            string sUrl;
            while (true)
            {
                Delay(50); //系统延迟50毫秒，够少了吧！               
                if (webBrowser.ReadyState == WebBrowserReadyState.Complete) //先判断是否发生完成事件。  
                {
                    if (!webBrowser.IsBusy) //再判断是浏览器是否繁忙                    
                    {
                        i = i + 1;
                        if (i == 2 ) //为什么 是2呢？因为每次加载frame完成时就会置IsBusy为false,未完成就就置IsBusy为false，你想一想，加载一次，然后再一次，再一次...... 最后一次.......  
                        {
                            sUrl = webBrowser.Url.ToString();
                            if (sUrl.Contains("res")) //这是判断没有网络的情况下                             
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        continue;
                    }
                    i = 0;
                }
            }
        }

    }

}
