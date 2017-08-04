using System;
using System.Collections.Generic;
using System.Linq;

namespace Crawler.Helper
{
    public   class SpiderTask
    {
        private   AssignTask AssignTask { get; set; }

        private   PageOperator PageOperator { get; set; }

        private List<string> DocIds=new List<string>();
        public SpiderTask(CustomWB WebBrower, AssignTask at)
        {
            AssignTask = at;
            PageOperator=new PageOperator(WebBrower);
        }

        public void Start()
        {
            List<string> docids= PageOperator.GetDocIds();
            foreach (var id in docids)
            {
                if (!AssignTask.TaskPool.TempDocIds.Contains(id))
                {
                    docids.Add(id);//去重复添加
                }
            }
            AssignTask.TaskPool.TempDocIds.AddRange(docids);
            if (!PageOperator.NextPage())//返回false表示没有一下页了
            {
                //当前任务执行完毕  将所有docid 加入数据库重复的不添加 todo   添加完成之后抓取子页面
                
                return;
            }
            Start();
        }

      


    }
}
