using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows.Forms;
using Crawler.Helper;
using Crawler.Model;
namespace Crawler
{
    [ComVisible(true)]
    public partial class FormAssgin : Form
    {

        private PageOperator _po;//页面操作对象
        private const int MaxDoc = 2000;//每个任务最大可以抓取的数量
        private IEnumerable<keyType> _keyTypes;//接下来可以进行分任务的类型
        private string oldUrl;//缓存一个原来的链接，当页面没有数据或者是么有达到想要的页面，重新跳转到当前页面
        public List<SearchContent> TypeContents;// 页面的内容可以在第二次抓取的时候，未分配玩的在一次进行分配
        public FormAssgin(string url)
        {
            
            InitializeComponent();
            webBrowser.Navigate(new Uri(url));
            webBrowser.ObjectForScripting = this;
            _po = new PageOperator(webBrowser);
            oldUrl = url;

        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            string title = webBrowser.Document.Title;
            if (string.IsNullOrEmpty(title))
            {//表示dom还没有加载完成
                return;
            }
            string jsroot = "";
            if (title.Contains("导航已取消"))
            {
                webBrowser.Navigate(new Uri(oldUrl));
                return;
                ;
            }
            if (title.Contains("访问验证"))
            {//加入验证的js 然后回到后台填写验证吗

            }
            else if (title.Contains("首页 - 中国裁判文书网"))
            {

            }
            else if (title.Contains("列表页 - 中国裁判文书网"))
            {
                jsroot = "/assign.js";

            }
            if (!string.IsNullOrEmpty(jsroot))
            {
                HtmlElement script = webBrowser.Document.CreateElement("script");
                script.SetAttribute("type", "text/javascript");
                string str = File.ReadAllText(Application.StartupPath + jsroot);
                script.SetAttribute("text", str);
                webBrowser.Document.Body.AppendChild(script);
            }
        }

        //加载完毕回调回来
        public void Assign(bool isCondition)
        {
            //判断页面是有检索条件
            if (!isCondition)
            {
                webBrowser.Navigate(new Uri(oldUrl));
                return;
               
            }
            if (_po.GetSumDoc() < MaxDoc)
            {//当前页面可以被完全抓取的情况下创建子任务
                Program.form.AddWebForm(new FormChild(webBrowser.Url.ToString()), webBrowser.Url.ToString());
            }
            else
            {
                //模拟点击旁边的具体
                Task.Factory.StartNew(() =>
                {
                    SplitTask();
                });

            }
        }
        private string GetCondUrl(SearchContent tc)
        {
            string url = webBrowser.Url.ToString();
            //todo 这里也需要一个验证码参数 guid 和对应的key
            string pms = string.Format($"+{tc.Content}+++{tc.KeyType}:{tc.KeyType}");
            return url+pms;
        }

        private void SplitTask()
        {
            try
            {
                if (_keyTypes == null)
                {
                    _keyTypes = Enum.GetValues(typeof(keyType)).Cast<keyType>();
                }
                if (!_keyTypes.Any())
                {
                    //表示分到不能再分了
                    //记录不能再分url todo
                    return;
                }
                var keyfirst = _keyTypes.First();
                TypeContents = _po.GetTypeByWord(keyfirst);
                foreach (var item in TypeContents)
                {
                    var url = GetCondUrl(item);
                    if (item.CountDoc <= MaxDoc)
                    {
                        Program.form.AddWebForm(new FormChild(url),url);
                        TypeContents.Remove(item);
                    }
                    else
                    {
                        FormAssgin fa = new FormAssgin(url)
                        {
                            _keyTypes = _keyTypes.Where(m => m != keyfirst) //去掉已经分类过的
                        };
                        var assgins = LogicalDB.GetPreAssgin();
                        if (assgins.Count > 0)
                        {
                            fa.TypeContents = assgins;
                        }
                        Program.form.AddWebForm(new FormAssgin(url), url);
                        TypeContents.Remove(item);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }










        }
    }
}
