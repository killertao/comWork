using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crawler
{
    public partial class Async : Form
    {
  

        public Async()
        {
            InitializeComponent();
        }



        #region 异步调用
        private void button1_Click(object sender, EventArgs e)
        {
            Action<string> action = (a) => {
                richTextBox1.Text += a + Environment.NewLine;
                Thread.Sleep(5000);
            };
            execAction(action, "我加入一行数据");
        }
        public  void execAction(Action<string> action, string b)
        {
            //第一个参数式  委托的参数， 第二个参数式回掉函数，  第3个是状态参数
            IAsyncResult reslut = action.BeginInvoke(b, t =>
            {
                Thread.Sleep(5000);
                MessageBox.Show("我们完成了");
                //这里面是回调 ,但再等待的时候做任何操作
            }, null);

            //while (reslut.IsCompleted)
            //{
            //     Thread.Sleep(100);
            //     //可以再等待期间做其他操作，但是有睡眠时间的误差
            //}


            //reslut.AsyncWaitHandle.WaitOne();//等待异步完成
            // reslut.AsyncWaitHandle.WaitOne(20000);//等待多少毫秒就不等了
            //使用了waitone的话，就变成了同步的，会等待再执行下面的步骤

            // MessageBox.Show("11111");
            action.EndInvoke(reslut);//endinvoke 是等会委托运行完成后获取起返回值
            //endinvoke相对于 委托只能够调用一次   ，

        }
        #endregion

        #region 异步多线程

        private void button2_Click(object sender, EventArgs e)
        {


            Action<int> action = a =>
            {
                Thread.Sleep(200);
                Invoke(new Action<string>(showText), a.ToString());

            };
            for (int i = 0; i < 100; i++)
            {
                action.BeginInvoke(i, null, null);
            }


        }
        void showText(string a)
        {
            richTextBox1.Text += string.Format($"{a}\n");
        }

        public void  changeText(string a)
        {
           Invoke(new Action<string>(showText),a);
        }

        #endregion

        #region   Thread  老的线程 ， 
        private void Thread1()
        {
            ThreadStart start = () => { };//ThreadStart 一个没有参数没有返回类型的委托
            Thread th =new Thread(start); //thread默认是前端线程
            th.IsBackground = true;//是否是后台线程  后台线程的话，主线程退出，子线程立马关闭， 
                                                    //前台线程， 主线程退出，子线程会继续运行。
            th.Start();
            th.Abort();//销毁线程
        }



        #endregion

        #region  线程池  单例模式享元模式
        private void button3_Click(object sender, EventArgs e)
        {
            ManualResetEvent mre=new ManualResetEvent(false);
            ThreadPool.QueueUserWorkItem(t =>
            {
                mre.Set();
                Invoke(new Action<string>(showText), "1");
            
            });
            mre.WaitOne();//等待  当为false 的时候 必须执行 mer.Set 之后才会运行下面的代码
            mre.Reset();//重新关闭
            mre.Set();
            ThreadPool.QueueUserWorkItem(t =>
            {
                Invoke(new Action<string>(showText), "2");
            });
            ThreadPool.QueueUserWorkItem(t =>
            {
                Invoke(new Action<string>(showText), "3");
            });

        }



        #endregion

        private Task task = null;
        #region Task  基于线程池的线程  后台线程
        private void btnTask_Click(object sender, EventArgs e)
        {
            task = Task.Factory.StartNew(() =>
            {
                       
                changeText("我发士大夫士大夫 飞洒地方");

            });
           

           // task.Start();
            // task.Wait();//
            //task.ContinueWith()执行完成后执行

            //Task.WaitAll();
            //Task.WaitAny(); 回卡 主线程
            //Task.Factory.ContinueWhenAll(); // 全部任务完成之后执行。 不卡 主线程

          // Task.Delay()

            //Task.Factory.CancellationToken
        }


        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
              
        }
    }
}








