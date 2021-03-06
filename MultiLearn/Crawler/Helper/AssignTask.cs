﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Crawler.Model;
using static Crawler.Helper.PageOperator;

namespace Crawler.Helper
{
    public class AssignTask
    {
        private CustomWB _webBrowser =new CustomWB();
        private PageOperator _pageOperator;
        private const int MaxDoc = 500;
        private IEnumerable<keyType> _keyTypes;
        public TaskPool TaskPool;
        public AssignTask(TaskPool tp)
        {
            TaskPool = tp;
        }

        public void Begin()
        {
            DateTime date = LogicalDB.GetDate();
            string url = BaseUrl(date);
            Start(url);
            
        }

        public void Start(string url)
        {
            try
            {
                WebBrowser wb = new WebBrowser();
                wb.ScriptErrorsSuppressed = true;
                wb.Navigate("url");
                SplitTask();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private string BaseUrl(DateTime date)
        {
            string datesta = date.ToString("yyyy-MM-dd");
            string dateend = date.AddDays(1).ToString("yyyy-MM-dd");
            string url = string.Format($"http://wenshu.court.gov.cn/List/List?sorttype=1&conditions=searchWord+++{datesta} TO {dateend} +上传日期:{datesta} TO  {dateend}");
            return url;
        }

        private string GetCondUrl(SearchContent tc)
        {
            string url = _webBrowser.Url.ToString();
            string pms = string.Format($"+{tc.Content}+++{tc.KeyType}:{tc.Content}");
            return url;
        }

        //分配任务
        private void CreateTask(string url)
        {
            CustomWB wb = new CustomWB();
            wb.Url = new Uri(url); ;
            var st = new SpiderTask(wb, this);
            var task = new Task(a =>
           {
               var stp = a as SpiderTask;
               stp.Start();
           }, st);
            task.Start();


        }

        private void SplitTask()
        { 
            if (_keyTypes == null)
            {
                _keyTypes = Enum.GetValues(typeof(keyType)).Cast<keyType>();
            }
            if (_keyTypes.Any())
            {//表示分到不能再分了
                //记录不能再分url todo
                return;
            }
            foreach (var key in _keyTypes)
            {
                var contents = _pageOperator.GetTypeByWord(key); 
                foreach (var item in contents)
                {

                    var url = GetCondUrl(item);
                    if (item.CountDoc <= MaxDoc)
                    {
                        CreateTask(url);
                    }
                    else
                    {
                        var at = new AssignTask(TaskPool);
                        at._keyTypes = _keyTypes.Where(m => m != key);//去掉已经分类过的
                        at.Start(url);
                    }
                }
            }
        }
    }
}
