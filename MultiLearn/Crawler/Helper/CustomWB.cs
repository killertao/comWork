using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crawler.Helper
{
    public  class CustomWB:WebBrowser
    {


        public bool IsLoadSccees { get; set; } = false;
        public Action CallBack = null;
        public CustomWB()
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


       
    }
}
