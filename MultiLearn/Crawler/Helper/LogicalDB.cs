using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Crawler.Helper
{
    public class LogicalDB
    {
        private  static readonly   CusDbContent Db = new CusDbContent(); 
        //获取抓取那一天的总任务
        public static DateTime GetDate()
        {
            var dates = Db.Set<SearchedDate>();
            var maxdate = dates.Max(d => d.SDate);
            return maxdate == null ? DateTime.Now : DateTime.Now.AddDays(-1);

        }
        public  static void AddDocId(List<string> docs)
        {
            var s = docs.Cast<DocIds>();
            Db.Set<DocIds>().AddRange(s);
            Db.SaveChanges();
        }

       

    }
}
