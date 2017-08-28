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

        private static bool closed;
        private static readonly object _locker = new object();//私有静态只读
        public static WinMain Instance { get; private set; } //存储当前窗口尸体
        public static BindingList<SchedulerInfo> Schedulers = new BindingList<SchedulerInfo>();
        //全部开始
        private void BtnStartClick(object sender, EventArgs e)
        {
        }

        //全部停止
        private void BtnStopClick(object sender, EventArgs e)
        {
            foreach (var schedulerInfo in DgvSites.Rows)
            {
                ((SchedulerInfo)schedulerInfo).SiteFrame.Pause();
            }
        }

        //退出
        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void WinMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            closed = true;
        }

        private void WinMain_Load(object sender, EventArgs e)
        {
            Instance = this;

            CheckForIllegalCrossThreadCalls = false;
            DgvSites.AutoGenerateColumns = false;
            DgvSites.DataSource = Schedulers;
            Schedulers.Add(new SchedulerInfo { SiteName = "法信网", ThreadCount = 1, SiteFrame = new FaXinFrame(), State = "开始" });
            //Schedulers.Add(new SchedulerInfo {SiteName = "中国裁判文书网", ThreadCount = 1, SiteFrame = new ZgcpwswFrame(), State = "开始"});
            ////Schedulers.Add(new SchedulerInfo { SiteName = "北大法易", ThreadCount = 1, SiteFrame = new BdfyFrame(), State = "开始" });
            //Schedulers.Add(new SchedulerInfo {SiteName = "北大法宝", ThreadCount = 1, SiteFrame = new BdfbFrame(), State = "开始" });
            //Schedulers.Add(new SchedulerInfo { SiteName = "多线程测试", ThreadCount = 1, SiteFrame = new TreadTestFrame(), State = "开始" });


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






        //单元格点击事件
        private void DgvSites_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //处理开始事件
            //开始
            if (DgvSites.Columns[e.ColumnIndex].Name == "Start")
            {
                var schedulerInfo = ((SchedulerInfo)DgvSites.Rows[e.RowIndex].DataBoundItem);//获取行数据对象  

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
                            // schedulerInfo.SiteFrame.Continue(threadCount);//页面对对象里面的暂停
                            schedulerInfo.State = "暂停";
                        }
                        break;
                    default:
                        schedulerInfo.SiteFrame.Pause();//页面对象里面的继续方法
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
            else if (DgvSites.Columns[e.ColumnIndex].Name == "RetryError")
            {
                var schedulerInfo = ((SchedulerInfo)DgvSites.Rows[e.RowIndex].DataBoundItem);
                //schedulerInfo.SiteFrame.SchedulerInstance.RetryError();
            }

        }




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
