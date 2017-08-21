using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Crawler.Model;
using static Crawler.Helper.SqlHelper;
namespace Crawler.Helper
{
    public class LogicalDB
    {
        private static readonly CusDbContent Db = new CusDbContent();
        //获取抓取那一天的总任务
        public static DateTime GetDate()
        {
            var dates = Db.Set<SearchedDate>();
            var maxdate = dates.Max(d => d.SDate);
            return maxdate == null ? DateTime.Now : DateTime.Now.AddDays(-1);

        }

        //插入子
        public static void AddDocId(List<string> docs)
        {
            var s = docs.Cast<DocIds>();
            //todo 再这里判断下，数据库有咩有这一项，有的话不填加
            Db.Set<DocIds>().AddRange(s);
            Db.SaveChanges();
        }

        public static List<SearchContent> GetPreAssgin()
        {
            //这里只获取100个任务 等执行完毕再来
            var scs = Db.Set<SearchContent>().Where(sc => sc.Status == 0).Take(100).ToList();
            SqlHelper.ExecuteNonQuery(GetConnection(), CommandType.Text, "update SearchContent set Status=-1 where  Id in select top 100 Id from SearchContent where Status=0");
            //将这些状态设置为正则执行 -1
            return scs;
        }

        public static List<ChildPre> GetPreChild(int count)
        {
            var childs = Db.Set<ChildPre>().Where(sc => sc.Status == 0).Take(count);
            SqlHelper.ExecuteNonQuery(GetConnection(), CommandType.Text, "update ChildPre set Status=-1 where  Id in select top 100 Id from ChildPre where Status=0");
            return childs.ToList();
        }

        public static bool ChangePreChid(string url)
        {
            var childpre = Db.Set<ChildPre>().FirstOrDefault(c => c.Url == url);
            childpre.Status = 1;//完成
            return Db.SaveChanges()>0;
        }
    }
}
