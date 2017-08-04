using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Crawler.Helper
{
    public class TaskPool
    {
        public List<Task> Takes = new List<Task>();

        public void EndReStart()
        {
            Thread.Sleep(1000 * 60 * 5); //等待5分钟。让其加入任务
            if (Takes.Count == 0)
            {
                TaskPool tp = new TaskPool();
                var at = new AssignTask(tp);
                at.Begin();
            }
            else
            {
                EndReStart();
            }
        }

        public   List<string> TempDocIds=new List<string>();
    }
}
