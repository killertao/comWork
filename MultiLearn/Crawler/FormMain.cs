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

        public Dictionary<string, Form> webForm = new Dictionary<string, Form>(); //存储所有的子页面
        public void AddWebForm<T>(string url)
        {

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(AddWebForm<T>), url);
            }
            else
            {
                if (typeof(T) == typeof(FormChild))
                {
                    FormChild f2 = new FormChild(url);
                    FormShow(f2, url);
                }
                else
                {
                    FormAssgin f2 = new FormAssgin(url);
                    FormShow(f2, url);
                }
            }
        }

        private void FormShow(Form f2, string url)
        {
            f2.MdiParent = this;
            tabControl1.TabPages.Add(url, "页面" + webForm.Count);
            tabControl1.TabPages[webForm.Count].Dock = DockStyle.Fill;
            f2.Parent = tabControl1.TabPages[webForm.Count];
            f2.Show();
            if (f2.GetType() == typeof(FormChild))
            {
                webForm.Add(url, f2);
            }

        }

        //开始抓取
        private void btnSpiderSta_Click(object sender, EventArgs e)
        {
            string url = BaseUrl(LogicalDB.GetDate());
            AddWebForm<FormChild>(url);
        }




        private string BaseUrl(DateTime date)
        {

            string datesta = date.ToString("yyyy-MM-dd");
            string dateend = date.AddDays(1).ToString("yyyy-MM-dd");
            //http://wenshu.court.gov.cn/List/List?sorttype=1&conditions=searchWord+++2017-08-10%20TO%202017-08-11+上传日期:2017-08-10%20TO%202017-08-11&conditions=searchWord+2+AJLX++案件类型:刑事案
            string url = string.Format($"http://wenshu.court.gov.cn/list/list/?sorttype=1&conditions=searchWord+++{datesta}%20TO%20{dateend}+上传日期:{datesta}%20TO%20{dateend}");
            return url;
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            AddWebForm<FormChild>("http://www.baidu.com");
        }

        ///
        private string CreateGuid()
        {
            //function createGuid()
            //{
            //    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
            //}
            string str = "";
            Random rd = new Random();
            for (int i = 0; i < 4; i++)
            {
                double s =(1+(rd.Next(10000000))/10000000.0)*0x10000;
                str +=((int)Math.Round(s)).ToString("X").Substring(1)+"-";
            }
            return str.TrimEnd('-');
        }
    }
}


