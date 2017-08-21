using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cooshu.Spider.Core
{
    public class SpiderTaskStack
    {
        public void Push(SpiderTask task)
        {
            switch (task.Priority)
            {
                case SpiderTaskPriority.First:
                    _first.Push(task);
                    break;
                case SpiderTaskPriority.Second:
                    _second.Push(task);
                    break;
                case SpiderTaskPriority.Third:
                    _third.Push(task);
                    break;
                default:
                    _third.Push(task);
                    break;
            }
        }

        public bool TryPop(out SpiderTask task)
        {
            return _first.TryPop(out task) || _second.TryPop(out task) || _third.TryPop(out task);
        }

        public int Count => _first.Count + _second.Count + _third.Count;

        readonly ConcurrentStack<SpiderTask> _first = new ConcurrentStack<SpiderTask>();

        readonly ConcurrentStack<SpiderTask> _second = new ConcurrentStack<SpiderTask>();

        readonly ConcurrentStack<SpiderTask> _third = new ConcurrentStack<SpiderTask>();
    }
}
