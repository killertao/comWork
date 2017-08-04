using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Crawler.Helper;

namespace Crawler
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        public Dictionary<string, FormChild> webForm = new Dictionary<string, FormChild>();
        public void AddWebForm(string url)
        {

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(AddWebForm), url);
            }
            else
            {
                FormChild f2 = new FormChild(url);
                f2.Show(panel2);
            }
        }

        private void btnSpiderSta_Click(object sender, EventArgs e)
        {
            //打开一个页面
            string url = BaseUrl(LogicalDB.GetDate());
            AddWebForm(url);
        }




        private string BaseUrl(DateTime date)
        {
            string datesta = date.ToString("yyyy-MM-dd");
            string dateend = date.AddDays(1).ToString("yyyy-MM-dd");
            string url = string.Format($"http://wenshu.court.gov.cn/list/list/?sorttype=1&conditions=searchWord+++{datesta}%20TO%20{dateend}+上传日期:{datesta}%20TO%20{dateend}");
            return url;
        }

     
    }
}


