using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using Cooshu.Spider.Core;
using Cooshu.Spider.WebFrame;

namespace Cooshu.Spider
{
    public partial class WinMain : Form
    {
        public WinMain()
        {
            InitializeComponent();
        }
        
        private void BtnStartClick(object sender, EventArgs e)
        {
            if (BtnStart.Text == "开始" || BtnStart.Text == "继续")
            {
                //new ZgcpwswFrame().Start();
                BtnStart.Text = "暂停";
                BtnStop.Show();
            }
            else
            {
                //SpiderTaskScheduler.Instance.Pause();
                BtnStart.Text = "继续";
                BtnStop.Hide();
            }
        }

        private void BtnStopClick(object sender, EventArgs e)
        {


            //long t = Convert.ToInt64(0.1);
            //SpiderTaskScheduler.Instance.Stop();
            //new ZgcpwswFrame().Stop();

            //BtnStop.Hide();
            //BtnStart.Text = "开始";

            //var httpvisit = new HttpVisitor("http://vpnlib.imu.edu.cn/rwt/HE5YZ4DVPSZDVL3QP75YPLURNN4XZZLYF3SXP/", Encoding.Default,cookie: "FWinCookie=1; clientId=CID2b0b81b3f6eb49d0697a9c9e7cd56c11; Hm_lvt_58c470ff9657d300e66c7f33590e53a8=1444836624,1444836765,1444836969,1444840134; bdyh_record=121744510%2C121862410%2C121138248%2C121495045%2C121239998%2C121862409%2C120636635%2C121974146%2C120801216%2C121974147%2C121004952%2C120484452%2C120550282%2C120268302%2C121240101%2C121428117%2C119574865%2C119873879%2C121428530%2C121428034%2C; JSESSIONID=5C9DC71086FE441AF9779FC663E9B3B6; Hm_lpvt_58c470ff9657d300e66c7f33590e53a8=1444840134");
            //var result = httpvisit.GetString(a =>
            //{
            //    var httpRequest = a as HttpWebRequest;
            //    httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
            //    httpRequest.Referer = "http://www.seek68.cn/openform.asp?a=84623.02&c=0asme09";
            //});
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void WinMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            closed = true;
            //SpiderTaskScheduler.Instance.Stop();
            //new ZgcpwswFrame().Stop();
        }

        private void WinMain_Load(object sender, EventArgs e)
        {
            Instance = this;

            CheckForIllegalCrossThreadCalls = false;
            //DgvWaiting.AutoGenerateColumns = true;
            var column1 = new DataGridViewColumn
            {
                DataPropertyName = "Url",
                HeaderText = "性别",
                Name = "column1"
            };

            DgvSites.AutoGenerateColumns = false;
            DgvSites.DataSource = Schedulers;
            Schedulers.Add(new SchedulerInfo {SiteName = "中国裁判文书网", ThreadCount = 1, SiteFrame = new ZgcpwswFrame(), State = "开始"});
            //Schedulers.Add(new SchedulerInfo { SiteName = "北大法易", ThreadCount = 1, SiteFrame = new BdfyFrame(), State = "开始" });
            Schedulers.Add(new SchedulerInfo {SiteName = "北大法宝", ThreadCount = 1, SiteFrame = new BdfbFrame(), State = "开始" });
            Schedulers.Add(new SchedulerInfo { SiteName = "多线程测试", ThreadCount = 1, SiteFrame = new TreadTestFrame(), State = "开始" });

            //DgvWaiting.Columns.Add(column1);
            //DgvWaiting.DataSource = SpiderTaskScheduler.Instance.SpiderTaskQueue;
            //SpiderTaskScheduler.Instance.SpiderTaskQueue.Add(new SpiderTask { Page = new Page { Name = "aaa" }, Url = "ccc" });



            //SpiderTaskScheduler.Instance.SpiderTaskQueue.Add(new SpiderTask {Url = "d"});
            //DgvWaiting.clear;
            //DgvWaiting.ResetBindings();
            //DgvWaiting.DataMember
            //var a = new BindingList<SpiderTask>(SpiderTaskScheduler.Instance.SpiderTaskQueue);

        }


        public static void WriteLog(string data)
        {
            if (closed)
            {
                return;
            }

            var maxLengthForLog = 40960;
            lock (_locker)
            {
                Instance.TbxLog.Text = data + Instance.TbxLog.Text;
                if (Instance.TbxLog.Text.Length > maxLengthForLog)
                {
                    var indexForEnd = Instance.TbxLog.Text.IndexOf("\r\n", maxLengthForLog);
                    if (indexForEnd > -1)
                    {
                        Instance.TbxLog.Text = Instance.TbxLog.Text.Substring(0, indexForEnd);
                    }
                }
            }
        }

        public void RefresScheduleInfo()
        {
            lock (_locker)
            {
                foreach (var schedulerInfo in Schedulers)
                {
                    schedulerInfo.Error = schedulerInfo.SiteFrame.SchedulerInstance._errorTaskQueue.Count;
                    schedulerInfo.Waiting = schedulerInfo.SiteFrame.SchedulerInstance._spiderTaskStack.Count;
                }
                DgvSites.Refresh();
            }
        }
        
        private static bool closed;

        /// <summary>
        /// 实例
        /// </summary>
        public static WinMain Instance { get; private set; }


        //单元格点击事件
        private void DgvSites_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //处理开始事件
            //开始
            if (DgvSites.Columns[e.ColumnIndex].Name == "Start")
            {
                var schedulerInfo = ((SchedulerInfo) DgvSites.Rows[e.RowIndex].DataBoundItem);//获取行数据对象  
                
                switch (DgvSites.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                {
                    case "开始":
                    {
                        var threadCount = schedulerInfo.ThreadCount;
                        schedulerInfo.SiteFrame.Start(threadCount);  //页面对象里的开始方法
                        schedulerInfo.State = "暂停";
                    }
                        break;
                    case "继续":
                    {
                        var threadCount = schedulerInfo.ThreadCount;
                      //  schedulerInfo.SiteFrame.Continue(threadCount);//页面对对象里面的暂停
                        schedulerInfo.State = "暂停";
                    }
                        break;
                    default:
                       //  schedulerInfo.SiteFrame.Pause();//页面对象里面的继续方法
                        schedulerInfo.State = "继续";
                        break;
                }

                DgvSites.Refresh();
            }

            //附加处理
            else if (DgvSites.Columns[e.ColumnIndex].Name == "btnProcessor")
            {
                var schedulerInfo = ((SchedulerInfo)DgvSites.Rows[e.RowIndex].DataBoundItem);
              // schedulerInfo.SiteFrame.AppendProcess();
            }

            //重试错误任务
            else if(DgvSites.Columns[e.ColumnIndex].Name == "RetryError")
            {
                var schedulerInfo = ((SchedulerInfo)DgvSites.Rows[e.RowIndex].DataBoundItem);
               //schedulerInfo.SiteFrame.SchedulerInstance.RetryError();
            }

        }

        public static BindingList<SchedulerInfo> Schedulers = new BindingList<SchedulerInfo>();

        private static readonly object _locker = new object();
    }

    public class SchedulerInfo
    {
        public string SiteName { get; set; } // 

        public int ThreadCount { get; set; }

        public int Waiting { get; set; }   

        public int Error { get; set; }

        public SiteFrame SiteFrame { get; set; }

        public string LoginData { get; set; }

        public string State { get; set; }
        
    }
}
