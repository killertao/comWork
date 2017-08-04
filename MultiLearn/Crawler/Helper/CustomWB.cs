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
            
            this.DocumentCompleted += (sender, source) =>
            {
                var htmlDocument = this.Document;
                if (htmlDocument != null)
                {
                    var arrjs = htmlDocument.GetElementsByTagName("script").Cast<HtmlElement>();
                    if (arrjs.Count() > 14)
                    {
                        Thread.Sleep(500);
                        CallBack?.Invoke();
                        IsLoadSccees = true;
                    }
                }
            };
        }

       
    }
}
