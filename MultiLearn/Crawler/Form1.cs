using System;
using System.Net;
using System.Windows.Forms;

namespace Crawler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var s = GetExtenalIpAddress();
         
        }

        /// <summary>  
        /// 获取外网ip地址  
        /// </summary>  
        public static string GetExtenalIpAddress()
        {
            String url = "http://hijoyusers.joymeng.com:8100/test/getNameByOtherIp";
            string IP = "未获取到外网ip";
            try
            {
                //从网址中获取本机ip数据    
                WebClient client = new WebClient();
                client.Encoding = System.Text.Encoding.Default;
                var str = client.DownloadString(url);
                IP = str == "" ? IP : str;
                client.Dispose();
            }
            catch (Exception) { }
            return IP;
        }

        private void btnFindIP_Click(object sender, EventArgs e)
        {
            CurrentIP.Text = GetExtenalIpAddress();
            Helper.GetData();
        }

        public int c = a + b;
        private static int a;
        private static int b;
     
    }



}
