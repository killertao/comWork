using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Crawler.Helper;
using mshtml;
using WeifenLuo.WinFormsUI.Docking;
namespace Crawler
{
    [ComVisible(true)]
    public partial class FormChild:IDisposable //: DockContent
    {
        public PageOperator Po;
        private bool isLoad = true;
        private string url;

        public FormChild(string url)
        {
            this.url = url;
            InitializeComponent();
            webBrowser.Navigate(new Uri(url));
            webBrowser.Navigated += Navigated;
            Po = new PageOperator(webBrowser);
            webBrowser.ObjectForScripting = this;
           // base.OnLoad(e);

        }

        //protected override void OnLoad(EventArgs e)
        //{
           
        //}

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





        private void Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            var htmlDocument = webBrowser.Document;
            var arrjs = htmlDocument.GetElementsByTagName("script").Cast<HtmlElement>();
        }

        private void webBrowser_FileDownload(object sender, EventArgs e)
        {
            var s = 1;
        }

        public void Test()
        {
            var s = 1;
        }

        public void Dispose()
        {

        }
    }










}
