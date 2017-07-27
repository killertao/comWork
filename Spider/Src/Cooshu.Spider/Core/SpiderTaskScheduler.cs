using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cooshu.Spider.Core
{
    /// <summary>
    /// 任务调度程序
    /// </summary>
    public class SpiderTaskScheduler
    {

        //TODO:后台可以控制一组抓取框架有单独任务调度器管理
        /// <summary>
        /// 单例
        /// </summary>
        private SpiderTaskScheduler()
        {
        }

        public static SpiderTaskScheduler Instance(string name, SiteFrame siteFrame)
        {
            if (_instances.ContainsKey(name))
            {
                return _instances[name];
            }

            var newInstance = new SpiderTaskScheduler {_siteFrame = siteFrame};
            _instances[name] = newInstance;

            return newInstance;
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void Start(int threadCount)
        {
            //已经有任务在运行,说明已经开始或未完全结束
            if (_taskPool!=null)
            {
                return;d
            }
            
            _taskPool = new List<Task>();
            _tokenSource = new CancellationTokenSource();

            //创建线程任务池
            for (var i = 0; i < threadCount; i++)
            {
                var i1 = i;
                var threadTask = new Task(()=> { Execute(i1); }, _tokenSource.Token);
                _taskPool.Add(threadTask);
                threadTask.Start();
            }

            WinMain.Instance.RefresScheduleInfo();
        }

        /// <summary>
        /// 暂定
        /// </summary>
        public void Pause(Action afterCancel=null)
        {
            if (_tokenSource?.IsCancellationRequested != false)
            {
                return;
            }

            WinMain.Instance.RefresScheduleInfo();
            
            _tokenSource.Cancel();
            afterCancel?.Invoke();
            _tokenSource = null;
            _taskPool=null;

        }

        /// <summary>
        /// 终止
        /// </summary>
        public void Stop(Action afterCancel=null)
        {
            Pause(afterCancel);
            _spiderTaskStack = new SpiderTaskStack();
            WinMain.Instance.RefresScheduleInfo();
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="spiderTask"></param>
        public void AddTask(SpiderTask spiderTask)
        {
            _spiderTaskStack.Push(spiderTask);
        }

        /// <summary>
        /// 重试错误队列中的内容
        /// </summary>
        public void RetryError()
        {
            _errorTaskQueue.ToList().ForEach(a =>
            {
                a.FailedTimes = 0;
                _spiderTaskStack.Push(a);
            });
            _errorTaskQueue.Clear();
        }

        /// <summary>
        /// 是否取消任务
        /// </summary>
        /// <returns></returns>
        public bool IsCancel()
        {
            return _tokenSource.IsCancellationRequested;
        }

        /// <summary>
        /// 执行的任务
        /// </summary>
        private void Execute(int number)
        {
            SpiderTask spiderTask = null;
            SpiderContext = new SpiderContext(number, _siteFrame);

            while (true)
            {
                if (_tokenSource?.IsCancellationRequested != false)
                {
                    spiderTask?.Page.LoginHelper.Logout(spiderTask);
                    return;
                }

                if (_spiderTaskStack.TryPop(out spiderTask) == false)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                spiderTask.SpiderContext = SpiderContext;
                

                try
                {
                    spiderTask.Execute(this);
                    WinMain.WriteLog($"{Thread.CurrentThread.ManagedThreadId},{number},{spiderTask.SpiderContext.Number}抓取成功:{spiderTask.Url}:{spiderTask.PostData?.Json()?? spiderTask.PostJson}\r\n");
                    WinMain.Instance.RefresScheduleInfo();
                }
                catch(Exception error)
                {
                    if (spiderTask.FailedTimes < 4)
                    {
                        spiderTask.FailedTimes++;
                        _spiderTaskStack.Push(spiderTask);
                        WinMain.WriteLog($"{Thread.CurrentThread.ManagedThreadId},{number},{spiderTask.SpiderContext.Number}抓取失败:{spiderTask.Url}:{spiderTask.PostData?.Json()}\r\n");
                        continue;
                    }

                    WinMain.WriteLog($"{Thread.CurrentThread.ManagedThreadId},{number},{spiderTask.SpiderContext.Number}抓取失败：{error}\r\n");
                    WinMain.WriteLog($"    {spiderTask.Url}:{spiderTask.PostData?.Json()}\r\n");

                    _errorTaskQueue.Push(spiderTask);

                    var path = $"{Directory.GetCurrentDirectory()}/Log/";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    lock (LogWriteLocker)
                    {
                        using (var writer = new StreamWriter($"{path}{DateTime.Now.ToString("yyyy-MM-dd")}.txt", true))
                        {
                            writer.Write(
                                $"{error}\r\n{spiderTask.Page.Name}\r\n{spiderTask.Url}\r\n{spiderTask.ResponseData?.ResponseUrl}\r\n{spiderTask.PostData?.Json()}\r\n\r\n\r\n");
                        }
                    }
                    WinMain.Instance.RefresScheduleInfo();
                }
            }
        }

        /// <summary>
        /// 实例
        /// </summary>
        private readonly static Dictionary<string,SpiderTaskScheduler> _instances = new Dictionary<string,SpiderTaskScheduler>();

        /// <summary>
        /// 任务池
        /// </summary>
        private List<Task> _taskPool;
        /// <summary>
        /// 错误队列
        /// </summary>
        public readonly ConcurrentStack<SpiderTask> _errorTaskQueue  = new ConcurrentStack<SpiderTask>();


        /// <summary>
        /// 上下文 
        /// </summary>
        public SpiderContext SpiderContext;

        /// <summary>
        /// 蜘蛛任务队列
        /// </summary>
        public SpiderTaskStack _spiderTaskStack = new SpiderTaskStack();

        /// <summary>
        /// 线程任务Token
        /// </summary>
        private CancellationTokenSource _tokenSource;

        private SiteFrame _siteFrame { get; set; }

        private static readonly object LogWriteLocker = new object();
    }
}