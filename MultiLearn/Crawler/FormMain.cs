using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Crawler.Helper;
using  Crawler.Model;
namespace Crawler
{
    public partial class FormMain : Form
    {
        //private Dictionary<string, FormChild> childForm = new Dictionary<string, FormChild>(); //存储所有的子页面
        //private Dictionary<string, FormAssgin> assginForm = new Dictionary<string, FormAssgin>(); //存储所有的任务分配页面
        private Dictionary<string, Form> webForm = new Dictionary<string, Form>();
        public FormMain()
        {
            InitializeComponent();
        }

        //开始抓取
        private void btnSpiderSta_Click(object sender, EventArgs e)
        {
            //这里要考虑到第二次进来的时候重新分配为位完成的任务
            
            string url = BaseUrl(LogicalDB.GetDate());
            var fromassign=new FormAssgin(url);
            //查看数据库是否有残留任务 
            var  assgins=LogicalDB.GetPreAssgin();
            if (assgins.Count >0)
            {
                fromassign.TypeContents = assgins;
            }
            //处理是否有子任务残留
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    
                   var count= webForm.Count(a => a.Value.GetType() == typeof(FormChild));
                    if (count < 1000)
                    {
                        var childs = LogicalDB.GetPreChild(1000 - count);
                        foreach (var item in childs)
                        {
                            AddWebForm(new FormChild(item.Url), item.Url);
                        }

                    }
                    Thread.Sleep(1000 * 60);
                }

            });
            AddWebForm(fromassign, url);
        }

        //关闭之前，将未完成的任务和子任务存到数据库
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
             Thread th=new Thread(a =>
             {
                 var wf = a as Dictionary<string, Form>;
                 foreach (var item in wf)
                 {
                     string url = item.Key;
                     if (item.Value.GetType() == typeof(FormChild))
                     {
                         FormChild child = item.Value as FormChild;
                          //将url 和页数存储到数库
                         int PageSize = child.PageIndex;

                         //todo
                     }
                     else
                     {
                         FormAssgin assgin = item.Value as FormAssgin;
                         var s = assgin.TypeContents;
                         
                         //将url 和对应的所有未完成的content存到数据库，下次虚拟化一个assign出来
                     }
                 }

             });
            th.IsBackground = false;//使用前台线程
            th.Start();
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
          //  AddWebForm<FormChild>("http://www.baidu.com");
        }
        public void AddWebForm<T>(T fm,string url)
        {

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<T,string>(AddWebForm), fm,url);
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

        public void RemoveWebForm<T>(string url)
        {
            tabControl1.TabPages.RemoveByKey(url);
            webForm.Remove(url);
        }

        private void FormShow(Form f2, string url)
        {
            f2.MdiParent = this;
            tabControl1.TabPages.Add(url, "页面" + webForm.Count);
            tabControl1.TabPages[webForm.Count].Dock = DockStyle.Fill;
            f2.Parent = tabControl1.TabPages[webForm.Count];
            f2.Show();
            webForm.Add(url,f2);
           

        }
        ///


        private string BaseUrl(DateTime date)
        {

            string datesta = date.ToString("yyyy-MM-dd");
            string dateend = date.AddDays(1).ToString("yyyy-MM-dd");
            //todo 在这里需要拼接一个验证码 number 和guid进去，页面返回才有结果
            string url = string.Format($"http://wenshu.court.gov.cn/list/list/?sorttype=1&conditions=searchWord+++{datesta}%20TO%20{dateend}+上传日期:{datesta}%20TO%20{dateend}");
            return url;
        }

   
    }
}


